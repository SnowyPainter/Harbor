using Harbor.Cargo;
using System;

namespace Harbor.Ship
{
    /// <summary>
    /// Principals
    /// #1 ONLY XML SERIAL SUPPORT.
    /// </summary>
    public abstract class NetworkShip : Ship
    {
        protected static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Please, Destination must be Absoulute uri.
        /// </summary>
        public Uri Destination { get; set; }

        protected NetworkShip(Uri destination)
        {
            if (destination == null) throw new Exception("Destination uri is null");
            Destination = destination;
        }
    }
}
