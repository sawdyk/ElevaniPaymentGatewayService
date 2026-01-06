using AutoMapper;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static ElevaniPaymentGateway.Core.Helpers.Pagination.QueryableExtensions;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IAccountNotificationEmailService _accountNotificationEmailService;
        private readonly IHttpContextHelperService _httpContextHelperService;
        private readonly UserContextDto _userContext;
        private readonly IUserQuery _userQuery;
        private readonly IUserRoleQuery _userRoleQuery;
        private readonly IMerchantQuery _merchantQuery;
        private readonly IBaseRepository<MerchantUser> _merchantUserRepository;
        private readonly IMerchantUserQuery _merchantUserQuery;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly IRoleQuery _roleQuery;
        public UserService(ILogger<UserService> logger, RoleManager<Role> roleManager, UserManager<User> userManager,
            IHttpContextHelperService httpContextHelperService, IUserQuery userQuery, IUserRoleQuery userRoleQuery,
            IBaseRepository<User> userRepository, IBaseRepository<MerchantUser> merchantUserRepository, IMerchantQuery merchantQuery,
            IMerchantUserQuery merchantUserQuery, IMapper mapper, IBaseRepository<UserRole> userRoleRepository, ISqlTransactionService sqlTransactionService,
            IRoleQuery roleQuery, IAccountNotificationEmailService accountNotificationEmailService)
        {
            _logger = logger;
            _merchantUserRepository = merchantUserRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextHelperService = httpContextHelperService;
            _userContext = _httpContextHelperService.UserContext();
            _userQuery = userQuery;
            _userRoleQuery = userRoleQuery;
            _userRepository = userRepository;
            _merchantQuery = merchantQuery;
            _merchantUserQuery = merchantUserQuery;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _sqlTransactionService = sqlTransactionService;
            _roleQuery = roleQuery;
            _accountNotificationEmailService = accountNotificationEmailService;
        }

        public async Task<GenericPagedResponse<UserRoleDto>> AdminUsersAsync(PaginationParams paginationParams)
        {
            try
            {
                var adminRole = await _roleManager.FindByNameAsync(nameof(UserRoles.Admin));
                var superAdminRole = await _roleManager.FindByNameAsync(nameof(UserRoles.SuperAdmin));

                var adminUsersQuery = await _userRoleQuery.ListAsync(x => x.RoleId == adminRole.Id || x.RoleId == superAdminRole.Id, true);

                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<UserRole>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<UserRole>>
                        {
                            new(){ PropertyExpression = x => x.User.FirstName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.LastName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.Email, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.UserName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.PhoneNumber, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.Status, SearchType = SearchType.Equals },
                        }
                    };

                    adminUsersQuery = adminUsersQuery.DynamicSearch(searchConfig);
                }

                var result = await adminUsersQuery.ToPagedResultAsync(paginationParams);
                var userRoleDto = _mapper.Map<PagedResult<UserRoleDto>>(result);

                return GenericPagedResponse<UserRoleDto>.Success(userRoleDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> CreateAdminUserAsync(AdminUserRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (existingUser is not null) throw new DuplicateException($"User with email address '{request.EmailAddress}' already exists.");

                var role = await _roleManager.FindByNameAsync(request.Role.ToString());
                if (role is null) throw new NotFoundException($"Role does not exist");

                if (role.Name.Equals(nameof(UserRoles.MerchantUser)) || role.Name.Equals(nameof(UserRoles.MerchantAdmin)))
                    throw new GenericException($"You cannot create an admin user using a merchant role");

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.EmailAddress,
                    EmailConfirmed = true,
                    Status = UserStatus.Active,
                };

                var defaultPassword = RandomGeneratorHelper.RandomString(12) + "$";
                _logger.LogInformation($"Default password generated for user {user.FirstName} ==> {defaultPassword}");
                var userResult = await _userManager.CreateAsync(user, defaultPassword);
                if (!userResult.Succeeded) throw new GenericException("An error occurred while trying to create admin account");
                else
                {
                    var userRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!userRoleResult.Succeeded) throw new GenericException("An error occurred while trying to create admin user role");
                }

                //email sending to admin user
                await _accountNotificationEmailService.SendNewAdminUserMailAsync(user, defaultPassword);
                
                var userDto = _mapper.Map<UserDto>(user);
                return GenericResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DuplicateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> CreateMerchantUserAsync(MerchantUserRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (existingUser is not null) throw new DuplicateException($"User with email address '{request.EmailAddress}' already exists.");

                var merchant = await _merchantQuery.GetByAsync(x => x.Id == request.MerchantId);
                if (merchant is null) throw new NotFoundException($"Merchant does not exist");

                var role = await _roleManager.FindByNameAsync(request.Role.ToString());
                if (role is null) throw new NotFoundException($"Role does not exist");

                if (role.Name.Equals(nameof(UserRoles.Admin)) || role.Name.Equals(nameof(UserRoles.SuperAdmin)))
                    throw new GenericException($"You cannot create a merchant user using an admin user role");

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.EmailAddress,
                    EmailConfirmed = true,
                    Status = UserStatus.Active,
                };

                var defaultPassword = RandomGeneratorHelper.RandomString(12) + "$";
                _logger.LogInformation($"Default password generated for user {user.FirstName} ==> {defaultPassword}");
                var userResult = await _userManager.CreateAsync(user, defaultPassword);
                if (!userResult.Succeeded) throw new GenericException("An error occurred while trying to create merchant account");
                else
                {
                    var userRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!userRoleResult.Succeeded) throw new GenericException("An error occurred while trying to create merchant user role");
                }

                var merchantUser = new MerchantUser
                {
                    MerchantId = request.MerchantId,
                    UserId = user.Id,
                };

                _merchantUserRepository.Add(merchantUser);
                await _merchantUserRepository.SaveChangesAsync();

                //email sending to merchant user
                await _accountNotificationEmailService.SendNewAdminUserMailAsync(user, defaultPassword);

                var userDto = _mapper.Map<UserDto>(user);

                return GenericResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DuplicateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse> DeleteAsync(Guid id)
        {
            try
            {
                var user = (await _userQuery.GetByAsync(x => x.Id == id));
                if (user is null) throw new NotFoundException("User does not exist");

                _userRepository.Delete(user);
                await _userRepository.SaveChangesAsync();

                return GenericResponse.Success();
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericPagedResponse<UserRoleDto>> MerchantUsersAsync(PaginationParams paginationParams)
        {
            try
            {
                var merchantAdminRole = await _roleManager.FindByNameAsync(nameof(UserRoles.MerchantAdmin));
                var merchantUserRole = await _roleManager.FindByNameAsync(nameof(UserRoles.MerchantUser));

                var merchantsQuery = await _userRoleQuery.ListAsync(x => x.RoleId == merchantAdminRole.Id || x.RoleId == merchantUserRole.Id, true);

                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<UserRole>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<UserRole>>
                        {
                            new(){ PropertyExpression = x => x.User.FirstName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.LastName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.Email, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.UserName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.PhoneNumber, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.User.Status, SearchType = SearchType.Equals },
                        }
                    };

                    merchantsQuery = merchantsQuery.DynamicSearch(searchConfig);
                }

                var result = await merchantsQuery.ToPagedResultAsync(paginationParams);
                var userRoleDto = _mapper.Map<PagedResult<UserRoleDto>>(result);

                return GenericPagedResponse<UserRoleDto>.Success(userRoleDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericPagedResponse<UserDto>> MerchantUsersByMerchantIdAsync(string merchantId, PaginationParams paginationParams)
        {
            try
            {
                var merchantUsersQuery = (await _merchantUserQuery.ListAsync(x => x.MerchantId == merchantId, true))
                    .Select(x => x.User);

                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<User>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<User>>
                        {
                            new(){ PropertyExpression = x => x.FirstName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.LastName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Email, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.UserName, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.PhoneNumber, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Status, SearchType = SearchType.Equals },
                        }
                    };

                    merchantUsersQuery = merchantUsersQuery.DynamicSearch(searchConfig);
                }

                var result = await merchantUsersQuery.ToPagedResultAsync(paginationParams);
                var userDto = _mapper.Map<PagedResult<UserDto>>(result);

                return GenericPagedResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> SetStatusAsync(UserStatusRequest request)
        {
            try
            {
                var user = (await _userQuery.GetByAsync(x => x.Id == request.Id, false));
                if (user is null) throw new NotFoundException("User does not exist");

                user.Status = request.Status;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = _userContext.UserId.ToString();

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                var userDto = _mapper.Map<UserDto>(user);
                return GenericResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> UpdateAdminUserAsync(UpdateUserRequest request)
        {
            try
            {
                var user = (await _userQuery.GetByAsync(x => x.Id == request.Id, false));
                if (user is null) throw new NotFoundException("User does not exist");

                var role = await _roleQuery.GetByAsync(x => x.Name == request.Role.ToString());
                if (role is null) throw new NotFoundException($"Role does not exist");

                if (role.Name.Equals(nameof(UserRoles.MerchantUser)) || role.Name.Equals(nameof(UserRoles.MerchantAdmin)))
                    throw new GenericException($"Invalid role");

                user.FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName;
                user.LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName;
                user.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? user.PhoneNumber : request.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = _userContext.UserId.ToString();

                var transaction =  await _sqlTransactionService.BeginTransactionAsync();

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                var existingRoles = await _userManager.GetRolesAsync(user);
                if (existingRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, existingRoles);

                var userRoleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
                if (!userRoleResult.Succeeded) throw new GenericException("An error occurred while trying to update organization user role");

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                var userDto = _mapper.Map<UserDto>(user);
                return GenericResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> UpdateMerchantUserAsync(UpdateMerchantUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user is null) throw new NotFoundException($"User does not exist");

                var merchant = await _merchantQuery.GetByAsync(x => x.Id == request.MerchantId);
                if (merchant is null) throw new NotFoundException($"Merchant does not exist");

                var role = await _roleQuery.GetByAsync(x => x.Name == request.Role.ToString());
                if (role is null) throw new NotFoundException($"Role does not exist");

                if (role.Name.Equals(nameof(UserRoles.Admin)) || role.Name.Equals(nameof(UserRoles.SuperAdmin)))
                    throw new GenericException($"Invalid role");

                user.FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName;
                user.LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName;
                user.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? user.PhoneNumber : request.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = _userContext.UserId.ToString();

                var transaction = await _sqlTransactionService.BeginTransactionAsync();

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                var userRole = await _userRoleQuery.GetByAsync(x => x.UserId == user.Id);
                if (userRole is not null)
                {
                    userRole.UserId = user.Id;
                    userRole.RoleId = role.Id;
                    userRole.UpdatedAt = DateTime.UtcNow;
                    userRole.UpdatedBy = _userContext.UserId.ToString();

                    _userRoleRepository.Update(userRole);
                    await _userRoleRepository.SaveChangesAsync();
                }

                var merchantUser = await _merchantUserQuery.GetByAsync(x => x.UserId == user.Id);
                if(merchantUser is not null)
                {
                    merchantUser.MerchantId = request.MerchantId;
                    merchantUser.UserId = user.Id;
                    merchantUser.UpdatedAt = DateTime.UtcNow;
                    merchantUser.UpdatedBy = _userContext.UserId.ToString();

                    _merchantUserRepository.Update(merchantUser);
                    await _merchantUserRepository.SaveChangesAsync();
                }

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                var userDto = _mapper.Map<UserDto>(user);
                return GenericResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DuplicateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserRoleDto>> UserByIdAsync(Guid id)
        {
            try
            {
                var user = (await _userRoleQuery.GetByAsync(x => x.UserId == id, true));
                if (user is null) throw new NotFoundException("User does not exist");

                var userRoleDto = _mapper.Map<UserRoleDto>(user);

                return GenericResponse<UserRoleDto>.Success(userRoleDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
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
