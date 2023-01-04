using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace AzureFunctionDemo;

public static class EmailLicenseFiles
{
    [FunctionName("EmailLicenseFiles")]
    public static void Run(
        [BlobTrigger("licenses/{orderId}.lic")]string licenseFileContents,
        [SendGrid(ApiKey = "SendGridApiKey")] ICollector<SendGridMessage> sender,
        [Table("orders","PKorders","{orderId}")] Order order,
        string orderId,
        ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{orderId}.lic \n Size: {licenseFileContents.Length} Bytes");

        //look up the database to find what the email address is
        var email = order.Email;
        log.LogInformation($"Got order from {email}\n License file name: {orderId}.lic");

        var message = new SendGridMessage
        {
            From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender"))
        };
        message.AddTo(email);
        var plainTextBytes = Encoding.UTF8.GetBytes(licenseFileContents);
        var base64 = Convert.ToBase64String(plainTextBytes);
        message.AddAttachment($"{orderId}.lic",base64,"text/plain");
        message.Subject = "Your license file";
        message.HtmlContent = "Thanks for your order";
        
        // send the email only when it doesn't end @test.com
        if(!email.EndsWith("@test.com"))
            sender.Add(message);
    }
}