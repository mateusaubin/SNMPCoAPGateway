using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway.Engine
{
    public interface IEngine
    {
        Message Process(Message request);
    }
}
