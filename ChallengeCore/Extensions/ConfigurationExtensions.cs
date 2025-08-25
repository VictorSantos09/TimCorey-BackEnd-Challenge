using Ardalis.GuardClauses;

using Microsoft.Extensions.Configuration;

namespace ChallengeCore.Extensions;
public static class ConfigurationExtensions
{
    public const string DefaultConnectionStringName = "Default";
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name = DefaultConnectionStringName)
    {
        return Guard.Against.NullOrEmptyOrWhiteSpace(configuration.GetConnectionString(name));
    }
}
