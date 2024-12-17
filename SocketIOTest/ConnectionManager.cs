using System.Net.Sockets;
using System.Net;

namespace SocketServerTest
{
    public class ConnectionManager : IDisposable
    {
        private readonly TcpListener _listener;
        private CommunicationManager? _commManager;
      //  private readonly List<TcpClient> _client;
        public ConnectionManager(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
           // _client = new List<TcpClient>();
            // Begin accepting incoming connections asynchronously
            _listener.BeginAcceptTcpClient(AcceptIncomingConnectionCallback, _listener);

        }

        private void AcceptIncomingConnectionCallback(IAsyncResult result)
        {
            TcpListener listener = (TcpListener)result.AsyncState!;
            TcpClient incomingClient = listener.EndAcceptTcpClient(result);
            NetworkStream incomingStream = incomingClient.GetStream();

            //_client.Add(incomingClient);
            
            // Pass the incoming stream to the CommunicationManager
            CommunicationManager commManager = new CommunicationManager(incomingClient);
            _commManager = commManager;
            _listener.BeginAcceptTcpClient(AcceptIncomingConnectionCallback, _listener);
           // CheckConnection();
            // ...
        }

        //private void CheckConnection()
        //{
        //    var count = 0;
        //    while (true)
        //    {
        //        if (count == _client.Count){
        //            count = 0;
        //        }
        //        try
        //        {
        //            var item = _client[count];
        //            if (item == null) continue;
        //            if (!item.Connected)
        //            {
        //                _client.RemoveAt(count);

        //            }
        //            count++;
        //        }
        //        catch (Exception ex)
        //        {

        //            count = 0;
        //        }
               
               

        //    }
        //}
        public void Dispose()
        {
            //_client.Clear();
            _listener.Stop();
            _listener.Dispose();
        }

        public CommunicationManager? communicationManager
        {
            get
            {
                return _commManager;
            }
        }
    }
}
