using Grpc.Core;
using AddressesValidator;
using Server;

namespace AddressesValidator.Services;

public class ValidateAddressesService : ValidateAddresses.ValidateAddressesBase
{
    private readonly ILogger<ValidateAddressesService> _logger;

    public ValidateAddressesService(ILogger<ValidateAddressesService> logger)
    {
        _logger = logger;
    }

    public override Task<AddressesReply> ValidateAddresses(AddressesRequest request, ServerCallContext context)
    {
        var reply = new AddressesReply();
        foreach (var address in request.Addresses)
        {
            var addressTmp = new ValidateString();
            addressTmp.Value = address;
            if (string.IsNullOrWhiteSpace(address))
            {
                addressTmp.IsValid = false;
                addressTmp.Comment = "Address must be filled";
            }
            else
            {
                addressTmp.IsValid = true;
            }
            reply.Addresses.Add(addressTmp);   
        }
        
        return Task.FromResult(reply);
    }
}