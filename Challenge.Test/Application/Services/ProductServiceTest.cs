using ChallengeCore.Application.Services.Products;
using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.Extensions.Logging;

namespace Challenge.Test.Application.Services;

public class ProductServiceTest
{
    private readonly ProductService _sut;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger = Substitute.For<ILogger<ProductService>>();

    public ProductServiceTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _sut = new ProductService(_productRepository, _logger);
    }

    [Fact]
    public void GetAll_ReturnsProducts()
    {
        var products = new List<Product> { new(1, "Notebook", 1000, []) };
        _ = _productRepository.GetAll().Returns(products);

        var result = _sut.GetAll();

        Assert.NotNull(result);
        _ = Assert.Single(result.Value);
        Assert.Equal("Notebook", result.Value.First().Name);
    }

    [Fact]
    public void GetById_ReturnsProduct_WhenExists()
    {
        var product = new Product(2, "Mouse", 50, []);
        _ = _productRepository.GetById(2).Returns(product);

        var result = _sut.GetById(2);

        Assert.NotNull(result);
        Assert.Equal("Mouse", result.Value.Name);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotExists()
    {
        _ = _productRepository.GetById(99).Returns((Product?)null);

        var result = _sut.GetById(99);

        Assert.Null(result);
    }

    [Fact]
    public void Add_CallsRepository()
    {
        var product = new Product(3, "Teclado", 120, []);

        _ = _sut.Add(product);

        _productRepository.Received(1).Add(product);
    }

    [Fact]
    public void Update_CallsRepository()
    {
        var product = new Product(4, "Monitor", 800, []);

        _ = _sut.Update(product);

        _productRepository.Received(1).Update(product);
    }

    [Fact]
    public void Delete_CallsRepository()
    {
        var product = new Product(5, "Headset", 200, []);

        _ = _sut.Delete(product.Id);

        _productRepository.Received(1).Delete(product);
    }
}
