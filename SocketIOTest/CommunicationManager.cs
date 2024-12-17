using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest
{
    public class CommunicationManager
    {
        // private List<TcpClient> _client;
        private static List<ConsumerIdentifiers> _consumerIdentifiers = new List<ConsumerIdentifiers>();
        public CommunicationManager(TcpClient client)
        {
            //_client = client.OrderBy(x => x.GetStream().DataAvailable).ToList();

            // _consumerIdentifiers = new List<ConsumerIdentifiers>();
            //// for (var i = 0; i < _client.Count; i++)
            {
                //var item = client[i];
                var netorkStream = client.GetStream();

                //if (!netorkStream.DataAvailable)
                // {
                StartReading(netorkStream, client);
                //}
            }
        }

        private void StartReading(NetworkStream stream, TcpClient client)
        {
            byte[] readBuffer = new byte[1024];
            Console.WriteLine($"{_consumerIdentifiers.Count + 1} Client Connected sucessfully");
            stream.BeginRead(readBuffer, 0, readBuffer.Length, (result) =>
            {
                if (!client.Connected)
                {
                    var itmList = _consumerIdentifiers.Where(p => p.client.Connected == false).ToList();
                    itmList.ForEach(p =>
                    {
                        _consumerIdentifiers.Remove(p);
                    });

                    return;
                }
                int bytesRead = stream.EndRead(result);
                byte[] buffer = (byte[])result.AsyncState!;

                // Process the incoming data
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + message);
                if (!string.IsNullOrEmpty(message))
                {
                    ConsumerIdentifier data = JsonConvert.DeserializeObject<ConsumerIdentifier>(message)!;
                    var check = _consumerIdentifiers.FirstOrDefault(x => x.Name == data.Name);
                    if (check != null)
                    {
                        check.Topics.Add(data.Topic);
                    }
                    else
                    {
                        _consumerIdentifiers.Add(new ConsumerIdentifiers { client = client, Name = data.Name, Topics = new List<string> { data.Topic } });
                    }

                }


                // Continue reading
                StartReading(stream, client);
            }, readBuffer);
        }

        public void SendMessage(Message message)
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            var count = 0;
            var consumerIdentifiers = _consumerIdentifiers.Where(x => x.Topics.Contains(message.Topic)).ToList();
            foreach (var consumer in consumerIdentifiers)
            {
                var networkStream = consumer.client.GetStream();
                networkStream.Write(data, 0, data.Length);
                Console.WriteLine($" Message sent to {count} client sucessfully");
                count++;
            }



        }

        //public Message ReceiveMessage()
        //{
        //    byte[] data = new byte[1024];
        //    int bytesRead = _stream.Read(data, 0, data.Length);
        //    string json = Encoding.UTF8.GetString(data, 0, bytesRead);
        //    return JsonConvert.DeserializeObject<Message>(json);
        //}
    }
}
