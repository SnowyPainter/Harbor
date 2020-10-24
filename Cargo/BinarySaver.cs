using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

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
    public class BinarySaver
    {
        public string Savepath { get; set; }
        public void TransferToBinary<T>(T objectToWrite, string path = "")
        {
            using (Stream stream = File.Open(path != "" ? path : Savepath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public T TransferBinary<T>(string path = "")
        {
            using (Stream stream = File.Open(path != "" ? path : Savepath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

    }
}
