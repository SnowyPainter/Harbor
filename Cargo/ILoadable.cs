using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Cargo
{
    public interface ILoadable
    {
        CargoType Type { get; }
    }
}
