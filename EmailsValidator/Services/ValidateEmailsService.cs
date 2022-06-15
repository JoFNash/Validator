using System.Text.RegularExpressions;
using Grpc.Core;
using EmailsValidator;
using Server;

namespace EmailsValidator.Services;

public class ValidateEmailsService : ValidateEmails.ValidateEmailsBase
{
    private readonly ILogger<ValidateEmailsService> _logger;

    public ValidateEmailsService(ILogger<ValidateEmailsService> logger)
    {
        _logger = logger;
    }

    public override Task<EmailsReply> ValidateEmails(EmailsRequest request, ServerCallContext context)
    {
        var reply = new EmailsReply();
        var regex = new Regex("[a-zA-Z1-9\\-\\._]+@[a-z1-9]+(.[a-z1-9]+){1,}");
        foreach (var email in request.Emails)
        {
            var emailTmp = new ValidateString();
            emailTmp.Value = email;
            if (!regex.IsMatch(email))
            {
                emailTmp.IsValid = false;
                emailTmp.Comment = "Формат почты должен представлять из себя: <имя_почты@почта.ru>.";
            }
            else
            {
                emailTmp.IsValid = true;
            }
            reply.Emails.Add(emailTmp);   
        }
        
        return Task.FromResult(reply);
    }
}