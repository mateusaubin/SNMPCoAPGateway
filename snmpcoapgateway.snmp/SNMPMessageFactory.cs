using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using SNMPCoAPGateway.Messages;

namespace SNMPCoAPGateway.SNMP
{
    public class SNMPMessageFactory : IMessageFactory
    {
        public Message Create(ISnmpMessage message)
        {
            IEnumerable<DataUnit> msgData = ExtractMessageData(message.Scope.Pdu);

            var result = new Message(msgData);

            return result;
        }

        private IEnumerable<DataUnit> ExtractMessageData(ISnmpPdu pdu)
        {
            var operation = DetectOperation(pdu.TypeCode);

            foreach (var item in pdu.Variables)
            {
                DataType type = DetectDataType(item.Data.TypeCode);

                DataUnit data = new DataUnit(item.Id.ToString(), type, operation);

                if (type != DataType.Unknown)
                    data.Value = item.Data.ToString();

                data.Validate();

                yield return data;
            }
        }

        private Operation DetectOperation(SnmpType pduType)
        {
            switch (pduType)
            {
                /*
            case SnmpType.GetRequestPdu:
            case SnmpType.GetNextRequestPdu:
            case SnmpType.ResponsePdu:
            case SnmpType.SetRequestPdu:
            case SnmpType.TrapV1Pdu:
            case SnmpType.GetBulkRequestPdu:
            case SnmpType.InformRequestPdu:
            case SnmpType.TrapV2Pdu:
            case SnmpType.ReportPdu:
                 */
                case SnmpType.GetRequestPdu:
                    return Operation.Get;
                case SnmpType.SetRequestPdu:
                    return Operation.Set;
                default:
                    throw new UnsupportedOperationException();
            }
        }

        private DataType DetectDataType(SnmpType snmpType)
        {
            switch (snmpType)
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

        public bool CanHandle(object message)
        {
            return message is ISnmpMessage;
        }

        public Message Create(object message)
        {
            if (!this.CanHandle(message))
                throw new UnsupportedTypeException("Unrecognized message type");

            return this.Create((ISnmpMessage)message);
        }
    }
}
