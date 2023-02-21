namespace ShopApi.Entity;

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; }
}
