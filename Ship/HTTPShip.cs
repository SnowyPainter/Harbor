using Harbor.Cargo;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
//Support MQTT client, HTTP client
namespace Harbor.Ship
{ 

    public class HTTPShip:NetworkShip 
    {
        private WebRequest request;
        /// <summary>
        /// Miliseconds
        /// </summary>
        public int TimeInterval { get; set; } = 10;
        public HTTPShip(Uri dest)
        {
            if (dest == null) throw new Exception("Dest uri is null");

            Destination = dest;
        }
        private WebRequest getWebRequestFromDest()
        {
            var request = WebRequest.Create(Destination);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            request.ContentType = "application/xml";

            return request;
        }
        /// <summary>
        /// Send to destination uri Each report. (POST Method)
        /// </summary>
        public override void PullAwayReports()
        {
            foreach (var r in reports)
            {
                var packet = r.ToPacket();
                request = getWebRequestFromDest();
                request.ContentLength = packet.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(packet, 0, packet.Length);
                }

                Thread.Sleep(TimeInterval);
            }
        }
        /// <summary>
        /// Send to destination uri Each report. (POST Method)
        /// </summary>
        public override void PullAwayCargos()
        {
            foreach (var c in cargos)
            {
                if (c.PrimaryTime == null)
                    c.SetPrimaryTimeNow();
                var packet = c.ToPacket();
                request = getWebRequestFromDest();
                request.ContentLength = packet.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(packet, 0, packet.Length);
                }

                Thread.Sleep(TimeInterval);
            }
        }

    }


    /*public class MQTTShip : NetworkShip
    {
        public MQTTShip(Uri dest)
        {
            if (dest == null) throw new Exception();

            Destination = dest;
        }

        public void PullAwayReports()
        {
            foreach (var r in reports)
            {

            }
        }
        public void PullAwayCargos()
        {
            foreach (var c in cargos)
            {

            }
        }

        //Only reports & cargos
        public override async Task PullAwayAsync()
        {
            var cargoTask = Task.Run(new Action(() => PullAwayCargos()));

            PullAwayReports();

            await cargoTask;

        }
    }*/
}
