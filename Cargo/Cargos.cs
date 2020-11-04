using ADPC.Log;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ADPC.Cargo
{
    [Serializable]
    public enum CargoType
    {
        GenericObject,
        Text,
        Voice,
        Log
    }

    #region Static Extention
    public static class CargosExtension
    {
        public static T Pop<T>(this List<T> cargos, int index) where T : ILoadable
        {
            var c = cargos[index];
            cargos.RemoveAt(index);
            return c;
        }
    }
    #endregion

    #region Cargo ProtoContracts & Serializables
    [Serializable]
    [ProtoContract]
    public class RawCargo : ILoadable //A raw data 
    {
        public CargoType Type { get; } = CargoType.GenericObject;
        public DateTime? PrimaryTime { get; private set; }
        public bool IsLocked { get; set; } = false;
        [ProtoMember(1)]
        public byte[] Data { get; set; }

        public RawCargo()
        {
        }
        public string GetRaw()
        {
            return Data.ToString();
        }
        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;

        public bool IsEmpty()
        {
            return Data == null ? true : false;
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }
    }

    [Serializable]
    [ProtoContract]
    public class TextCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Text;
        [ProtoMember(1)]
        public DateTime? PrimaryTime { get; private set; } = null;
        public bool IsLocked { get; set; } = false;
        [ProtoMember(2)]
        public List<string> Texts { get; private set; }
        public TextCargo()
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
        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;
        public bool IsEmpty()
        {
            return Texts == null || Texts.Count <= 0 ? true : false;
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }
    }
    [Serializable]
    [ProtoContract]
    public class VoiceCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Voice;
        [ProtoMember(1)]
        public DateTime? PrimaryTime { get; private set; } = null;
        public bool IsLocked { get; set; } = false;
        [ProtoMember(2)]
        public byte[] RawVoice { get; set; }

        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;
        public bool IsEmpty()
        {
            return RawVoice == null || RawVoice.Length <= 0 ? true : false;
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }
    }
    [Serializable]
    [ProtoContract]
    public class LogCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Log;
        [ProtoMember(1)]
        public DateTime? PrimaryTime { get; private set; } = null;
        [ProtoMember(2)]
        private Stack<IActivityLog> logs { get; set; }

        public bool IsLocked { get; set; } = false;

        public LogCargo()
        {
            logs = new Stack<IActivityLog>();
        }

        public void Load(DateTime time, FrameworkElement element, Point mousePoint)
        {
            if (!IsLocked)
                logs.Push(new ActivityLog(time, element, mousePoint));
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public void Load(IActivityLog log)
        {
            if (!IsLocked)
                logs.Push(log);
            else
                throw new CargoException(CargoExceptionMsg.Locked);
        }
        public Stack<IActivityLog> GetLogs()
        {
            return logs;
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }
        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;


        public bool IsEmpty()
        {
            return logs.Count <= 0 ? true : false;
        }
    }

    #endregion
}
