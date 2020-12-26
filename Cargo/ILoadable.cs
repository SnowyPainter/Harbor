using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Cargo
{
    //C# 8.0 인터페이스 기본 구현 사용 필요 / 7

    public interface ILoadable
    {
        bool IsLocked { get; set; }
        CargoType Type { get; }
        //PrimaryTime is the first time to load to ship. it preserve as filename. never change
        DateTime? PrimaryTime { get; }
        void SetPrimaryTimeOnce(DateTime t);
        bool IsEmpty();
        //All the cargos must be locked before load at ship.
        void Lock();
        void UnLock();
    }
}
