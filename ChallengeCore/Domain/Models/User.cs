using Ardalis.GuardClauses;

using ChallengeCore.Extensions;

namespace ChallengeCore.Domain.Models;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public IEnumerable<Purchase>? Purchases { get; set; }

    public User(int id, string name, string email, string nickname, IEnumerable<Purchase>? purchases = null)
    {
        Id = id;
        Name = Guard.Against.NullOrEmptyOrWhiteSpace(name);
        Email = Guard.Against.NullOrEmptyOrWhiteSpace(email);
        Nickname = Guard.Against.NullOrEmptyOrWhiteSpace(nickname);
        Purchases = purchases ?? [];
    }

    public User()
    {

    }
}
