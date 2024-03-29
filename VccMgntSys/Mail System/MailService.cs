﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VccMgntSys.Mail_System;

namespace VccMgntSys.Mail_System
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService()
        {
        }

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            /*
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var attachment in mailRequest.Attachments)
                {
                    if (attachment.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            attachment.CopyTo(ms);
                            fileBytes = ms.ToArray();

                        }

                        builder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                    }
                }
            }
            */
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
