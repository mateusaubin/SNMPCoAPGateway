using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class Message
    {
        public IList<DataUnit> Data { get; private set; }

        public Message(IEnumerable<DataUnit> data)
        {
            this.Data = data.ToArray();
        }
    }
}
