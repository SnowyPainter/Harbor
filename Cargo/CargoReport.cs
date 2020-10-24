using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
    [Serializable]
    public class CargoReport //Analyzed cargo by cargoType
    {
        public ML.Property.PosNegPercent PNP { get; set; } //for voice, log, text
        public ML.Property.Emotion Emotion
        {
            get; set; //for voice, log, text
        }
        public DateTime ReportedTime { get; set; }
        public CargoReport()
        {
            ReportedTime = DateTime.Now;
        }
    }
}
