using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebTestServer
{
    public class ConnectionManager
    {
        private readonly CommunicationManager _communicationManager;
        public ConnectionManager(string host)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(host);
            listener.Start();

            _communicationManager = new CommunicationManager(listener);
            
        }

        public CommunicationManager? communicationManager
        {
            get
            {
                return _communicationManager;
            }
        }
    }
}
