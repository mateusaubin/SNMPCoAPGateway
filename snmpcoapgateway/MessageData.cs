using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class MessageData
    {
        public string Identifier { get; private set; }

        public DataType Type { get; private set; }

        public object Value { get; set; }

        public MessageData(string identifier, DataType type)
        {
            this.Identifier = identifier;
            this.Type = type;
        }
    }
}
