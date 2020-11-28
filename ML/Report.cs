using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Harbor.Cargo
{

    #region Static Extension
    public static class CargoReportExtension
    {
        public static Report Pop(this List<Report> reports, int index)
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
    public class Report //Analyzed cargo by cargoType
    {
        [ProtoMember(1)]
        public ML.Property.PosNegPercent PNP { get; set; }
        [ProtoMember(2)]
        public ML.Property.Emotion Emotion { get; set; }
        [ProtoMember(3)]
        public DateTime ReportedTime { get; set; }
        public Report()
        {
            ReportedTime = DateTime.Now;
        }
    }
    #endregion
}
