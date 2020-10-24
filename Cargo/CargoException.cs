using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
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
