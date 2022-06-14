using System;
using System.Net.Http;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Server;

void PrintReply(ValidateString reply, string name)
{
    Console.WriteLine();
    Console.WriteLine($"{name}:");
    Console.Write("Value = ");
    Console.WriteLine(reply.Value);
    Console.Write("Is valid: ");
    Console.WriteLine(reply.IsValid);
    if (reply.HasComment)
    {
        Console.Write("Comment: ");
        Console.WriteLine(reply.Comment);
    }
}

void PrintReplyDate(ValidateTimestamp reply)
{
    Console.WriteLine();
    Console.WriteLine("Date:");
    Console.Write("Value = ");
    Console.WriteLine(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(reply.Value.Seconds));
    Console.Write("Is valid: ");
    Console.WriteLine(reply.IsValid);
    if (reply.HasComment)
    {
        Console.Write("Comment: ");
        Console.WriteLine(reply.Comment);
    }
}

var token = new CancellationTokenSource();
var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:7298", new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new Validate.ValidateClient(channel);

Console.Write("Name = ");
var name = Console.ReadLine();
Console.Write("Surname = ");
var surname = Console.ReadLine();
Console.Write("Patronymic = ");
var patronymic = Console.ReadLine();
Console.Write("Passport = ");
var passport = Console.ReadLine();
Console.Write("Date = ");
var date = Console.ReadLine();
var correctDate = DateTime.Parse(date);

var input = new DataRequest
{
    Fullname = new Fullname
    {
        Name = name, 
        Surname = surname, 
        Patronymic = patronymic
    }, 
    Passport = passport,
    BirthDate = DateTime.SpecifyKind(correctDate, DateTimeKind.Utc).ToTimestamp()
};

var reply = await client.ValidateAsync(input, cancellationToken: token.Token);
PrintReply(reply.Fullname.Name, "Name");
PrintReply(reply.Fullname.Surname, "Surname");
PrintReply(reply.Fullname.Patronymic, "Patronymic");
PrintReply(reply.Passport, "Passport");
PrintReplyDate(reply.BirthDate);

channel.Dispose();
