using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoAP.EndPoint.Resources;
using CoAP;

namespace SNMPCoAPGateway.CoAPAgent.Resources
{
    public class SysName : LocalResource, GatewayResource
    {
        public SysName()
            : base("1.3.6.1.2.1.1.5.0")
        { }

        public override void DoGet(Request request)
        {
            Response response = new Response(Code.Content);

            response.PayloadString = Value();

            request.Respond(response);
        }

        public string Value()
        {
            return Environment.MachineName;
        }
    }
}
