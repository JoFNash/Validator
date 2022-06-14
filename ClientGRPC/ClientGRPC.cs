using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Grpc.Net.Client;
using Microsoft.CodeAnalysis;
using Server;
using StructsWPF;

// using DataRequestWPF = Server.DataRequest;
// using DataReplyWPF = Server.DataReply;
// using FullnameWPF = Server.Fullname;
// using FullnameReplyWPF = Server.FullnameReply;
// using ValidateStringWPF = Server.ValidateString;
// using ValidateTimestampWPF = Server.ValidateTimestamp;


namespace ClientGRPC;


// public struct Optional<T>
// {
//     public bool HasValue { get; private set; }
//     private T value;
//     public T Value
//     {
//         get
//         {
//             if (HasValue)
//                 return value;
//             else
//                 throw new InvalidOperationException();
//         }
//     }
//
//     public Optional(T value)
//     {
//         this.value = value;
//         HasValue = true;
//     }
//
//     public static explicit operator T(Optional<T> optional)
//     {
//         return optional.Value;
//     }
//     public static implicit operator Optional<T>(T value)
//     {
//         return new Optional<T>(value);
//     }
//
//     public override bool Equals(object obj)
//     {
//         if (obj is Optional<T>)
//             return this.Equals((Optional<T>)obj);
//         else
//             return false;
//     }
//     public bool Equals(Optional<T> other)
//     {
//         if (HasValue && other.HasValue)
//             return object.Equals(value, other.value);
//         else
//             return HasValue == other.HasValue;
//     }
// }

public class Client
{
    private CancellationTokenSource _token;
    private Validate.ValidateClient _client;

    public Client(string hostname)
    {
        _token = new CancellationTokenSource();
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        var channel = GrpcChannel.ForAddress(hostname, new GrpcChannelOptions { HttpHandler = httpHandler });
        _client = new Validate.ValidateClient(channel);
    }

    public DataReplyWPF Validate(DataRequestWPF request)
    {
        var input = new DataRequest
        {
            Fullname = new Fullname
            {
                Name = request.Fullname.Name, 
                Surname = request.Fullname.Surname, 
                Patronymic = request.Fullname.Patronymic
            }, 
            Passport = request.Passport,
            BirthDate = request.BirthDate,
        };
        foreach (var number in request.PhoneNumbers)
        {
            input.PhoneNumbers.Add(number);
        }
        
        foreach (var email in request.Emails)
        {
            input.Emails.Add(email);
        }
        
        foreach (var address in request.Addresses)
        {
            input.Addresses.Add(address);
        }

        var reply = _client.Validate(input, cancellationToken: _token.Token);
        
        var replyWPF = new DataReplyWPF
        {
            Fullname = new FullnameReplyWPF
            {
                Name = new ValidateStringWPF
                {
                    Value = reply.Fullname.Name.Value,
                    IsValid = reply.Fullname.Name.IsValid,
                    Comment = reply.Fullname.Name .HasComment ? reply.Fullname.Name.Comment : ""
                },
                Surname = new ValidateStringWPF
                {
                    Value = reply.Fullname.Surname.Value,
                    IsValid = reply.Fullname.Surname.IsValid,
                    Comment = reply.Fullname.Surname.HasComment ? reply.Fullname.Surname.Comment : ""
                },
                Patronymic = new ValidateStringWPF
                {
                    Value = reply.Fullname.Patronymic.Value,
                    IsValid = reply.Fullname.Patronymic.IsValid,
                    Comment = reply.Fullname.Patronymic.HasComment ? reply.Fullname.Patronymic.Comment : ""
                }
            },
            Passport = new ValidateStringWPF
            {
                Value = reply.Passport.Value,
                IsValid = reply.Passport.IsValid,
                Comment = reply.Passport.HasComment ? reply.Passport.Comment : ""
            },
            BirthDate = new ValidateTimestampWPF
            {
                Value = reply.BirthDate.Value,
                IsValid = reply.BirthDate.IsValid,
                Comment = reply.BirthDate.HasComment ? reply.BirthDate.Comment : ""
            }
        };

        replyWPF.Emails = new List<ValidateStringWPF>();
        replyWPF.PhoneNumbers = new List<ValidateStringWPF>();
        replyWPF.Addresses = new List<ValidateStringWPF>();
        
        foreach (var number in reply.PhoneNumbers)
        {
            replyWPF.PhoneNumbers.Add(new ValidateStringWPF
            {
                Value = number.Value,
                IsValid = number.IsValid,
                Comment = number.HasComment ? number.Comment : ""
            });
        }
        
        foreach (var email in reply.Emails)
        {
            replyWPF.Emails.Add(new ValidateStringWPF
            {
                Value = email.Value,
                IsValid = email.IsValid,
                Comment = email.HasComment ? email.Comment : ""
            });
        }
        
        foreach (var address in reply.Addresses)
        {
            replyWPF.Addresses.Add(new ValidateStringWPF
            {
                Value = address.Value,
                IsValid = address.IsValid,
                Comment = address.HasComment ? address.Comment : ""
            });
        }

        return replyWPF;
    }
}