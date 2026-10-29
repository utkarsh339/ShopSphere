
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Api.Data;
using ShopSphere.Api.DTOs;

namespace ShopSphere.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly ShopSphereDbContext _dbContext;

    public ProductsController(ShopSphereDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProducts(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] string sortBy = "id",
        [FromQuery] string sortOrder = "asc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // 1. Validate pagination parameters.
        if (page < 1)
        {
            return BadRequest("Page must be at least 1.");
        }

        if (pageSize < 1 || pageSize > 50)
        {
            return BadRequest("PageSize must be between 1 and 50.");
        }

        // Prevent integer overflow when calculating the offset.
        long offset = ((long)page - 1) * pageSize;

        if (offset > int.MaxValue)
        {
            return BadRequest("Page is too large.");
        }

        // 2. Start with active products.
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(product => product.IsActive);

        // 3. Search by name, description, or SKU.
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            query = query.Where(product =>
                product.Name.Contains(searchTerm) ||
                (product.Description != null &&
                 product.Description.Contains(searchTerm)) ||
                product.SKU.Contains(searchTerm));
        }

        // 4. Filter by category.
        if (categoryId.HasValue)
        {
            query = query.Where(product =>
                product.CategoryId == categoryId.Value);
        }

        // 5. Validate sorting options.
        var normalizedSortBy = sortBy.Trim().ToLowerInvariant();
        var normalizedSortOrder = sortOrder.Trim().ToLowerInvariant();

        if (normalizedSortBy is not ("id" or "name" or "price"))
        {
            return BadRequest(
                "Invalid sortBy. Allowed values: id, name, price.");
        }

        if (normalizedSortOrder is not ("asc" or "desc"))
        {
            return BadRequest(
                "Invalid sortOrder. Allowed values: asc, desc.");
        }

        // 6. Count filtered products BEFORE pagination.
        var totalCount = await query.CountAsync();

        // 7. Apply sorting with ID as a stable secondary sort.
        query = (normalizedSortBy, normalizedSortOrder) switch
        {
            ("name", "asc") => query.OrderBy(p => p.Name).ThenBy(p => p.Id),
            ("name", "desc") => query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            ("price", "asc") => query.OrderBy(p => p.Price).ThenBy(p => p.Id),
            ("price", "desc") => query.OrderByDescending(p => p.Price).ThenBy(p => p.Id),
            ("id", "desc") => query.OrderByDescending(p => p.Id),
            _ => query.OrderBy(p => p.Id)
        };

        // 8. Fetch only the requested page.
        var products = await query
            .Skip((int)offset)
            .Take(pageSize)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                CategoryName = product.Category.Name
            })
            .ToListAsync();

        // 9. Return items and pagination metadata.
        var result = new PagedResultDto<ProductDto>
        {
            Items = products,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize)
        };

        return Ok(result);
    }



    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Id == id && product.IsActive)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                CategoryName = product.Category.Name
            })
            .SingleOrDefaultAsync();

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

}