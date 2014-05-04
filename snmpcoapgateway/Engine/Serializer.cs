using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SNMPCoAPGateway.Engine
{
    public class Serializer
    {
        public static byte[] ToBinary(DataType type, object input)
        {
            byte[] result;
            switch (type)
            {
                case DataType.Int:
                    result = BitConverter.GetBytes((int)input);
                    break;
                case DataType.UInt:
                    result = BitConverter.GetBytes((uint)input);
                    break;
                case DataType.ULong:
                    result = BitConverter.GetBytes((ulong)input);
                    break;
                case DataType.TimeSpan:
                    result = BitConverter.GetBytes(((TimeSpan)input).Ticks);
                    break;
                case DataType.String:
                    var enc = new UTF8Encoding(false, true);
                    result = enc.GetBytes((string)input);
                    break;
                case DataType.IPAddress://IPAddress.GetAddressBytes
                case DataType.Unknown:
                default:
                    throw new UnsupportedTypeException();
            }

            Debug.WriteLine("  SERIALIZE: {0} ==> {1}", input, string.Join(".", result));

            return result;
        }

        public static object FromBinary(DataType type, byte[] input)
        {
            object result;

            switch (type)
            {
                case DataType.Int:
                    result = BitConverter.ToInt32(input, 0);
                    break;
                case DataType.UInt:
                    result = BitConverter.ToUInt32(input, 0);
                    break;
                case DataType.ULong:
                    result = BitConverter.ToUInt64(input, 0);
                    break;
                case DataType.TimeSpan:
                    result = TimeSpan.FromTicks(BitConverter.ToInt64(input, 0));
                    break;
                case DataType.String:
                    var enc = new UTF8Encoding(false, true);
                    result = enc.GetString(input);
                    break;
                case DataType.IPAddress://IPAddress.GetAddressBytes
                case DataType.Unknown:
                default:
                    throw new UnsupportedTypeException();
            }

            Debug.WriteLine("DESERIALIZE: {0} ==> {1}", string.Join(".", input), result);

            return result;
        }
    }
}
