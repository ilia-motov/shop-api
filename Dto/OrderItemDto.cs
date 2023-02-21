namespace ShopApi.Dto;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
}
