using System.Net.Mail;
using MimeKit;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class EmailRequest
    {
        public List<string>? RecipientTo { get; set; }
        public List<string>? RecipientCC { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<LinkedResource>? LinkedResource { get; set; }
        public List<EmailAttachments> Attachments { get; set; }
    }

    public class EmailAttachments
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public ContentType ContentType { get; set; } //new ContentType("application","pdf")
    }
}
