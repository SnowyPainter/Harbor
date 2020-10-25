using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADPC.Cargo
{
    public static class CargoExtension
    {
        public static string ToDefault(this DateTime t)
        {
            return t.ToString("yyyyMMddHHmmssFFF"); // Why milisec? user clicking time gap
        }
        public static bool IsValidPath(this string path)
        {

            bool isValid = true;
            try
            {
                string fullPath = Path.GetFullPath(path);

                string root = Path.GetPathRoot(path);
                isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;

        }
    }
}
