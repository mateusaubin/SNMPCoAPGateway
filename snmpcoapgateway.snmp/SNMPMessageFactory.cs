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
        public Message FromRequest(ISnmpMessage message)
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

        public bool CanHandle(object message)
        {
            return message is ISnmpMessage;
        }

        public Message Create(object message)
        {
            if (!this.CanHandle(message))
                throw new UnsupportedTypeException("Unrecognized message type");

            return this.FromRequest((ISnmpMessage)message);
        }

        public IList<Variable> ToResponse(Message response)
        {
            return this.ToResponseIEnumerable(response).ToList();
        }

        private IEnumerable<Variable> ToResponseIEnumerable(Message response)
        {
            foreach (var item in response.Data)
            {
                yield return item.ToVariable();
            }
        }
    }
}
