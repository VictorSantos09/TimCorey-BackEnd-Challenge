using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;


namespace ChallengeCore.Infrastructure.Logging;

public static class Logger
{
    private const string _supressionJustification = "Método para simplificar a chamada, fornecendo o eventId e caso necessário mudar a framework.";

    public static class Code
    {
        public static readonly EventId General = new(0, "General");
        public static readonly EventId Info = new(1, "Info");
        public static readonly EventId Warning = new(2, "Warning");
        public static readonly EventId Error = new(3, "Error");
        public static readonly EventId Debug = new(4, "Debug");
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Information(this ILogger logger, string message, params object[] args)
    {
        logger.LogInformation(Code.Info, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Information(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogInformation(Code.Info, exception, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Warning(this ILogger logger, string message, params object[] args)
    {
        logger.LogWarning(Code.Warning, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Warning(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogWarning(Code.Warning, exception, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Error(this ILogger logger, string message, params object[] args)
    {
        logger.LogError(Code.Error, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogError(Code.Error, exception, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Debug(this ILogger logger, string message, params object[] args)
    {
        logger.LogDebug(Code.Debug, message, args);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = _supressionJustification)]
    public static void Debug(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogDebug(Code.Debug, exception, message, args);
    }
}
