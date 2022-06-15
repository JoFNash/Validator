using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using Grpc.Net.Client;
using Microsoft.CodeAnalysis;
using Server;
using StructsWPF;

namespace ClientGRPC
{
    public class Client
    {
        private CancellationTokenSource _token;
        private Validate.ValidateClient _client;

        public Client(string hostname)
        {
            _token = new CancellationTokenSource();
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(hostname, new GrpcChannelOptions {HttpHandler = httpHandler});
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
                        Comment = reply.Fullname.Name.HasComment ? reply.Fullname.Name.Comment : ""
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
}