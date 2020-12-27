using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

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
        /// <summary>
        /// Init ReportedTime with DateTime.Now
        /// </summary>
        public Report()
        {
            ReportedTime = DateTime.Now;
        }
        public Report(DateTime dateTime)
        {
            ReportedTime = dateTime;
        }

        public byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(LogCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return Encoding.UTF8.GetBytes(xml);
        }
    }
    #endregion
}
