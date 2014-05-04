using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using SNMPCoAPGateway.Messages;
using SNMPCoAPGateway.SNMP;

namespace SNMPCoAPGateway.Tests
{
    /// <summary>
    /// Summary description for MessageFactoryTest
    /// </summary>
    [TestClass]
    public class MessageFactoryTest
    {
        private Random Rand { get; set; }

        private SNMPMessageFactory Factory { get; set; }

        public MessageFactoryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Rand = new Random();
            Factory = new SNMPMessageFactory();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Get()
        {
            var version = VersionCode.V2;
            var community = new OctetString("public");
            var vlist = new List<Variable> { 
                new Variable(
                    new ObjectIdentifier(".1.3.6.1.2.1.1.5.0") //sysname
                )
            };
            var snmpMsg = new GetRequestMessage(Rand.Next(), version, community, vlist);

            var msg = Factory.ToForward(snmpMsg);

            Assert.IsTrue(msg.Data.All(x => x.Operation == Operation.Get));
            Assert.AreEqual(vlist.Count, msg.Data.Count);
        }
    }
}
