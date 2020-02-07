using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusReader
{
    public class Modbus
    {
        public byte NodeID = 1;

        public IModbusMaster ModbusMaster { get; protected set; }

        public static int Get32BitRegister(byte group, byte parameter) {
            int register = 20000 + 200 * group + 2 * parameter;

            return register;
        }

        public int ReadInt32(byte group, byte parameter) {
            int registerStart = Get32BitRegister(group, parameter) - 1;
            ushort[] response = new ushort[2];
            response = ModbusMaster.ReadHoldingRegisters(NodeID, (ushort) registerStart, 2);
            int result = (response[0] << 16) + response[1];

            return result;
        }
    }
}
