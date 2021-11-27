using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Harbor.Cargo
{
    public class Data
    {
        public Data() { }
        /// <summary>
        /// Activity must be very simple as much as it can express what to do.
        /// </summary>
        public string Content { get; set; }
    }
}
