using System;
using System.Collections.Generic;

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
    public class Report //Analyzed cargo by cargoType
    {
        public ML.Property.PosNegPercent PNP { get; set; }
        public ML.Property.Emotion Emotion { get; set; }
        public DateTime ReportedTime { get; set; }
        public Report()
        {
            ReportedTime = DateTime.Now;
        }
    }
    #endregion
}
