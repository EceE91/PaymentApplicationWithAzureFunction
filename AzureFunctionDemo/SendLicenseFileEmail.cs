using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace AzureFunctionDemo;

public static class SendLicenseFileEmail
{
    [FunctionName("SendLicenseFileEmail")]
    public static void Run([BlobTrigger("licenses/{name}", 
            Connection = "AzureWebJobsStorage")]string licenseFileContents,
        [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
        string name, 
        ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {licenseFileContents.Length} Bytes");

        var email = "test@hotmail.com"; //Regex.Match(licenseFileContents, @"^email\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value;
        log.LogInformation($"Got order from {email}\n License file name: {name}");

        message = new SendGridMessage
        {
            From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender"))
        };
        message.AddTo(email);
        var plainTextBytes = Encoding.UTF8.GetBytes(licenseFileContents);
        var base64 = Convert.ToBase64String(plainTextBytes);
        message.AddAttachment(name,base64,"text/plain");
        message.Subject = "Your license file";
        message.HtmlContent = "Thanks for your order";
    }
}