using ShopApi.Entity;

namespace ShopApi.DataAcsess;

public interface IDataSeedingService
{
    public void SeedDatabase();
}
public class DataSeedingService : IDataSeedingService
{
    private readonly ShopDb _context;

    public DataSeedingService(ShopDb context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void SeedDatabase()
    {
        if (!_context.Database.EnsureCreated())
            return;

        var products = new List<Product>
        {
            new Product
            {
                Name = "Lenovo legion 5",
                Price= 1000m,

            },
            new Product
            {
                Name = "Dji Mavic 3 Pro",
                Price= 1200m
            },
            new Product
            {
                Name = "Sony a7",
                Price= 800m
            },
            new Product
            {
                Name = "Iphone 13 Pro",
                Price= 750m
            },
            new Product
            {
                Name = "Samsung S22",
                Price= 620m
            },
            new Product
            {
                Name = "Nvidia RTX3090",
                Price= 1100m
            }
        };
        _context.Products.AddRange(products);
        _context.SaveChanges();
    }
}
