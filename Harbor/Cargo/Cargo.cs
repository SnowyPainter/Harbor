using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Harbor.Cargo
{
    [Serializable]
    public abstract class Cargo
    {
        [XmlIgnore]
        public bool IsLocked { get; private set; }

        [XmlAttribute]
        public readonly CargoType Type;
        /// <summary>
        /// Very Worth Data
        /// PrimaryTime is the first time to load to ship. it preserve as filename.
        /// </summary>
        public DateTime? PrimaryTime { get; set; }

        public Cargo(CargoType t)
        {
            Type = t;
        }
        public void SetPrimaryTimeNow()
        {
            PrimaryTime = DateTime.Now;
        }
        public abstract bool IsEmpty();
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }

        public abstract byte[] ToPacket();
    }
}
