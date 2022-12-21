﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionDemo;

public static class OnPaymentReceived
{
    [FunctionName("OnPaymentReceived")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [Queue("orders")] IAsyncCollector<Order> orderQueue, ILogger log)
    {
        // since route is set to null, by default it's /api/OnPaymentRecevied 
        
        log.LogInformation("Payment received");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var order = JsonConvert.DeserializeObject<Order>(requestBody);

        // if there's no such queue called "orders", then AddAsync() function creates it automatically for us
        // Besides that, it serializes order object automatically and sends it to the queue called "orders"
        // The connectionstr that belongs to the queue is kept in local.settings.json > AzureWebJobsStorage
        await orderQueue.AddAsync(order);
        
        return new OkObjectResult($"Purchase successful. OrderId: {order.OrderId}, ProductId: {order.ProductId}, Price: {order.Price}");
    }
}