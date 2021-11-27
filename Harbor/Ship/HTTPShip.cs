using Harbor.Cargo;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
//Support MQTT client, HTTP client
namespace Harbor.Ship
{ 

    public class HTTPShip:NetworkShip 
    {
        /// <summary>
        /// Miliseconds
        /// </summary>
        public int TimeInterval { get; set; } = 10;
        public HTTPShip(Uri dest):base(dest) {
            //Application/XML
        }
        
        /// <summary>
        /// Send to destination uri Each report. (POST Method)
        /// </summary>
        public override void PullAwayPrivateCargos()
        {
            foreach (var c in publicCargos)
            {
                if (c.PrimaryTime == null)
                    c.SetPrimaryTimeNow();
                var packet = new StringContent(c.ToXMLPacket(), Encoding.UTF8, "application/xml");

                HttpClient.PostAsync(Destination.ToString(), packet);

                Thread.Sleep(TimeInterval);
            }
        }

    }
}
