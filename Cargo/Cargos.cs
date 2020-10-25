using ADPC.Log;
using System;
using System.Collections.Generic;
using System.Text;
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
    [Serializable]
    public class RawCargo : ILoadable //A raw data 
    {
        public CargoType Type { get; } = CargoType.GenericObject;
        public DateTime? PrimaryTime { get; private set; }
        public string Data { get; set; }

        public RawCargo() 
        {
        }
        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;
        public string GetRaw()
        {
            return Data.ToString();
        }
        public bool IsEmpty()
        {
            return Data == null || Data == "" ? true : false;
        }
    }
    [Serializable]
    public class TextCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Text;
        public DateTime? PrimaryTime { get; private set; } = null;
        private string text { get; set; }
        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;
        public bool IsEmpty()
        {
            return text == null || text.Length <= 0 ? true : false;
        }
    }
    [Serializable]
    public class VoiceCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Voice;
        public DateTime? PrimaryTime { get; private set; } = null;

        private byte[] rawVoice { get; set; }

        public void SetPrimaryTimeOnce(DateTime t) => PrimaryTime = t;
        public bool IsEmpty()
        {
            return rawVoice == null || rawVoice.Length <= 0 ? true : false;
        }
    }
    [Serializable]
    public class LogCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Log;
        public DateTime? PrimaryTime { get; private set; } = null;

        private Stack<IActivityLog> logs { get; set; }
        public bool IsLocked = false;

        public LogCargo()
        {
            logs = new Stack<IActivityLog>();
        }

        public void Load(DateTime time, FrameworkElement element, Point mousePoint)
        {
            if (!IsLocked)
                logs.Push(new ActivityLog(time, element, mousePoint));
            else
                throw new CargoException("Already locked cargo");
        }
        public void Load(IActivityLog log)
        {
            if (!IsLocked)
                logs.Push(log);
            else
                throw new CargoException("Already locked cargo");
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

        public Stack<IActivityLog> GetLogs()
        {
            return logs;
        }
        public bool IsEmpty()
        {
            return logs.Count <= 0 ? true : false;
        }
    }
}
