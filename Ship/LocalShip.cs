using ADPC.Cargo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace ADPC.Ship
{
    public class LocalShip:IShip // save files for private, public - loggings
    {
        private static FileStream fileStream;
        //set local saving path etc...
        private Stack<CargoReport> reports;
        
        //save public (log) private (pattern, emotion)

        public LocalShip()
        {

        }
    }
}
