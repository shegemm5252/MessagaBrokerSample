

namespace WebTestServer
{
    public class Message
    {
        public string Topic { get; set; }
        public string Data { get; set; }
    }

    public class ConsumerIdentifiers
    {

        public List<string> Topics { get; set; } = new List<string>();
        public string Name { get; set; }
        public string client { get; set; }
    }
    public class ConsumerIdentifier
    {

        public string Topic { get; set; }
        public string Name { get; set; }
    }
}
