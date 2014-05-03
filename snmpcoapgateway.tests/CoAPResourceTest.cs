using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPCoAPGateway.CoAPAgent.Resources;

namespace SNMPCoAPGateway.Tests
{
    [TestClass]
    public class CoAPResourceTest
    {
        [TestMethod]
        public void SysNameTest()
        {
            var r = new SysName();
            var x = r.Value();
        }
    }
}
