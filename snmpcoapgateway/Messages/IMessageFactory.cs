using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway.Messages
{
    public interface IMessageFactory
    {
        bool CanHandle(object message);

        Message Create(object message);
    }
}
