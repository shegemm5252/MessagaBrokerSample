using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebTestServer
{
    public class CommunicationManager
    {
        private static List<ConsumerIdentifiers> _consumerIdentifiers = new List<ConsumerIdentifiers>();
        public CommunicationManager(HttpListener listener)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    string method = request.HttpMethod;

                    if (method == "GET")
                    {
                        // Get the request path
                        string path = request.Url!.AbsolutePath;
                        HttpListenerResponse response = context.Response;

                       // Console.WriteLine($"Received {method} request to " + path + " with body: ");
                        string responseString = "<html><body> Hello, World! </body></html>";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                    else
                    {
                         await RegisterSubscriber(context);
                    }
                }
            });

        }

        private async Task RegisterSubscriber(HttpListenerContext context)
        {
            Stream body = context.Request.InputStream;
            string requestBody = await new StreamReader(body).ReadToEndAsync();
            HttpListenerResponse response = context.Response;
            var register = JsonConvert.DeserializeObject<ConsumerIdentifiers>(requestBody);
            if (register != null)
            {
                if (register.Topics.Count > 0)
                {
                    var check = _consumerIdentifiers.Where(x => x.Name == register.Name).FirstOrDefault();
                    if (check != null)
                    {
                        _consumerIdentifiers.Remove(register);
                    }
                    _consumerIdentifiers.Add(register);
                    Console.WriteLine(requestBody);
                }
                
            }
            string responseString = "Registered successfully";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public async Task PublishAsync(Message message)
        {
            await Task.Run(() =>
             {
                 List<Action> actionList = new List<Action>();
                 var listUrl = _consumerIdentifiers.Where(x => x.Topics.Contains(message.Topic)).ToList();
                 foreach (var identifier in listUrl)
                 {
                     Action action = () =>
                     {
                         _ = PostAsync(message, identifier.client);
                     };

                     actionList.Add(action);
                 }
                 Parallel.Invoke(actionList.ToArray());
             });
        }

        private async Task PostAsync(Message message, string url)
        {
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                // Read the response body
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}
