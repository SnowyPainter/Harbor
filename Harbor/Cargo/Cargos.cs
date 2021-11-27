using Harbor.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
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
        public List<byte> Data { get; private set; }
        public RawCargo() : base(CargoType.GenericObject) { Data = new List<byte>(); }
        public void Load(byte data) {
            Data.Add(data);
        }
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
    public class DataLogCargo : Cargo
    {
        public Stack<DataLog> Logs { get; private set; }
        public DataLogCargo():base(CargoType.Log)
        {
            Logs = new Stack<DataLog>();
        }
        public void Load(DateTime time, string c)
        {
            if (!IsLocked)
                Logs.Push(new DataLog(time, c));
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public void Load(DataLog log)
        {
            if (!IsLocked)
                Logs.Push(log);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public Stack<DataLog> GetLogs()
        {
            return Logs;
        }

        public override bool IsEmpty()
        {
            return Logs.Count <= 0 ? true : false;
        }
        public override byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(WPFLogCargo));
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
    public class WPFLogCargo : Cargo
    {
        public Stack<WPFActivityLog> Logs { get; private set; }

        public WPFLogCargo():base(CargoType.Log)
        {
            Logs = new Stack<WPFActivityLog>();
        }

        public void Load(DateTime time, FrameworkElement element, Point mousePoint)
        {
            if (!IsLocked)
                Logs.Push(new WPFActivityLog(time, element, mousePoint));
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public void Load(WPFActivityLog log)
        {
            if (!IsLocked)
                Logs.Push(log);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public Stack<WPFActivityLog> GetLogs()
        {
            return Logs;
        }

        public override bool IsEmpty()
        {
            return Logs.Count <= 0 ? true : false;
        }
        public override byte[] ToPacket()
        {
            var serializer = new XmlSerializer(typeof(WPFLogCargo));
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
