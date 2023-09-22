namespace ChallengeCore.DTOs;
public class BaseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    public BaseDTO(bool success, string message, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static BaseDTO Invalid(string message, object? data = null)
    {
        return new(false, message, data);
    }

    public static BaseDTO Valid(string message, object? data = null)
    {
        return new(true, message, data);
    }
}