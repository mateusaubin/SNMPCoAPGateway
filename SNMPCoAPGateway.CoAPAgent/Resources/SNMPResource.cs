using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoAP.EndPoint.Resources;
using CoAP;
using SNMPCoAPGateway.Engine;
using System.Diagnostics;

namespace SNMPCoAPGateway.CoAPAgent.Resources
{
    class SNMPResource<T> : LocalResource
        where T : ISNMPResource
    {
        static readonly T Instance;

        static SNMPResource()
        {
            var tmp = Activator.CreateInstance<T>();
            Instance = tmp;
        }

        public SNMPResource()
            : base(Instance.Identifier)
        {
            ContentTypeCode = MediaType.ApplicationOctetStream;
        }

        public override void DoGet(Request request)
        {
            if (!Instance.CanGet)
                request.Respond(Code.MethodNotAllowed);

            Response response = new Response(Code.Content);

            var val = Instance.Value;
            response.Payload = Serializer.ToBinary(Instance.Type, val);

            request.Respond(response);
        }

        public override void DoPost(Request request)
        {
            if (!Instance.CanSet)
                request.Respond(Code.MethodNotAllowed);

            Response response = new Response(Code.Changed);

            var input = Serializer.FromBinary(Instance.Type, request.Payload);
            Instance.Value = input;

            var output = Instance.Value;
            response.Payload = Serializer.ToBinary(Instance.Type, output);

            request.Respond(response);
        }
    }
}
