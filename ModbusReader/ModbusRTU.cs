using Modbus.Device;
using Modbus.Serial;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusReader
{
    public class ModbusRTU: Modbus
    {
        public SerialPort SerialPort { get; private set; }

        public ModbusRTU(uint comPort, byte nodeID, int baudRate = 9600, 
            int dataBits = 8, Parity parity = Parity.None, StopBits stopBits = StopBits.One) 
        {
            SerialPort = new SerialPort($"COM{comPort}") {
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = parity,
                StopBits = stopBits
            };

            SerialPort.Open();
            var adapter = new SerialPortAdapter(SerialPort);
            ModbusMaster = ModbusSerialMaster.CreateRtu(adapter);
        }

        public void ClosePort() {
            if (SerialPort != null && SerialPort.IsOpen) {
                SerialPort.Close();
            }
        }
    }
}
