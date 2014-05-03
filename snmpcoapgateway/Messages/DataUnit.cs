using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway
{
    public class DataUnit
    {
        public string Identifier { get; private set; }

        public Operation Operation { get; set; }

        public DataType Type { get; private set; }

        public object Value { get; set; }

        public DataUnit(string identifier, DataType type, Operation operation)
        {
            this.Identifier = identifier;
            this.Operation = operation;
            this.Type = type;
        }

        public void Validate()
        {
            if (Operation == Operation.Set && Value == null)
                throw new InvalidMessageDataException();
        }
    }
}
