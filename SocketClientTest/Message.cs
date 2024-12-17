using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientTest
{
    public class Message
    {
        public string Topic { get; set; }
        public string Data { get; set; }
    }

    public class ConsumerIdentifier
    {

        public string Topic { get; set; }
        public string Name { get; set; }
    }
}
