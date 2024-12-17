// See https://aka.ms/new-console-template for more information
using WebTestClient;

Console.WriteLine("Hello, Client!");

var vr = new ClientConnectionManager("http://localhost:8000/server/");

Console.WriteLine("Supply a topic");
var topic = Console.ReadLine();
vr.clientManager!.ConsumeAsync(topic, (value) => {
    Console.WriteLine(value);   
}).Wait();

var read = Console.ReadLine();
while (read != null){
     read = Console.ReadLine();
}