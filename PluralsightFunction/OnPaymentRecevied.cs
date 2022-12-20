using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PluralsightFunction;

public static class OnPaymentRecevied
{
    [FunctionName("OnPaymentRecevied")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [Queue("orders")] IAsyncCollector<Order> orderQueue, ILogger log)
    {
        log.LogInformation("Payment received");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var order = JsonConvert.DeserializeObject<Order>(requestBody);

        // eger "orders" isimli queue yoksa AddAsync bunu create eder,
        // ayrica order objesini de otomatik olarak serialize eder ve o sekilde queue'ya yazar
        // queue'ya ait connection string local.settings.json icindeki AzureWebJobsStorage'da depolanir
        await orderQueue.AddAsync(order); // async
        return new OkObjectResult($"Purchase successful. OrderId: {order.OrderId}, ProductId: {order.ProductId}, Price: {order.Price}");
    }
    
    public class Order
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
    }
}