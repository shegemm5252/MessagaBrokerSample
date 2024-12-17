using SocketServerTest;

Console.WriteLine("This is to test server commununication");

var server = new ConnectionManager(5008);
Console.WriteLine("Supply your message");
var msg = Console.ReadLine();
while (msg != null)
{
    server.communicationManager!.SendMessage(new Message{ Data = msg, Topic = "TestTopic" });
    msg = Console.ReadLine();
    server.communicationManager!.SendMessage(new Message { Data = msg!, Topic = "TestTopic2" });
    msg = Console.ReadLine();
}