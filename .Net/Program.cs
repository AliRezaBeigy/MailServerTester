using System;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace MailServerTester
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("[+] Mail Server Started");
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.example.json", true, true)
                .AddEnvironmentVariables().Build();

            var message = new MimeMessage();
            message.Subject = "Test Subject";
            message.To.Add(new MailboxAddress(config["To:Name"], config["To:Address"]));
            message.From.Add(new MailboxAddress(config["From:Name"], config["From:Address"]));
            message.Body = new TextPart(TextFormat.Plain) {Text = "This is a test message from Mail Server Tester."};

            try
            {
                using var client = new SmtpClient();
                client.Connect(config["Host"], int.Parse(config["Port"]), bool.Parse(config["UseSSL"]));
                
                client.Authenticate(config["Auth:Username"], config["Auth:Password"]);
                client.Send(message);
                
                client.Disconnect(true);
                
                Console.WriteLine("[+] Email send successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[!] An Error Occured: " + ex.Message);
            }
        }
    }
}