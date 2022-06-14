using Grpc.Core;
using BirthDateValidator;
using Google.Protobuf.WellKnownTypes;
using Server;

namespace BirthDateValidator.Services;

public class ValidateBirthDateService : ValidateBirthDate.ValidateBirthDateBase
{
    private readonly ILogger<ValidateBirthDateService> _logger;

    public ValidateBirthDateService(ILogger<ValidateBirthDateService> logger)
    {
        _logger = logger;
    }

    public override Task<BirthDateReply> ValidateBirthDate(BirthDateRequest request, ServerCallContext context)
    {
        var birthDate = new ValidateTimestamp
        {
            Value = request.BirthDate
        };
        if (request.BirthDate > DateTime.UtcNow.AddYears(-14).ToTimestamp())
        {
            birthDate.IsValid = false;
            birthDate.Comment = "Birthdate must be correct and user must be at least 14 years old";
        } 
        else
        {
            birthDate.IsValid = true;
        }
        
        var reply = new BirthDateReply
        {
            BirthDate = birthDate
        };

        return Task.FromResult(reply);
    }
}