namespace ChallengeCore.DTOs;
public class UserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public UserDTO(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public UserDTO()
    {
        
    }
}