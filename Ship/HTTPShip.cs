using Harbor.Cargo;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
//Support MQTT client, HTTP client
namespace Harbor.Ship
{ 
    

    public class HTTPShip:NetworkShip 
    {
        private readonly HttpClient httpClient = new HttpClient();
        public HTTPShip(Uri dest)
        {
            if (dest == null) throw new Exception();

            Destination = dest;
        }
        
        public void PullAwayReports()
        {
            foreach(var r in reports)
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
    }


    public class MQTTShip : NetworkShip
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
    }
}
