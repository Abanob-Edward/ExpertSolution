using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.Interfaces;
using System.Net;

namespace Shared.Helper
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ConfigConstants _configConstants;

        public MailService(IOptions<MailSettings> mailSettings, IOptions<ConfigConstants> configConstants)
        {
            _mailSettings = mailSettings.Value;
            _configConstants = configConstants.Value;
        }


        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Cc.Add(MailboxAddress.Parse(_configConstants.MailCC));
                email.Cc.Add(MailboxAddress.Parse(_configConstants.MailCC2));
                email.Bcc.Add(MailboxAddress.Parse(_configConstants.MailBCC));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.None);
                //var creds = new NetworkCredential("noreply", _mailSettings.Password, "arado.org");

                //smtp.Authenticate(creds);
                //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                //smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                //await smtp.AuthenticateAsync(new NetworkCredential(_mailSettings.Mail, _mailSettings.Password));


                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }
    }
}