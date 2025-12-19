using MimeKit;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Helpers
{
    public class EmailResourceHelpers
    {
        public static string FixBase64ForImage(string Image)
        {
            StringBuilder sbText = new StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty); sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }
        public static LinkedResource linkedResource(string imagePath, string contentId)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);
            byte[] bitMap = Convert.FromBase64String(FixBase64ForImage(base64Image));
            MemoryStream streamBitMap = new MemoryStream(bitMap);

            LinkedResource resource = new LinkedResource(streamBitMap, MediaTypeNames.Image.Jpeg);
            resource.ContentId = contentId;
            resource.TransferEncoding = TransferEncoding.Base64;

            return resource;
        }

        public static List<LinkedResource> commonLinkedResource()
        {
            string logo = "wwwroot/MailTemplates/Images/logo.png";
            string facebook = "wwwroot/MailTemplates/Images/facebook.png";
            string instagram = "wwwroot/MailTemplates/Images/instagram.png";
            string twitter = "wwwroot/MailTemplates/Images/twitter.png";
            string linkedin = "wwwroot/MailTemplates/Images/linkedin.png";
            string whatsapp = "wwwroot/MailTemplates/Images/whatsapp.png";

            var logoRes = linkedResource(logo, "logo");
            var facebookRes = linkedResource(facebook, "facebook");
            var instagramRes = linkedResource(instagram, "instagram");
            var twitterRes = linkedResource(twitter, "twitter");
            var linkedInRes = linkedResource(linkedin, "linkedin");
            var whatsappRes = linkedResource(whatsapp, "whatsapp");

            return new List<LinkedResource>
            {
                logoRes,
                facebookRes,
                instagramRes,
                twitterRes,
                linkedInRes,
                whatsappRes
            };
        }

        public static BodyBuilder mimeKitCommonLinkedResource()
        {
            //string logo = "wwwroot/MailTemplates/Images/logo.png";
            //string facebook = "wwwroot/MailTemplates/Images/facebook.png";
            //string instagram = "wwwroot/MailTemplates/Images/instagram.png";
            //string twitter = "wwwroot/MailTemplates/Images/twitter.png";
            //string linkedin = "wwwroot/MailTemplates/Images/linkedin.png";
            //string whatsapp = "wwwroot/MailTemplates/Images/whatsapp.png";

            var builder = new BodyBuilder();
            //var logoImage = builder.LinkedResources.Add(logo);
            //logoImage.ContentId = "logo";
            //var facebookImage = builder.LinkedResources.Add(facebook);
            //facebookImage.ContentId = "facebook";
            //var instagramImage = builder.LinkedResources.Add(instagram);
            //instagramImage.ContentId = "instagram";
            //var twitterImage = builder.LinkedResources.Add(twitter);
            //twitterImage.ContentId = "twitter";
            //var linkedinImage = builder.LinkedResources.Add(linkedin);
            //linkedinImage.ContentId = "linkedin";
            //var whatsappImage = builder.LinkedResources.Add(whatsapp);
            //whatsappImage.ContentId = "whatsapp";

            return builder;
        }
    }
}
