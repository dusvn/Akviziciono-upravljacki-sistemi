using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class for read digital out 
    /// </summary>
    public class ReadCoilsFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCoilsFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
		public ReadCoilsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
        }

        /// <inheritdoc/>
        public override byte[] PackRequest()
        {
            byte[] request = new byte[12];
            ModbusReadCommandParameters ModbusRead = this.CommandParameters as ModbusReadCommandParameters;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.TransactionId)), 0, request, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.ProtocolId)), 0, request, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.Length)), 0, request, 4, 2);
            request[6] = ModbusRead.UnitId;
            request[7] = ModbusRead.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.StartAddress)), 0, request, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.Quantity)), 0, request, 10, 2);

            return request;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            ModbusReadCommandParameters ModbusRead = this.CommandParameters as ModbusReadCommandParameters;

            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            int count = 0;
            ushort adresa = ModbusRead.StartAddress;
            ushort value;
            byte maska = 1;
            for (int i = 0; i < response[8]; i++)
            {
                byte tempbyte = response[9 + i];
                for (int j = 0; j < 8; j++)
                {
                    value = (ushort)(tempbyte & maska);
                    tempbyte >>= 1;
                    dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, adresa), value);
                    count++;
                    adresa++;
                    if (count == ModbusRead.Quantity)
                    {
                        break;
                    }

                }
            }
            return dic; 
        }
    }
}