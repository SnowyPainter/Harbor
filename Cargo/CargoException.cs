using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Cargo
{
    public static class CargoExceptionMsg
    {
        public static readonly string NotLocked = "Cargo is not locked";
        public static readonly string Locked = "Cargo has been already locked";
        public static readonly string Empty = "Cargo is empty, so It can't be loaded to ship";
    }
    public class CargoException:Exception
    {
        public CargoException()
        {
        }

        public CargoException(string message)
            : base(message)
        {
        }
    }
}
