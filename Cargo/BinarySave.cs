using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Harbor.Cargo
{
    public class BinarySave
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
                var deserialized = binaryFormatter.Deserialize(stream);
                if (deserialized is T)
                    return (T)deserialized;
                else
                    return default;
            }
        }

    }
}
