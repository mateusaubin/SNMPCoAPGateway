﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNMPCoAPGateway.CoAPAgent.Resources
{
    class SysName : ISNMPResource
    {
        protected string MACHINE_NAME = Environment.MachineName;

        public bool CanGet
        {
            get { return true; }
        }

        public bool CanSet
        {
            get { return true; }
        }

        public string Identifier
        {
            get { return "1.3.6.1.2.1.1.5.0"; }
        }

        public DataType Type
        {
            get { return DataType.String; }
        }

        public object Value
        {
            get
            {
                return MACHINE_NAME;
            }
            set
            {
                MACHINE_NAME = (string)value;
            }
        }
    }
}
