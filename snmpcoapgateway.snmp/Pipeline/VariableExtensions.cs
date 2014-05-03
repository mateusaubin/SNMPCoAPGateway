using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib;

namespace SNMPCoAPGateway.SNMP.Pipeline
{
    static class VariableExtensions
    {
        static readonly Func<Variable, DataType>[] Detectors = new Func<Variable, DataType>[] { DetectDataTypeFromPdu, DetectDataTypeFromMIB };

        public static DataType DetectDataType(this Variable variable)
        {
            foreach (var detector in Detectors)
            {
                var type = detector.Invoke(variable);

                if (type != DataType.Unknown)
                    return type;
            }

            return DataType.Unknown;
        }

        public static Variable ToVariable(this DataUnit data)
        {
            var oid = new ObjectIdentifier(data.Identifier);
            var oct = new OctetString(data.Value.ToString());

            var result = new Variable(oid, oct);
            //var datatype = DataTypeToTypeCode(data.Type);

            return result;
        }

        static DataType DetectDataTypeFromMIB(Variable item)
        {
            //TODO: Implementar o lookup de tipos pela MIB
            return DataType.String;
        }

        static DataType DetectDataTypeFromPdu(Variable item)
        {
            return TypeCodeToDataType(item.Data.TypeCode);
        }

        public static DataType TypeCodeToDataType(SnmpType typeCode)
        {
            switch (typeCode)
            {
                case SnmpType.Integer32:
                    return DataType.Int;
                case SnmpType.OctetString:
                    return DataType.String;
                case SnmpType.Null:
                case SnmpType.Opaque:
                    return DataType.Unknown;
                case SnmpType.IPAddress:
                case SnmpType.NetAddress:
                    return DataType.IPAddress;
                case SnmpType.Counter32:
                case SnmpType.Gauge32:
                    return DataType.UInt;
                case SnmpType.TimeTicks:
                    return DataType.TimeSpan;
                case SnmpType.Counter64:
                    return DataType.ULong;
                default:
                    throw new UnsupportedTypeException();
            }
        }
    }
}
