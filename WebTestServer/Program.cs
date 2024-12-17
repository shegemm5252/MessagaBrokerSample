// See https://aka.ms/new-console-template for more information
using WebTestServer;

Console.WriteLine("Hello, Server!");

var vr = new ConnectionManager("http://localhost:8000/server/");

var read = Console.ReadLine();
while (read != null)
{
    vr.communicationManager!.PublishAsync(new Message { Data = read, Topic = "Topic" }).Wait();
    read = Console.ReadLine();
    vr.communicationManager!.PublishAsync(new Message { Data = read!, Topic = "Topic2" }).Wait();
    read = Console.ReadLine();
}