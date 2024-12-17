using System.Net;

namespace WebTestClient
{
    public class ClientConnectionManager
    {
        private readonly ClientCommunicationManager _communicationManager;
        public ClientConnectionManager(string host)
        {
            HttpListener listener = new HttpListener();
            Console.WriteLine("Supply a port the client will be listen from");
            var port = Console.ReadLine();
            string url =$"http://localhost:{port}/client/";
            listener.Prefixes.Add(url);
            listener.Start();

            _communicationManager = new ClientCommunicationManager(listener, host, url);
        }

        public ClientCommunicationManager? clientManager
        {
            get
            {
                return _communicationManager;
            }
        }
    }
}
