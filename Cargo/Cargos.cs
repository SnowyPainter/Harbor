using ADPC.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ADPC.Cargo
{
    public enum CargoType
    {
        GenericObject,
        Text,
        Voice,
        Log
    }
    public class RawCargo<T> : ILoadable //A raw data 
    {
        public CargoType Type { get; } = CargoType.GenericObject;
        public T Data { get; set; }

        public RawCargo() 
        {
        }
        public string GetRaw()
        {
            return Data.ToString();
        }
    }
    public class TextCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Text;

    }
    public class VoiceCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Voice;

    }
    public class LogCargo : ILoadable
    {
        public CargoType Type { get; } = CargoType.Log;

        private Stack<IActivityLog> logs { get; set; }
        public bool IsLocked = false;

        public LogCargo()
        {
            logs = new Stack<IActivityLog>();
        }

        public void Load(DateTime time, FrameworkElement element, Point mousePoint)
        {
            if(!IsLocked)
                logs.Push(new ActivityLog(time, element, mousePoint));
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public Stack<IActivityLog> GetLogs()
        {
            return logs;
        }
    }
}
