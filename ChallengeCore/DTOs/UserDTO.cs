namespace ChallengeCore.DTOs;
public class UserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }

    public UserDTO(string name, string email, string nickname)
    {
        Name = name;
        Email = email;
        Nickname = nickname;
    }

    public UserDTO()
    {

    }
}