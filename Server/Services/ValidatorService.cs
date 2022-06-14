using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Server;

namespace Server.Services;
public class ValidateService : Validate.ValidateBase
{
    private readonly ILogger<ValidateService> _logger;
    private readonly CancellationTokenSource _token;
    private readonly ValidateFullname.ValidateFullnameClient _fullnameClient;
    private readonly ValidatePhoneNumbers.ValidatePhoneNumbersClient _phoneNumbersClient;
    private readonly ValidateEmails.ValidateEmailsClient _emailsClient;
    private readonly ValidateAddresses.ValidateAddressesClient _addressesClient;
    private readonly ValidatePassport.ValidatePassportClient _passportClient;
    private readonly ValidateBirthDate.ValidateBirthDateClient _birthDateClient;

    public ValidateService(ILogger<ValidateService> logger)
    {
        _logger = logger;
        var builder = new ConfigurationBuilder().AddJsonFile("Configs/hosts.json", optional: false, reloadOnChange: true);
        var config = builder.Build();
        _token = new CancellationTokenSource();
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        var fullnameChannel = GrpcChannel.ForAddress(config.GetSection("FullnameValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
        var phoneNumbersChannel = GrpcChannel.ForAddress(config.GetSection("PhoneNumbersValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
        var emailsChannel = GrpcChannel.ForAddress(config.GetSection("EmailsValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
        var addressesChannel = GrpcChannel.ForAddress(config.GetSection("AddressesValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
        var passportChannel = GrpcChannel.ForAddress(config.GetSection("PassportValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
        var birthDateChannel = GrpcChannel.ForAddress(config.GetSection("BirthDateValidator").GetSection("HostName").Value, new GrpcChannelOptions { HttpHandler = httpHandler });
       
        _fullnameClient = new ValidateFullname.ValidateFullnameClient(fullnameChannel);
        _phoneNumbersClient = new ValidatePhoneNumbers.ValidatePhoneNumbersClient(phoneNumbersChannel);
        _emailsClient = new ValidateEmails.ValidateEmailsClient(emailsChannel);
        _addressesClient = new ValidateAddresses.ValidateAddressesClient(addressesChannel);
        _passportClient = new ValidatePassport.ValidatePassportClient(passportChannel);
        _birthDateClient = new ValidateBirthDate.ValidateBirthDateClient(birthDateChannel);
    }

    public override async Task<DataReply> Validate(DataRequest request, ServerCallContext context)
    {
        var fullnameReply = _fullnameClient.ValidateFullnameAsync(new FullnameRequest
        {
            Name = request.Fullname.Name, 
            Surname = request.Fullname.Surname, 
            Patronymic = request.Fullname.Patronymic
        });
        
        var passportReply = _passportClient.ValidatePassportAsync(new PassportRequest
        {
            Passport = request.Passport
        });
        
        var birthDateReply = _birthDateClient.ValidateBirthDateAsync(new BirthDateRequest
        {
            BirthDate = request.BirthDate
        });

        var reply = new DataReply
        {
            Fullname = await fullnameReply,
            Passport = (await passportReply).Passport,
            BirthDate = (await birthDateReply).BirthDate
        };

        return await Task.FromResult(reply);
    }
}    

