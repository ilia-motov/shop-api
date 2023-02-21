using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.DataAcsess;
using ShopApi.Dto;
using ShopApi.Entity;

namespace ShopApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ShopDb _db;

    public ProductController(ShopDb db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db)); ;
    }

    [HttpPut]
    public async Task<int> Create(ProductDto productDto)
    {
        if (productDto.Name == null)
        {
            throw new InvalidOperationException($"Не заполнено имя продукта");
        }

        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return product.Id;
    }

    [HttpGet]
    public Task<List<ProductDto>> ReadAll()
    {
        return _db.Products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
        })
            .ToListAsync();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            throw new InvalidOperationException($"Продукт {id} не найден");
        }

        var orderItems = await _db.OrderItems.FirstOrDefaultAsync(p => p.ProductId == id);

        if (orderItems != null)
        {
            throw new InvalidOperationException($"Невозможно удалить продукт: '{product.Name}'!  Он используется в заказе!");
        }

         _db.Products.Remove(product);
        await _db.SaveChangesAsync();
    }
}