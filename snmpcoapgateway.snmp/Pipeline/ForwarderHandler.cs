using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Pipeline;
using SNMPCoAPGateway.Engine;
using Lextm.SharpSnmpLib;
using SNMPCoAPGateway.Messages;

namespace SNMPCoAPGateway.SNMP.Pipeline
{
    public class ForwarderHandler : IMessageHandler
    {
        IEngine ForwarderEngine { get; set; }

        IMessageFactory MessageFactory { get; set; }

        public ForwarderHandler(IEngine forwarder, IMessageFactory converter)
        {
            MessageFactory = converter;
            ForwarderEngine = forwarder;
        }

        public void Handle(ISnmpContext context, ObjectStore store)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            //if (store == null)
            //{
            //    throw new ArgumentNullException("store");
            //}

            try
            {
                // converte
                var req = MessageFactory.ToForward(context.Request);

                // pergunta ao terceiro
                var reply = ForwarderEngine.Process(req);

                // converte
                var res = (IList<Variable>)MessageFactory.FromForward(reply);

                // responde
                context.GenerateResponse(res);
            }
            catch (Exception)
            {
                context.CopyRequest(ErrorCode.GenError, 0);
            }
        }
    }
}
