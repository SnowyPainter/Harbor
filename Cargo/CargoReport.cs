using System;
using ProtoBuf;

namespace ADPC.Cargo
{
    [Serializable]
    [ProtoContract]
    public class CargoReport //Analyzed cargo by cargoType
    {
        [ProtoMember(1)]
        public ML.Property.PosNegPercent PNP { get; set; }
        [ProtoMember(2)]
        public ML.Property.Emotion Emotion { get; set; }
        [ProtoMember(3)]
        public DateTime ReportedTime { get; set; }
        public CargoReport()
        {
            ReportedTime = DateTime.Now;
        }
    }
}
