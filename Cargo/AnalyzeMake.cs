using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
    public static class AnalyzeMake
    {

        public static CargoReport Texts(TextCargo cargo)
        {
            var report = new CargoReport();

            return report;
        }
        public static CargoReport Voices(VoiceCargo cargo)
        {
            var report = new CargoReport();

            return report;
        }
        public static CargoReport Logs(LogCargo cargo)
        {
            var report = new CargoReport();

            report.Emotion = ML.Property.Emotion.Angry;
            report.PNP = new ML.Property.PosNegPercent { Positive = 0, Negative = 100 };

            if(!report.PNP.IsValid())
            {
                //Re ML
                return null;
            }

            return report;
        }
    }
}
