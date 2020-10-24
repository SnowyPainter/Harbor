using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
    public class CargoReport //Analyzed cargo by cargoType
    {
        public ML.Property.PosNegPercent PNP { get; set; } //for voice, log, text
        public ML.Property.Emotion Emotion
        {
            get; set; //for voice, log, text
        }
    }
}
