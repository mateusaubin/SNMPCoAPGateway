using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class UnsupportedOperationException : Exception
    {
        const string DEFAULT_MESSAGE = "Unsupported Operation";

        public UnsupportedOperationException()
            : this(null)
        { }

        public UnsupportedOperationException(string message)
            : this(message, null)
        { }

        public UnsupportedOperationException(string message, Exception inner)
            : base(message ?? DEFAULT_MESSAGE, inner)
        { }
    }
}
