using System.Net;
using System.Net.Mail;

namespace EcoMeal.Backend.Infrastructure;
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string userName);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    public async Task SendWelcomeEmailAsync(string toEmail, string userName)
    {
        var emailSettings = _config.GetSection("EmailSettings");
        var host = emailSettings["Host"];
        var port = int.Parse(emailSettings["Port"] ?? "25");
        var user = emailSettings["User"];
        var password = emailSettings["Password"];
        var fromEmail = emailSettings["FromEmail"];
        var fromName = emailSettings["FromName"];

        using (var client = new SmtpClient(host, port))
        {
            client.Credentials = new NetworkCredential(user, password);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = "Bine ai venit in familia EcoMeal!",
                Body = $"Salut {userName},\n\nBine ai venit la EcoMeal! Suntem încântați să te avem alături de noi.\n\nEchipa EcoMeal",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}