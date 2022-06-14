using System.Text.RegularExpressions;
using Grpc.Core;
using PhoneNumbersValidator;
using Server;

namespace PhoneNumbersValidator.Services;

public class ValidatePhoneNumbersService : ValidatePhoneNumbers.ValidatePhoneNumbersBase
{
    private readonly ILogger<ValidatePhoneNumbersService> _logger;

    public ValidatePhoneNumbersService(ILogger<ValidatePhoneNumbersService> logger)
    {
        _logger = logger;
    }

    public override Task<PhoneNumbersReply> ValidatePhoneNumbers(PhoneNumbersRequest request, ServerCallContext context)
    {
        var reply = new PhoneNumbersReply();
        var regex = new Regex("\\+?\\d+([\\(\\s\\-]?\\d+[\\)\\s\\-]?[\\d\\s\\-]+)?");
        foreach (var number in request.PhoneNumbers)
        {
            var numberTmp = new ValidateString();
            numberTmp.Value = number;
            if (!regex.IsMatch(number))
            {
                numberTmp.IsValid = false;
                numberTmp.Comment = "Wrong phone number format";
            }
            else
            {
                numberTmp.IsValid = true;
            }
            reply.PhoneNumbers.Add(numberTmp);   
        }
        
        return Task.FromResult(reply);
    }
}