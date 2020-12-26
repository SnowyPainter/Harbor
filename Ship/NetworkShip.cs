using Harbor.Cargo;
using System;

namespace Harbor.Ship
{
    public enum DestinationType
    {
        MQTTBroker,
        HTTPServer,
        HTTPSServer,
        Wrong,
    };
    public abstract class NetworkShip : Ship
    {
        /// <summary>
        /// Please, Destination must be Absoulute uri.
        /// </summary>
        public Uri Destination { get; protected set; }
        protected PacketMake packetMake = new PacketMake();
        public bool SetDestination(Uri dest)
        {
            if (dest == null) return false;
            Destination = dest;
            return true;
        }

        public DestinationType GetDestinationType()
        {
            if (Destination.Scheme == "http")
            {
                return DestinationType.HTTPServer;
            }
            else if (Destination.Scheme == "https")
            {
                return DestinationType.HTTPSServer;
            }
            else if (Destination.Scheme == "mqtt")
            {
                return DestinationType.MQTTBroker;
            }
            else
            {
                return DestinationType.Wrong;
            }
        }
    }
}
