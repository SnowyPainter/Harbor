using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
    public interface ILoadable
    {
        bool IsLocked { get; set; }
        CargoType Type { get; }
        //PrimaryTime is the first time to load to ship. it preserve as filename. never change
        DateTime? PrimaryTime { get; }
        void SetPrimaryTimeOnce(DateTime t);
        bool IsEmpty();
        void Lock();
        void UnLock();
    }
}
