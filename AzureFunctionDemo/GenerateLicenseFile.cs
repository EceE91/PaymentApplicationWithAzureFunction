using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionDemo;

public static class GenerateLicenseFile
{
    // this is an output binding, which means we're going to be creating a blob,
    // and we're writing text to that blob storage
    [FunctionName("GenerateLicenseFile")]
    public static void Run(
        [QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Order order,
        [Blob("licences/{rand-guid}.lic")] TextWriter outputBlob,ILogger log)
    {
        // GenerateLicenseFile function will be triggered
        // when we add a new order object to the "orders" queue
        log.LogInformation($"C# Queue trigger function processed: {order}");
        
        outputBlob.WriteLine($"OrderId: {order.OrderId}");
        outputBlob.WriteLine($"ProductId: {order.ProductId}");
        outputBlob.WriteLine($"Price: {order.Price}");
        outputBlob.WriteLine($"OrderDate: {DateTime.Now}");
        outputBlob.WriteLine($"OrderId: {order.OrderId}");
        var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(order.Email + "secret"));
        
        outputBlob.WriteLine($"SecretCode:{BitConverter.ToString(hash).Replace("-","")}");
    }
}