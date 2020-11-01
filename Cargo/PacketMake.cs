using System;
using System.IO;
using ProtoBuf;

namespace ADPC.Cargo
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
                Serializer.Serialize(mstream,cargo);
                packet = mstream.ToArray();
            }
            return packet;
        }
    }

}
