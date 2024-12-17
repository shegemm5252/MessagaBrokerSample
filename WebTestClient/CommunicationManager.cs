using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WebTestClient
{
    public class ClientCommunicationManager
    {
        private readonly HttpListener _listener;
        private readonly HttpClient _httpClient;
        private readonly string _name;
        private static IDictionary<string, Action<string>> keyValuePairs = new Dictionary<string, Action<string>>();
        private readonly List<string> _topic;
        private readonly string _url;
        private readonly string _host;
        private bool _connected;
        public ClientCommunicationManager(HttpListener listener, string host, string url)
        {
            _listener = listener;
            _httpClient = new HttpClient();
            _name = Guid.NewGuid().ToString();
            _topic = new List<string>();
            _url = url;
            _host = host;
            Task.Run(() => GetData());
            Task.Run(() => CheckConnection());
            
        }

        private async Task Registered(string topic)
        {
            var checkTopic = _topic.Contains(topic);
            if (!checkTopic)
            {
                _topic.Add(topic);
                await ConnectAsync();
            }


        }

        private async Task ConnectAsync()
        {
            var requestBody = new ConsumerIdentifiers
            {
                Topics = _topic,
                client = _url,
                Name = _name
            };
            var data = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Send a POST request to the server
            var response = await _httpClient.PostAsync(_host, content);
            if (response.IsSuccessStatusCode)
            {
                // Read the response body
                var responseBody = await response.Content.ReadAsStringAsync();
               // Console.WriteLine(responseBody);
                _connected = true;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                _connected = false;
            }
        }

        private async Task CheckConnection()
        {
            while (true)
            {

                try
                {
                    var response = await _httpClient.GetAsync(_host);
                    if (response.IsSuccessStatusCode && !_connected)
                    {
                        await ConnectAsync();
                    }
                    else if (!response.IsSuccessStatusCode)
                    {
                        _connected = false;
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (HttpRequestException ex)
                {

                    var exm = ex.Message;
                    _connected = false;
                    Console.WriteLine("Error: " + ex.Message);
                }
                
                Thread.Sleep(2000);
            }
        }

        public async Task ConsumeAsync(string topic, Action<string> action)
        {
            await Registered(topic);
            keyValuePairs.Add(topic, action);

        }

        private async Task GetData()
        {
            while (true)
            {
                var context = _listener.GetContext();
                HttpListenerRequest request = context.Request;
                string method = request.HttpMethod;

                if (method == "GET")
                {
                    // Get the request path
                    string path = request.Url!.AbsolutePath;
                    HttpListenerResponse response = context.Response;

                   //Console.WriteLine($"Received {method} request to " + path + " with body: ");
                    string responseString = "<html><body> Hello, World! </body></html>";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                else
                {
                    await Subscriber(context);
                }
            }


        }

        private async Task Subscriber(HttpListenerContext context)
        {
            Stream body = context.Request.InputStream;
            string requestBody = await new StreamReader(body).ReadToEndAsync();
            HttpListenerResponse response = context.Response;
            var data = JsonConvert.DeserializeObject<Message>(requestBody);
            string topic = data.Topic;
            var pair = keyValuePairs.Where(n => n.Key == topic).FirstOrDefault();

            if (pair.Value != null)
            {
                pair.Value.Invoke(data.Data);
            }
            string responseString = "Recieved successfully";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}


