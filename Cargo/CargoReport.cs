using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ADPC.Cargo
{

    #region Static Extension
    public static class CargoReportExtension
    {
        public static CargoReport Pop(this List<CargoReport> reports, int index)
        {
            var r = reports[index];
            reports.RemoveAt(index);
            return r;
        }
    }
    #endregion

    #region Report ProtoContract & Serializable
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
    #endregion
}
