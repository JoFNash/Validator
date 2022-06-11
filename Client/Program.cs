using Grpc.Net.Client;
using Server;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:7298", new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new Greeter.GreeterClient(channel);

var input = new HelloRequest{ Name = "Alex"};
var reply = await client.SayHelloAsync(input);

Console.WriteLine(reply.Message);
Console.ReadLine();
