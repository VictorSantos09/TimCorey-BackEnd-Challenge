using Ardalis.GuardClauses;

namespace ChallengeCore.Extensions;
public static class GuardClauseExtensions
{
    public static string NullOrEmptyOrWhiteSpace(this IGuardClause guardClause,
                                                 string? input,
                                                 string? parameterName = null,
                                                 string? message = null)
    {
        guardClause.NullOrEmpty(input, parameterName, message);
        guardClause.NullOrWhiteSpace(input, parameterName, message);
        return input;
    }
}
