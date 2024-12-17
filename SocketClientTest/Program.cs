// See https://aka.ms/new-console-template for more information
using SocketClientTest;

Console.WriteLine("This is to test client commununication");

Thread.Sleep(5000);

var client = new ClientManager("localhost", 5008);

if(client.IsConnected){
   
} else {
    Console.WriteLine($"Failed to connect to port {client.port}");
}
var topic = Console.ReadLine();
client.communicationManager!.Consume(topic!, (value) =>
{
    Console.WriteLine(value);
});
var msg =  Console.ReadLine();