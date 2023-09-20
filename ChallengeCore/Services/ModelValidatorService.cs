using System.ComponentModel.DataAnnotations;

namespace ChallengeCore.Services;
internal class ModelValidatorService
{
    public static bool Validate(object obj, out List<ValidationResult> result)
    {
        ValidationContext context = new(obj);
        result = new();
        return Validator.TryValidateObject(obj, context, result);
    }
}