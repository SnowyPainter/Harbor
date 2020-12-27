using Harbor.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Harbor.Cargo
{
    [Serializable]
    public enum CargoType
    {
        [EnumMember]
        GenericObject,
        [EnumMember]
        Text,
        [EnumMember]
        Voice,
        [EnumMember]
        Log
    }

    #region Static Extention
    public static class CargosExtension
    {
        public static T Pop<T>(this List<T> cargos, int index) where T : Cargo
        {
            var c = cargos[index];
            cargos.RemoveAt(index);
            return c;
        }
    }
    #endregion

    #region Cargo ProtoContracts & Serializables
    [Serializable]
    public class RawCargo : Cargo
    {
        public byte[] Data { get; set; }

        public RawCargo() : base(CargoType.GenericObject) { }
        public string GetRaw()
        {
            return Data.ToString();
        }

        public override bool IsEmpty()
        {
            return Data == null ? true : false;
        }

        public override byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(RawCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return Encoding.UTF8.GetBytes(xml);
        }
    }

    
    [Serializable]
    public class TextCargo : Cargo
    {
        public List<string> Texts { get; private set; }
        public TextCargo():base(CargoType.Text)
        {
            Texts = new List<string>();
        }

        public void Load(string text)
        {
            if (!IsLocked)
                Texts.Add(text);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public override bool IsEmpty()
        {
            return Texts == null || Texts.Count <= 0 ? true : false;
        }
        public override byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(TextCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return Encoding.UTF8.GetBytes(xml);
        }
    }
    [Serializable]
    public class VoiceCargo : Cargo
    {
        public byte[] RawVoice { get; set; }

        public VoiceCargo() : base(CargoType.Voice) { }

        public override bool IsEmpty()
        {
            return RawVoice == null || RawVoice.Length <= 0 ? true : false;
        }

        public override byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(VoiceCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return Encoding.UTF8.GetBytes(xml);
        }
    }
    [Serializable]
    public class LogCargo : Cargo
    {
        public Stack<IActivityLog> Logs { get; private set; }

        public LogCargo():base(CargoType.Log)
        {
            Logs = new Stack<IActivityLog>();
        }

        public void Load(DateTime time, FrameworkElement element, Point mousePoint)
        {
            if (!IsLocked)
                Logs.Push(new ActivityLog(time, element, mousePoint));
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public void Load(IActivityLog log)
        {
            if (!IsLocked)
                Logs.Push(log);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public Stack<IActivityLog> GetLogs()
        {
            return Logs;
        }

        public override bool IsEmpty()
        {
            return Logs.Count <= 0 ? true : false;
        }
        public override byte[] ToPacket()
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
