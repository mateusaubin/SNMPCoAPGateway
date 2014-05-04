using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SNMPCoAPGateway.Engine;
using CoAP;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SNMPCoAPGateway.SNMP.Pipeline
{
    class CoAPEngine : IEngine
    {
        public string AgentAddress
        {
            get
            {
                return "coap://localhost";
            }
        }

        public Message Process(Message request)
        {
            var responseList = new List<DataUnit>();

            foreach (var data in request.Data)
            {
                var coapReq = ToCoAPRequest(data);
                var coapRes = GetResponse(coapReq);
                var responseData = ToResponseData(data, coapRes);
                responseList.Add(responseData);
            }

            var response = new Message(responseList);

            return response;
        }

        private DataUnit ToResponseData(DataUnit data, Response coapRes)
        {
            var response = DataUnit.Copy(data);
            var val = Serializer.FromBinary(data.Type, coapRes.Payload);
            response.Value = val;
            return response;
        }

        private Response GetResponse(Request request)
        {
            request.Execute();
            var response = request.ReceiveResponse();
            return response;
        }

        private Request ToCoAPRequest(DataUnit data)
        {
            var code = ToCode(data.Operation);
            var request = new Request(code);

            var url = string.Join("/", AgentAddress, data.Identifier);

            request.URI = new Uri(url);
            request.Token = TokenManager.Instance.AcquireToken();

            if (data.Operation == Operation.Set)
                request.Payload = Serializer.ToBinary(data.Type, data.Value);

            return request;
        }

        private int ToCode(Operation operation)
        {
            switch (operation)
            {
                case Operation.Get:
                    return Code.GET;
                case Operation.Set:
                    return Code.POST;
                case Operation.Unknown:
                default:
                    throw new UnsupportedOperationException();
            }
        }
    }
}
