using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebsiteBanHang.Models;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings), "Cấu hình EmailSettings không được để trống.");
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrEmpty(_emailSettings.SenderEmail))
        {
            throw new ArgumentNullException(nameof(_emailSettings.SenderEmail), "SenderEmail không được để trống.");
        }
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "Email người nhận không được để trống.");
        }

        using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
        {
            client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            client.EnableSsl = true;  // Đảm bảo bật SSL
            client.UseDefaultCredentials = false; // Không dùng thông tin đăng nhập mặc định
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }

}