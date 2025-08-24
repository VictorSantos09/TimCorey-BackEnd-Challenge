using ChallengeCore.Application.Services.Users;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Challenge.Test;

public class UserServiceTest
{
    private IUserService _sut;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger = Substitute.For<ILogger<UserService>>();

    public UserServiceTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserService(_userRepository, _logger);
    }

}