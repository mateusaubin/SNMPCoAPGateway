using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoAP.EndPoint;
using CoAP;
using CoAP.EndPoint.Resources;
using SNMPCoAPGateway.CoAPAgent.Resources;

namespace SNMPCoAPGateway.CoAPAgent
{
    class Agent : LocalEndPoint
    {
#if COAPALL
        static ISpec Spec = CoAP.Spec.Draft13;
#endif

        public Agent()
#if COAPALL
            : base(Spec)
#endif
        {
            AddResource(new SNMPResource<SysName>());
            AddResource(new SNMPResource<SysUpTime>());
        }

        static void Main(String[] args)
        {
            try
            {
                var server = new Agent();
                Console.WriteLine("Press any key to stop . . . ");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
