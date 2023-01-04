using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionDemo;

public static class GenerateLicenseFile
{
    // this is an output binding, which means we're going to be creating a blob,
    // and we're writing text to that blob storage
    [FunctionName("GenerateLicenseFile")]
    public static async Task Run(
        [QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Order order,
        IBinder binder
        ,ILogger log)
    {
        // GenerateLicenseFile function will be triggered
        // when we add a new order object to the "orders" queue
        log.LogInformation($"C# Queue trigger function processed: {order}");

        var outputBlob = await binder.BindAsync<TextWriter>(
            new BlobAttribute($"licenses/{order.OrderId}.lic")
            {
                Connection = "AzureWebJobsStorage"
            }
        );

        await outputBlob.WriteLineAsync($"OrderId: {order.OrderId}");
        await outputBlob.WriteLineAsync($"Email: {order.Email}");
        await outputBlob.WriteLineAsync($"ProductId: {order.ProductId}");
        await outputBlob.WriteLineAsync($"PurchaseDate: {DateTime.UtcNow}");
        var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(
            System.Text.Encoding.UTF8.GetBytes(order.Email + "secret"));
        await outputBlob.WriteLineAsync($"SecretCode: {BitConverter.ToString(hash).Replace("-", "")}");
    }
}