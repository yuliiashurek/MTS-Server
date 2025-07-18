﻿using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Server.Core.Interfaces;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        var smtpUser = _configuration["Email:SmtpUser"];
        var smtpPass = _configuration["Email:SmtpPass"];
        var fromEmail = _configuration["Email:FromEmail"];

        var smtpClient = new SmtpClient(smtpHost)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendLowStockAlert(string toEmail, string materialName, decimal currentStock, decimal minimumStock)
    {
        var subject = $"[Увага] Низький запас матеріалу: {materialName}";
        var body = $@"
<html>
<head>
  <style>
    body {{
      font-family: 'Segoe UI', sans-serif;
      background-color: #f9f9f9;
      margin: 0;
      padding: 0;
    }}
    .header {{
      background-color: #1B4332;
      color: white;
      padding: 20px;
      font-size: 24px;
      font-weight: bold;
    }}
    .container {{
      padding: 30px;
    }}
    .alert {{
      background-color: #ffe5e5;
      border: 1px solid #ffaaaa;
      padding: 15px;
      border-radius: 5px;
      color: #a94442;
    }}
    .footer {{
      margin-top: 30px;
      font-size: 12px;
      color: #777;
    }}
  </style>
</head>
<body>
  <div class='header'>
    ⚠ MTS – Облік матеріалів
  </div>
  <div class='container'>
    <div class='alert'>
      <p><strong>Увага!</strong> Запас матеріалу <strong>{materialName}</strong> зменшився до <strong>{currentStock}</strong> одиниць, що дорівнює або менше мінімального рівня <strong>{minimumStock}</strong>.</p>
    </div>
    <p>Рекомендується перевірити залишки та за потреби здійснити замовлення постачання.</p>
    <div class='footer'>
      Це автоматичне повідомлення. Будь ласка, не відповідайте на нього.
    </div>
  </div>
</body>
</html>";


        await SendAsync(toEmail, subject, body);
    }

}
