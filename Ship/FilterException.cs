using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Ship
{
    public class FilterException: Exception
    {
        public FilterException()
        {

        }
        public FilterException(string message)
            : base(message)
        {
        }
    }
}
