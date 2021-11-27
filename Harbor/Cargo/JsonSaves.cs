using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Harbor.Cargo
{
    public class JsonSaves
    {
        public void SaveToObject<T>(T objectToWrite, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                JsonSerializer.Serialize(stream, objectToWrite);
            }
        }

        public T? TransferToObject<T>(string path)
        {
            var text = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(text);
        }

    }
}
