using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read input registers functions/requests.
    /// </summary>
    public class ReadInputRegistersFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadInputRegistersFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public ReadInputRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
        }

        /// <inheritdoc />
        public override byte[] PackRequest()
        {
            byte[] request = new byte[12];
            // In command parametar we have low bytes of requers 
            // For high bytes we need to cast this in this section Read 
            ModbusReadCommandParameters ModbusRead = this.CommandParameters as ModbusReadCommandParameters;
            // 1. parametar of Block Copy is source Buffer
            // 2. Start byte of ModbusRead
            // 3. destination buffer
            // 4. Start byte of request 
            // 5. Count of bytes to copy into request 
            // Start of 1. is 0 becous we access in the start of TransactionId
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
            ModbusReadCommandParameters mdmReadCommParams = this.CommandParameters as ModbusReadCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();


            byte second_byte = response[10];
            byte first_byte = response[9];

            //ushort value = (ushort)(first_byte+(second_byte<<8));
            // if we have analog value >255 we need more then one byte 
            ushort value = (ushort)second_byte;

            dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, mdmReadCommParams.StartAddress), value);
            return dic;
        }
    }
}