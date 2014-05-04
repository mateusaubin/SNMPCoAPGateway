using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway.CoAPAgent.Resources
{
    class SysUpTime : ISNMPResource
    {
        protected readonly DateTime START_TIME = DateTime.Now;

        public bool CanGet
        {
            get { return true; }
        }

        public bool CanSet
        {
            get { return false; }
        }

        public string Identifier
        {
            get { return "1.3.6.1.2.1.1.3.0"; }
        }

        public DataType Type
        {
            get { return DataType.TimeSpan; }
        }

        public object Value
        {
            get
            {
                return DateTime.Now - START_TIME;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
