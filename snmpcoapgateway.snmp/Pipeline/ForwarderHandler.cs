using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Pipeline;
using SNMPCoAPGateway.Engine;
using Lextm.SharpSnmpLib;

namespace SNMPCoAPGateway.SNMP.Pipeline
{
    public class ForwarderHandler : IMessageHandler
    {
        public IEngine ForwarderEngine { get; set; }

        public void Handle(ISnmpContext context, ObjectStore store)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            try
            {

                var commonFactory = new SNMPMessageFactory();

                var msg = commonFactory.FromRequest(context.Request);

                ForwarderEngine = new CoAPEngine();
                var response = ForwarderEngine.Process(msg);

                IList<Variable> resp = commonFactory.ToResponse(response);

                context.GenerateResponse(resp);
                //context.GenerateResponse(variables);
                //context.Response = res
            }
            catch (Exception e)
            {
                context.CopyRequest(ErrorCode.GenError, 0);
            }
        }
    }
}
