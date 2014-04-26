using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib;

namespace SNMPCoAPGateway.SNMP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = new Server();
                server.Engine.Start();
                //TODO: ver um jeito de não deixar o server terminar....
                Console.WriteLine("Press any key to stop . . . ");
                Console.ReadKey();
                server.Engine.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
