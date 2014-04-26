using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;

namespace SNMPCoAPGateway.SNMP
{
    class MessageFactory
    {
        public Message Create(ISnmpMessage message)
        {
            Message ret;

            MessageType msgType = DetectMessageType(message.Scope.Pdu.TypeCode);
            IEnumerable<MessageData> msgData = ExtractMessageData(message.Scope.Pdu);

            ret = new Message(msgType, msgData);

            return ret;
        }

        private IEnumerable<MessageData> ExtractMessageData(ISnmpPdu iSnmpPdu)
        {
            foreach (var item in iSnmpPdu.Variables)
            {
                DataType type = DetectDataType(item.Data.TypeCode);
                MessageData data = new MessageData(item.Id.ToString(), type);

                if (type != DataType.Unknown)
                    data.Value = item.Data.ToString();

                yield return data;
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

        private MessageType DetectMessageType(SnmpType snmpType)
        {
            switch (snmpType)
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
                    return MessageType.Request;
                case SnmpType.SetRequestPdu:
                    return MessageType.Request;
                default:
                    throw new UnsupportedOperationException();
            }
        }

    }
}
