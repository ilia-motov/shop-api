using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.DataAcsess;
using ShopApi.Dto;
using ShopApi.Entity;

namespace ShopApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ShopDb _db;

    public OrderController(ShopDb db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db)); ;
    }

    [HttpPut]
    public async Task Create(OrderDto dto)
    {
        var orderProdictIds = dto.Items
            .Select(x => x.ProductId)
            .ToList();

        var dbProductIds = await _db.Products
            .Where(p => orderProdictIds.Contains(p.Id))
            .Select(x => x.Id)
            .ToListAsync();

        var invalidProductIds = orderProdictIds.Except(dbProductIds).ToList();
        if (invalidProductIds.Any())
            throw new InvalidOperationException($"В заказе обнаружены несуществующие идентификаторы продуктов: {string.Join(", ", invalidProductIds)}");

        if (dto.Items.Count == 0)
        {
            throw new InvalidOperationException("Нет продуктов для формирования заказа");
        }
        var order = new Order
        {
            Items = dto?.Items?.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
            })
            .ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    [HttpGet]
    public async Task<List<OrderDto>> ReadAll()
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            Items = o?.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Product = new ProductDto
                {
                    Id = i.ProductId,
                    Name = i.Product?.Name,
                    Price = i.Product.Price,
                }
            }).ToList()
        })
        .ToList();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        var order = await _db.Orders.FindAsync(id);

        if (order is null)
        {
            throw new InvalidOperationException($"Заказ {id} не найден");
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
    }
}
