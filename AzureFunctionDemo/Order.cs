namespace AzureFunctionDemo;

public class Order
{
    //Composite keys
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public decimal Price { get; set; }
    public string Email { get; set; }

}