using Harbor.Cargo;
using System;

namespace Harbor.Ship
{
    public enum DestinationType
    {
        MQTTBroker,
        HTTPServer,
        Wrong,
    };
    public enum HTTPMethod
    {
        POST,
        GET
    }
    /// <summary>
    /// Principals
    /// #1 ONLY XML SERIAL SUPPORT.
    /// </summary>
    public abstract class NetworkShip : Ship
    {
        /// <summary>
        /// Please, Destination must be Absoulute uri.
        /// </summary>
        public Uri Destination { get; protected set; }
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
