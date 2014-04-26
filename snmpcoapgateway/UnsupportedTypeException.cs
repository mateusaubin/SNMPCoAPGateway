using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class UnsupportedTypeException : Exception
    {
        const string DEFAULT_MESSAGE = "Unsupported Type";

        public UnsupportedTypeException()
            : this(null)
        { }

        public UnsupportedTypeException(string message)
            : this(message, null)
        { }

        public UnsupportedTypeException(string message, Exception inner)
            : base(message ?? DEFAULT_MESSAGE, inner)
        { }
    }
}
