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
    #region Cargo ProtoContracts & Serializables
    [Serializable]
    public class RawCargo : Cargo
    {
        public List<byte> Data { get; private set; }
        public RawCargo() : base(CargoType.GenericObject) { Data = new List<byte>(); }
        public void Load(byte data) {
            Data.Add(data);
        }
        public override bool IsEmpty()
        {
            return Data == null ? true : false;
        }

        public override string ToXMLPacket()
        {
            var serializer = new XmlSerializer(typeof(RawCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return xml;
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
        public override string ToXMLPacket()
        {
            var serializer = new XmlSerializer(typeof(TextCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return xml;
        }
    }
    [Serializable]
    public class VoiceCargo : Cargo
    {
        public byte[] RawVoice { get; set; }

        public VoiceCargo() : base(CargoType.Voice) { RawVoice = new byte[] { }; }

        public override bool IsEmpty()
        {
            return RawVoice == null || RawVoice.Length <= 0 ? true : false;
        }

        public override string ToXMLPacket()
        {
            var serializer = new XmlSerializer(typeof(VoiceCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return xml;
        }
    }
    [Serializable]
    public class DataCargo : Cargo
    {
        public Stack<Data> Datas { get; private set; }
        public DataCargo():base(CargoType.Log)
        {
            Datas = new Stack<Data>();
        }
        public void Load(Data log)
        {
            if (!IsLocked)
                Datas.Push(log);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public Stack<Data> GetDatas()
        {
            return Datas;
        }

        public override bool IsEmpty()
        {
            return Datas.Count <= 0 ? true : false;
        }
        public override string ToXMLPacket()
        {
            var serializer = new XmlSerializer(typeof(DataCargo));
            string xml = "";
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                xml = writer.ToString();
            }
            return xml;
        }
    }
    #endregion
}
