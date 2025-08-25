using Ardalis.GuardClauses;

namespace ChallengeCore.Extensions;
public static class GuardClauseExtensions
{
    public static string NullOrEmptyOrWhiteSpace(this IGuardClause guardClause,
                                                 string? input,
                                                 string? parameterName = null,
                                                 string? message = null)
    {
        _ = guardClause.NullOrEmpty(input, parameterName, message);
        _ = guardClause.NullOrWhiteSpace(input, parameterName, message);
        return input;
    }
}
