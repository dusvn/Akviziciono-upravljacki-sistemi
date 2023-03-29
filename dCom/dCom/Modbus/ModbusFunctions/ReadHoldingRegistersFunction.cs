using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Xml.Schema;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read holding registers functions/requests.
    /// This is class for reading out from analog signals 
    /// </summary>
    public class ReadHoldingRegistersFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadHoldingRegistersFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public ReadHoldingRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
        }

        /// <summary>
        /// Pack message for modbusSim who will give answer if this message request is valid 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.ProtocolId)), 0, request, 2,2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.Length)), 0, request, 4, 2);
            request[6] = ModbusRead.UnitId;
            request[7] = ModbusRead.FunctionCode;

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.StartAddress)), 0, request, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ModbusRead.Quantity)), 0, request, 10, 2);

            return request;
        }

        /// <summary>
        /// Using infos from modbus and reading data 
        /// Key is tuple (Type of signal,adress of signal)
        /// Point Type
        /// DIGITAL_OUTPUT = 0x01,
		//  DIGITAL_INPUT = 0x02,
		//  ANALOG_INPUT = 0x03,
		//  ANALOG_OUTPUT = 0x04,
		//  HR_LONG = 0x05,
        // Value meesage back from modbus sim(ocitana vrednost) 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            //TO DO: IMPLEMENT
            Dictionary<Tuple<PointType, ushort>, ushort> read_value = new Dictionary<Tuple<PointType, ushort>, ushort>();
            /*
            byte b; // single byte 
            ushort len = response[8];
            ushort adress = response[9];
            ushort value; 

            for(int i = 0; i < len/2; ++i)
            {
                b= response[i];
                value = (ushort)response[8+i<<2];
            }
            read_value.Add(<PointType.ANALOG_OUTPUT,>)

            */ 
            return read_value;
            
        }
    }
}