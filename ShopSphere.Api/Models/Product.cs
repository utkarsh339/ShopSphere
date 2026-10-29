
namespace ShopSphere.Api.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string SKU { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public Category Category { get; set; } = null!;
}
