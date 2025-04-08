using MailKit.Net.Smtp;
using MimeKit;

namespace _0_Framework.Application.Email;

public class EmailService : IEmailService
{
    public void SendEmail(string title, string messageBody, string destination)
    {
        var message = new MimeMessage();

        var from = new MailboxAddress("Atriya", "hosein.eskandariii1994@gmail.com");
        message.From.Add(from);

        var to = new MailboxAddress("User", destination);
        message.To.Add(to);

        message.Subject = title;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<h1>{messageBody}</h1>"
        };

        message.Body = bodyBuilder.ToMessageBody();

        var client = new SmtpClient();
        client.Connect("https://localhost/", 5001, false); //Config your host address
        client.Authenticate("test@atriya.com", "Atriya.123456");
        client.Send(message);
        client.Disconnect(true);
        client.Dispose();
    }
}