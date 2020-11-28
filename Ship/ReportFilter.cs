using Harbor.Cargo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Ship
{
    public class ReportFilter
    {
        public List<ML.Property.Emotion> PermitEmotions { get; set; }
        public ML.Property.PosNegPercent MinimumPNP { get; set; }
        public ML.Property.PosNegPercent MaximumPNP { get; set; }
        public ReportFilter()
        {
            PermitEmotions = new List<ML.Property.Emotion>();
            MinimumPNP = new ML.Property.PosNegPercent { Positive = 0, Negative = 0 };
            MaximumPNP = new ML.Property.PosNegPercent { Positive = 100, Negative = 100 };

            foreach (ML.Property.Emotion e in Enum.GetValues(typeof(ML.Property.Emotion)))
                PermitEmotions.Add(e);
        }

        public bool Validate(Report report)
        {
            return (MinimumPNP.Negative <= report.PNP.Negative && report.PNP.Negative <= MaximumPNP.Negative
                && MinimumPNP.Positive <= report.PNP.Positive && report.PNP.Positive <= MaximumPNP.Positive
                && PermitEmotions.Contains(report.Emotion));
        }
    }
}
