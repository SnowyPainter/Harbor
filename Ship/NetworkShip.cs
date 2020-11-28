using System;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Harbor.Ship
{
    public class NetworkShip:IShip //Support MQTT client, HTTP client
    {
        public Uri Destination { get; private set; }

        private readonly HttpClient httpClient = new HttpClient();

        public NetworkShip()
        {

        }
        
        public void SetDestination(Uri dest)
        {
            Destination = dest;
        }
    }
}
