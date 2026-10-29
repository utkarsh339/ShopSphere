using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Api.Controllers;
using ShopSphere.Api.Data;
using ShopSphere.Api.DTOs;
using ShopSphere.Api.Models;
using Xunit;

namespace ShopSphere.Api.Tests;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetProducts_ReturnsActiveProducts()
    {
        // Arrange: create a separate temporary database.
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);

        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        dbContext.Categories.Add(category);

        dbContext.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Wireless Headphones",
                Description = "Wireless headphones",
                SKU = "ELEC-001",
                Price = 2499.00m,
                IsActive = true,
                CategoryId = 1,
                Category = category
            },
            new Product
            {
                Id = 2,
                Name = "Inactive Product",
                Description = "This should not appear",
                SKU = "ELEC-002",
                Price = 999.00m,
                IsActive = false,
                CategoryId = 1,
                Category = category
            }
        );

        await dbContext.SaveChangesAsync();

        var controller = new ProductsController(dbContext);

        // Act: request the first page of products.
        var actionResult = await controller.GetProducts(
            search: null,
            categoryId: null,
            sortBy: "id",
            sortOrder: "asc",
            page: 1,
            pageSize: 10);

        // Assert: verify the response and its contents.
        var okResult = Assert.IsType<OkObjectResult>(
            actionResult.Result);

        var result = Assert.IsType<PagedResultDto<ProductDto>>(
            okResult.Value);

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal("Wireless Headphones", result.Items[0].Name);
        Assert.Equal("Electronics", result.Items[0].CategoryName);
    }


    [Fact]
    public async Task GetProducts_WithSearch_ReturnsMatchingProducts()
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);

        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        dbContext.Categories.Add(category);

        dbContext.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Wireless Headphones",
                Description = "Bluetooth headphones",
                SKU = "ELEC-001",
                Price = 2499m,
                IsActive = true,
                CategoryId = 1,
                Category = category
            },
            new Product
            {
                Id = 2,
                Name = "Smart Watch",
                Description = "Fitness tracking watch",
                SKU = "ELEC-002",
                Price = 3999m,
                IsActive = true,
                CategoryId = 1,
                Category = category
            }
        );

        await dbContext.SaveChangesAsync();

        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProducts(
            search: "headphones",
            categoryId: null,
            sortBy: "id",
            sortOrder: "asc",
            page: 1,
            pageSize: 10);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var result = Assert.IsType<PagedResultDto<ProductDto>>(okResult.Value);

        Assert.Single(result.Items);
        Assert.Equal("Wireless Headphones", result.Items[0].Name);
        Assert.Equal(1, result.TotalCount);
    }


    [Fact]
    public async Task GetProducts_WithPageSizeOne_ReturnsOneProduct()
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);

        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        dbContext.Categories.Add(category);

        dbContext.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Wireless Headphones",
                SKU = "ELEC-001",
                Price = 2499m,
                IsActive = true,
                CategoryId = 1,
                Category = category
            },
            new Product
            {
                Id = 2,
                Name = "Smart Watch",
                SKU = "ELEC-002",
                Price = 3999m,
                IsActive = true,
                CategoryId = 1,
                Category = category
            }
        );

        await dbContext.SaveChangesAsync();

        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProducts(
            search: null,
            categoryId: null,
            sortBy: "id",
            sortOrder: "asc",
            page: 1,
            pageSize: 1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var result = Assert.IsType<PagedResultDto<ProductDto>>(okResult.Value);

        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(1, result.Page);
        Assert.Equal(1, result.PageSize);
    }


    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 51)]
    public async Task GetProducts_WithInvalidPagination_ReturnsBadRequest(
        int page,
        int pageSize)
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);
        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProducts(
            search: null,
            categoryId: null,
            sortBy: "id",
            sortOrder: "asc",
            page: page,
            pageSize: pageSize);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }


    [Theory]
    [InlineData("discount", "asc")]
    [InlineData("price", "sideways")]
    public async Task GetProducts_WithInvalidSorting_ReturnsBadRequest(
        string sortBy,
        string sortOrder)
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);
        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProducts(
            search: null,
            categoryId: null,
            sortBy: sortBy,
            sortOrder: sortOrder,
            page: 1,
            pageSize: 10);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }


    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsProduct()
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);

        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        var product = new Product
        {
            Id = 1,
            Name = "Wireless Headphones",
            SKU = "ELEC-001",
            Price = 2499m,
            IsActive = true,
            CategoryId = 1,
            Category = category
        };

        dbContext.Categories.Add(category);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProductById(1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var result = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(1, result.Id);
        Assert.Equal("Wireless Headphones", result.Name);
        Assert.Equal("Electronics", result.CategoryName);
    }


    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<ShopSphereDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ShopSphereDbContext(options);
        var controller = new ProductsController(dbContext);

        var actionResult = await controller.GetProductById(999);

        Assert.IsType<NotFoundResult>(actionResult.Result);
    }


}