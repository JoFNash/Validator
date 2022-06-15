using System.Text.RegularExpressions;
using Grpc.Core;
using FullnameValidator;
using Server;

namespace FullnameValidator.Services;

public class ValidateFullnameService : ValidateFullname.ValidateFullnameBase
{
    private readonly ILogger<ValidateFullnameService> _logger;

    public ValidateFullnameService(ILogger<ValidateFullnameService> logger)
    {
        _logger = logger;
    }

    public override Task<FullnameReply> ValidateFullname(FullnameRequest request, ServerCallContext context)
    {
        var regex = new Regex("[A-Z][a-z]+[\\-\\s]?");
        var name = new ValidateString { Value = request.Name };
        if (!regex.IsMatch(request.Name))
        {
            name.IsValid = false;
            name.Comment = "Имя должно содержать латинские буквы и начинаться с большой буквы.";
        }
        else
        {
            name.IsValid = true;
        }
        
        var surname = new ValidateString { Value = request.Surname };
        if (!regex.IsMatch(request.Surname))
        {
            surname.IsValid = false;
            surname.Comment = "Фамилия должна содержать латинские буквы и начинаться с большой буквы.";
        }
        else
        {
            surname.IsValid = true;
        }
        
        var patronymic = new ValidateString { Value = request.Patronymic };
        if (!regex.IsMatch(request.Patronymic))
        {
            patronymic.IsValid = false;
            patronymic.Comment = "Отчество должно содержать латинские буквы и начинаться с большой буквы.";
        }
        else
        {
            patronymic.IsValid = true;
        }
        
        
        var reply = new FullnameReply
        {
            Name = name, Surname = surname, Patronymic = patronymic
        };

        return Task.FromResult(reply);
    }
}