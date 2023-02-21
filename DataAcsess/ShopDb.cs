using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopApi.Entity;
using System.Globalization;

namespace ShopApi.DataAcsess;

public class ShopDb : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new SqliteConnection("Filename=ShopDB.db");

        connection.CreateCollation("NOCASE", (x, y) => string.Compare(x, y, true, new CultureInfo("ru-RU")));
        connection.CreateFunction("upper", (string value) => value.ToUpper(new CultureInfo("ru-RU")));
        connection.CreateFunction("lower", (string value) => value.ToLower(new CultureInfo("ru-RU")));

        optionsBuilder.UseSqlite(connection)
        .UseSnakeCaseNamingConvention();
    }
}
