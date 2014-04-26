using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class Message
    {
        public MessageType Type { get; private set; }

        public IList<MessageData> Data { get; private set; }

        public Message(MessageType type, IEnumerable<MessageData> data)
        {
            this.Type = type;
            this.Data = data.ToList();
        }
    }
}
