using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using SNMPCoAPGateway.Messages;
using SNMPCoAPGateway.SNMP.Pipeline;

namespace SNMPCoAPGateway.SNMP
{
    public class SNMPMessageFactory : IMessageFactory
    {
        public bool CanHandle(object input)
        {
            return (input is ISnmpMessage);
        }

        public Message ToForward(object message)
        {
            if (!CanHandle(message))
                throw new UnsupportedTypeException("Unknown message type");

            return this.ToRequest((ISnmpMessage)message);
        }

        public object FromForward(Message message)
        {
            return this.ToResponse(message).ToList();
        }

        private Message ToRequest(ISnmpMessage message)
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
                var oid = string.Join(".", item.Id.ToNumerical());
                DataType type = item.DetectDataType();

                DataUnit data = new DataUnit(oid, type, operation);

                if (operation == Operation.Set)
                    data.Value = item.Data.ToString();

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

        private IEnumerable<Variable> ToResponse(Message response)
        {
            foreach (var item in response.Data)
            {
                yield return item.ToVariable();
            }
        }
    }
}
