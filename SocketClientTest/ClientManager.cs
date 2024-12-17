using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientTest
{
    public class ClientManager : IDisposable
    {
        private  TcpClient? _client;
        private  int _port;
        private  CommunicationManager? _commManager;
        private  string? _host;
        public ClientManager(string hostname, int port)
        {
            _port = port;
            _host = hostname;
            Connect();
        }

        private void Connect(bool reconnect = false)
        {
            try
            {
                _client = new TcpClient(_host!, _port);

                if (_client.Connected)
                {
                    NetworkStream stream = _client.GetStream();
                    if (stream.CanRead && stream.CanWrite)
                    {
                        // Connected and stream is available
                        _commManager = new CommunicationManager(stream);
                        Console.WriteLine($"Client has been connected to port {_port}");
                        if(reconnect) {
                            
                            _commManager.Reconsume();
                        }
                        // ...
                    }
                    else
                    {
                        Console.WriteLine("Stream is not available");

                    }
                }
                else
                {
                    Console.WriteLine("Failed to connect");
                }
                Reconnect();
            }
            catch (Exception ex)
            {
                
                _client!.Dispose();
                Console.WriteLine("Failed to connect");
                Thread.Sleep(500);

            }
               
        }

        public void Disconnect()
        {
            
            _client!.Close();
        }

        public void Reconnect() {
            Task.Run(() => {
                while (true)
                {
                    if (_client!.Connected)
                    {
                        continue;
                    }
                    _client!.Dispose();
                    Console.WriteLine("Failed to connect");
                    Thread.Sleep(500);
                    Connect(true);

                }
            });  
            
        }

        public void Dispose()
        {
            Disconnect();
        }

        public bool IsConnected
        {
            get
            {
                return _client!.Connected;
            }
        }



        public int port
        {
            get
            {
                return _port;
            }
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
