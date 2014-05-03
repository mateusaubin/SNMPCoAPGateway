using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class InvalidMessageDataException : Exception
    {
        const string DEFAULT_MESSAGE = "Invalid message data";

        public InvalidMessageDataException()
            : this(null)
        { }

        public InvalidMessageDataException(string message)
            : this(message, null)
        { }

        public InvalidMessageDataException(string message, Exception inner)
            : base(message ?? DEFAULT_MESSAGE, inner)
        { }
    }
}
