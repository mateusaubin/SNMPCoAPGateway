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

        public static DataUnit Copy(DataUnit src)
        {
            var result = new DataUnit(src.Identifier, src.Type, src.Operation);
            return result;
        }
    }
}
