using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Harbor.Cargo
{
    public class PacketMake //cargo report -> protobuf
    {
        public PacketMake()
        {

        }

        public byte[] ToPacket(ILoadable cargo)
        {
            byte[] packet;
            using(var mstream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(mstream,cargo);
                packet = mstream.ToArray();
            }
            return packet;
        }
    }

}
