using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lextm.SharpSnmpLib;
using System.Diagnostics;

namespace SNMPCoAPGateway.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var operations = Enumerable.Range(0xA0, 0xAF);
            foreach (var snmpType in EnumUtil.GetValues<SnmpType>())
            {
                if (operations.Contains((int)snmpType))
                    Debug.WriteLine(snmpType);
            }
        }
    }
}
