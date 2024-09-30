using ChallengeCore.DTOs;
using ChallengeCore.Models;
using ChallengeCore.Services;
using NSubstitute;

namespace Challenge.Test;

public class UserServiceTest
{
    private IUserService _sut;
    private readonly IUserDAO _userDao;

    public UserServiceTest()
    {
        _userDao = Substitute.For<IUserDAO>();
        _sut = new UserService(_userDao);
    }

    [Fact]
    public async Task Register_DeveFalhar_NomeInvalido_MenorQueTresCaracteres()
    {
        UserDTO dto = new UserDTO("te", "teste@gmail.com", "teste");
        var result = await _sut.Register(dto);

        Assert.False(result.Success);
        Assert.Equal("nome deve conter no mínimo 3 dígitos.", result.Message);
    }

    [Fact]
    public async Task Register_DeveFalhar_EmailInvalido_MenorQueTresCaracteres()
    {
        UserDTO dto = new UserDTO("teste", "tes", "teste");
        var result = await _sut.Register(dto);

        Assert.False(result.Success);
        Assert.Equal("email deve conter '@' e no mínimo 3 dígitos.", result.Message);
    }

    [Fact]
    public async Task Register_DeveFalhar_EmailInvalido_MenorQueTresCaracteresSemArroba()
    {
        UserDTO dto = new UserDTO("teste", "tes", "teste");
        var result = await _sut.Register(dto);

        Assert.False(result.Success);
        Assert.Equal("email deve conter '@' e no mínimo 3 dígitos.", result.Message);
    }


    [Fact]
    public async Task Register_DeveFalhar_NicknameInvalido_MenorQueTresCaracteresSemArroba()
    {
        UserDTO dto = new UserDTO("teste", "tes@", "te");
        var result = await _sut.Register(dto);

        Assert.False(result.Success);
        Assert.Equal("nickname deve conter no mínimo 3 dígitos.", result.Message);
    }

    [Fact]
    public async Task Register_DeveCadastrar_QuandoDadosValidos()
    {
        UserDTO dto = new UserDTO("teste", "tes@", "teste");
        _userDao.Create(dto).Returns(BaseDTO.Valid("conta criada com sucesso"));

        var result = await _sut.Register(dto);

        Assert.True(result.Success);
        Assert.Equal("conta criada com sucesso", result.Message);
    }

    [Fact]
    public async Task Register_NaoDeveCadastrar_UsuarioExistente()
    {
        UserDTO dto = new UserDTO("teste", "tes@", "teste");
        _userDao.Create(dto).Returns(BaseDTO.Invalid("usuário já existente"));

        var result = await _sut.Register(dto);

        Assert.False(result.Success);
        Assert.Equal("usuário já existente", result.Message);
    }
}