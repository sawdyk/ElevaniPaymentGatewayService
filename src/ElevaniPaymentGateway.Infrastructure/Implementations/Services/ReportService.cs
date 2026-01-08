using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net;
using static ElevaniPaymentGateway.Core.Helpers.Pagination.QueryableExtensions;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IReportDataService _reportDataService;
        public ReportService(ILogger<ReportService> logger, IReportDataService reportDataService)
        {
            _logger = logger;
            _reportDataService = reportDataService;
        }

        public async Task<GenericResponse<ReportResponse<Transaction>>> TransactionsReportAsync(TransactionReportRequest request, PaginationParams paginationParams)
        {
            var reportResponse = new GenericResponse<ReportResponse<Transaction>>();
            var transactionsQuery = await _reportDataService.TransactionsReportAsync(request);

            if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
            {
                var searchConfig = new SearchConfig<Transaction>
                {
                    SearchTerm = paginationParams.SearchTerm,
                    SearchProperties = new List<SearchProperty<Transaction>>
                        {
                            new(){ PropertyExpression = x => x.Reference, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Currency, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Amount, SearchType = SearchType.Contains },
                        }
                };

                transactionsQuery = transactionsQuery.DynamicSearch(searchConfig);
            }

            var paginatedResult = await transactionsQuery.ToPagedResultAsync(paginationParams);
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("NonCommercial"); // license
                using (ExcelPackage? excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add($"Transactions_Report");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    //Header of table  
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "S/N";
                    workSheet.Cells[1, 2].Value = "Merchant ID";
                    workSheet.Cells[1, 3].Value = "Reference";
                    workSheet.Cells[1, 4].Value = "Currency";
                    workSheet.Cells[1, 5].Value = "Amount";
                    workSheet.Cells[1, 6].Value = "Narration";
                    workSheet.Cells[1, 7].Value = "Country Code";
                    workSheet.Cells[1, 8].Value = "Status";
                    workSheet.Cells[1, 9].Value = "Date Created";

                    if (transactionsQuery.Any())
                    {
                        //Body of table  
                        int recordIndex = 2; //starts from the second column after the header
                        var transactions = await transactionsQuery.ToListAsync();
                        foreach (var trans in transactions)
                        {
                            workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                            workSheet.Cells[recordIndex, 2].Value = $"{trans.MerchantId}";
                            workSheet.Cells[recordIndex, 3].Value = trans.Reference;
                            workSheet.Cells[recordIndex, 4].Value = trans.Currency;
                            workSheet.Cells[recordIndex, 5].Value = trans.Amount;
                            workSheet.Cells[recordIndex, 6].Value = trans.Narration;
                            workSheet.Cells[recordIndex, 7].Value = trans.CountryCode;
                            workSheet.Cells[recordIndex, 8].Value = trans.Status;
                            workSheet.Cells[recordIndex, 9].Value = trans.CreatedAt.ToString("dd/MM/yyyy");
                            recordIndex++;
                        }

                        //Cell fitting formatters
                        for (int col = 1; col <= 9; col++)
                            workSheet.Column(col).AutoFit();
                    }

                    //response
                    byte[] excelBytes = await excel.GetAsByteArrayAsync();
                    string base64Data = Convert.ToBase64String(excelBytes);

                    reportResponse.Code = HttpStatusCode.OK;
                    reportResponse.Message = "Successful";
                    reportResponse.Data = new ReportResponse<Transaction>
                    {
                        Data = paginatedResult,
                        FileName = $"Transaction Report_{request.StartDate.ToString("ddMMyyyy")}_{request.EndDate.ToString("ddMMyyyy")}",
                        Base64Data = base64Data
                    };
                }

                return reportResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                    $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
