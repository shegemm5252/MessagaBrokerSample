using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientTest
{
    public class CommunicationManager
    {
        private NetworkStream _stream;
        private static  IDictionary<string, Action<string>> keyValuePairs = new Dictionary<string, Action<string>>();
      // private static List<string> _listTopic = new List<string>();
        private string _nameId;
        public CommunicationManager(NetworkStream stream)
        {
            _stream = stream;
            _nameId = Guid.NewGuid().ToString();   
            StartReading();
        }
        private void StartReading()
        {
            try
            {
                byte[] readBuffer = new byte[1024];
                _stream.BeginRead(readBuffer, 0, readBuffer.Length, ReadCallback, readBuffer);
            }
            catch (Exception ex)
            {

                var o = ex.Message;
            }
          
        }

        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                int bytesRead = _stream.EndRead(result);
                byte[] buffer = (byte[])result.AsyncState!;

                // Process the incoming data
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (!string.IsNullOrEmpty(message))
                {
                    Message data = JsonConvert.DeserializeObject<Message>(message)!;
                    var pair = keyValuePairs.FirstOrDefault(x => x.Key == data.Topic);
                    if (pair.Value != null)
                    {
                        pair.Value.Invoke(data.Data);
                    }
                    Console.WriteLine("Received: " + message);
                }
            }
            catch (Exception ex)
            {

                
            }
           
           

            // Continue reading
            StartReading();
        }
        
        public void Consume(string topic, Action<string> action) {

            var identifier = new ConsumerIdentifier{ Name = _nameId, Topic= topic };
            string json = JsonConvert.SerializeObject(identifier);
            byte[] data = Encoding.UTF8.GetBytes(json);
            _stream.Write(data, 0, data.Length);

            keyValuePairs.Add(topic, action);
        }

        public void Reconsume(){
            foreach (var item in keyValuePairs)
            {
                Thread.Sleep(500);
                var identifier = new ConsumerIdentifier { Name = _nameId, Topic = item.Key };
                string json = JsonConvert.SerializeObject(identifier);
                byte[] data = Encoding.UTF8.GetBytes(json);
                _stream.Write(data, 0, data.Length);
            }
        }
    }
}
