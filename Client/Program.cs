using System;
using System.Collections.Generic;
using System.Net.Http;
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

var emails = new List<string>();
emails.Add("hello! email1");
emails.Add("Not hello! email2");

var input = new DataRequest{ Fullname = new Fullname{ Name = name, Surname = surname, Patronymic = patronymic} };
foreach (var email in emails)
{
    input.Emails.Add(email);
}

var reply = await client.ValidateAsync(input);
PrintReply(reply.Fullname.Name, "Name");
PrintReply(reply.Fullname.Surname, "Surname");
PrintReply(reply.Fullname.Patronymic, "Patronymic");

channel.Dispose();
