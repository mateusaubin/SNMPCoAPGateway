using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Pipeline;

namespace SNMPCoAPGateway.SNMP.Pipeline
{
    public class ForwarderHandler : IMessageHandler
    {
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


            var commonFactory = new SNMPMessageFactory();

            var msg = commonFactory.Create(context.Request);



        }
    }
}
