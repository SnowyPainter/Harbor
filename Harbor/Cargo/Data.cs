using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Harbor.Cargo
{
    public static class CargoExtensions
    {

    }
    public class Data
    {
        /*
        [JsonIgnore]
        public static Dictionary<string,int> HeaderCodes = new Dictionary<string, int>()
        {
            { "XML", 0 }, { "JSON", 1}, {"Binary", 2}, {"String", 3}
        }; 
        */

        public Data() { }
        //public int Header { get; set; } = HeaderCodes["String"];
        /// <summary>
        /// Activity must be very simple as much as it can express what to do.
        /// </summary>
        public string Content { get; set; } = "";
    }
}
