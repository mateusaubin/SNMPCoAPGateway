using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Lextm.SharpSnmpLib;
using System.Net.Sockets;
using System.Net;

namespace SNMPCoAPGateway.SNMP
{
    class Server
    {
        internal static IUnityContainer Container { get; private set; }

        public SnmpEngine Engine { get; private set; }

        public Server()
        {
            Container = new UnityContainer();
            Container.LoadConfiguration("agent");

            // TODO: this is a hack. review it later.
            var store = Container.Resolve<ObjectStore>();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());

            var users = Container.Resolve<UserRegistry>();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            users.Add(
                new OctetString("privacy"),
                new DESPrivacyProvider(
                    new OctetString("privacyphrase"),
                    new MD5AuthenticationProvider(new OctetString("authentication"))));




            Engine = Container.Resolve<SnmpEngine>();



            const int port = 5512;
            //using (var Engine = Container.Resolve<SnmpEngine>())
            //{
                Engine.ExceptionRaised += (sender, e) =>
                {
                    System.Diagnostics.Debugger.Break();
                    throw e.Exception;
                };

                Engine.Listener.ClearBindings();
                if (Socket.OSSupportsIPv4)
                    Engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));

                if (Socket.OSSupportsIPv6)
                    Engine.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
            //}
        }
    }
}
