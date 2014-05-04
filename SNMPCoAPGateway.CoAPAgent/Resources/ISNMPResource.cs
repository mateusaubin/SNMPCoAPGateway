using System;
namespace SNMPCoAPGateway.CoAPAgent.Resources
{
    interface ISNMPResource
    {
        bool CanGet { get; }
        bool CanSet { get; }
        string Identifier { get; }
        DataType Type { get; }
        object Value { get; set; }
    }
}
