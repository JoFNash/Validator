using System.Text.RegularExpressions;
using Grpc.Core;
using PassportValidator;
using Server;

namespace PassportValidator.Services;

public class ValidatePassportService : ValidatePassport.ValidatePassportBase
{
    private readonly ILogger<ValidatePassportService> _logger;

    public ValidatePassportService(ILogger<ValidatePassportService> logger)
    {
        _logger = logger;
    }

    public override Task<PassportReply> ValidatePassport(PassportRequest request, ServerCallContext context)
    {
        var regex = new Regex("\\d{4}\\s\\d{6}");
        var reply = new PassportReply
        {
            Passport = new ValidateString
            {
                Value = request.Passport
            }
        };

        if (!regex.IsMatch(request.Passport))
        {
            reply.Passport.IsValid = false;
            reply.Passport.Comment = "В поле номер паспорта введите: <серия(4 знака)_номер(6 знаков)>";
        } 
        else
        {
            reply.Passport.IsValid = true;
        }

        return Task.FromResult(reply);
    }
}