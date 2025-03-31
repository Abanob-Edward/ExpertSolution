using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }

    }
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public class ConfigConstants
    {
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailCC2 { get; set; }
        public string MailBCC { get; set; }
        public string Fees { get; set; }
     
    }
}
