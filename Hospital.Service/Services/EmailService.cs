﻿using Hospital.Core.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hospital.Service.OptionsModels;

namespace Hospital.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smptClient = new SmtpClient();

            smptClient.Host = _emailSettings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Localhost Şifre sıfırlama linki";
            mailMessage.Body = @$"<h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız</h4>
            <p><a href='{resetPasswordEmailLink}'>Şifre yenileme link</a></p>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }

        public async Task SendTakeAppointmentEmail(string toEmail)
        {
            var smptClient = new SmtpClient();

            smptClient.Host = _emailSettings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);

            mailMessage.Subject = "Randevu Bilgilendirme";
            mailMessage.Body = @$"<h4>Randevunuz kayıt edilmiştir</h4>";
           
            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }

        public async Task SendConfirmCodeEmail(int ConfirmCode, string ToEmail)
        {
            var smptClient = new SmtpClient();

            smptClient.Host = _emailSettings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Localhost Üye Aktivasyon Linki";
            mailMessage.Body = @$"<h4>Hesabınızı aktif etmek için aşağıdaki kodu kullanınız</h4>
            <p><a>Üye aktivasyon kodu: {ConfirmCode}</a></p>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }
    }
}
