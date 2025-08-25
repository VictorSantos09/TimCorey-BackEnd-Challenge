using ChallengeCore.Application.Services.Users;
using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.Extensions.Logging;

namespace Challenge.Test.Application.Services;

public class UserServiceTest
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger = Substitute.For<ILogger<UserService>>();

    public UserServiceTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserService(_userRepository, _logger);
    }

    [Fact]
    public void GetAll_ReturnsUsers()
    {
        // Arrange
        var users = new List<User> { new(1, "Alice", "alice@email.com", "alice") };
        _ = _userRepository.GetAll().Returns(users);

        // Act
        var result = _sut.GetAll();

        // Assert
        Assert.NotNull(result);
        _ = Assert.Single(result.Value);
        Assert.Equal("Alice", result.Value.First().Name);
    }

    [Fact]
    public void GetById_ReturnsUser_WhenExists()
    {
        // Arrange
        var user = new User(2, "Bob", "bob@email.com", "bob");
        _ = _userRepository.GetById(2).Returns(user);

        // Act
        var result = _sut.GetById(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Bob", result.Value.Name);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _ = _userRepository.GetById(99).Returns((User?)null);

        // Act
        var result = _sut.GetById(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Add_CallsRepository()
    {
        // Arrange
        var user = new User(3, "Carol", "carol@email.com", "carol");

        // Act
        _ = _sut.Add(user);

        // Assert
        _userRepository.Received(1).Add(user);
    }

    [Fact]
    public void Update_CallsRepository()
    {
        // Arrange
        var user = new User(4, "Dave", "dave@email.com", "dave");

        // Act
        _ = _sut.Update(user);

        // Assert
        _userRepository.Received(1).Update(user);
    }

    [Fact]
    public void Delete_CallsRepository()
    {
        // Arrange
        var user = new User(5, "Eve", "eve@email.com", "eve");

        // Act
        _ = _sut.Delete(user.Id);

        // Assert
        _userRepository.Received(1).Delete(user);
    }
}
