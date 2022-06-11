using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server;

namespace Server.Services
{
    public class ValidateService : Validate.ValidateBase
    {
        private readonly ILogger<ValidateService> _logger;

        public ValidateService(ILogger<ValidateService> logger)
        {
            _logger = logger;
        }

        public override Task<DataReply> Validate(DataRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DataReply
            {
                Fullname = new FullnameReply
                {
                    Name = new ValidateString{IsValid = true, Value = request.Fullname.Name, Comment = "OK"},
                    Surname = new ValidateString{IsValid = true, Value = request.Fullname.Surname, Comment = "OK"},
                    Patronymic = new ValidateString{IsValid = true, Value = request.Fullname.Patronymic, Comment = "OK"}
                }
            });
        }
    }    
}

