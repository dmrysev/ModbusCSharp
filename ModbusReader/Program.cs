using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using Modbus.Serial;
using System.Security.Permissions;

namespace ModbusReader
{
    class Program
    {
        static ModbusRTU ModbusRTU;

        static void Main(string[] args) {
            ReadModbusTCP();
            //ReadModbusRTU();
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void ReadModbusTCP() {
            using (var tcpClient = new TcpClient("192.168.230.5", 502)) {
                ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);
                var register = (ushort) Modbus.Get32BitRegister(30, 12);
                ushort reg2 = (20000 + 200 * 1 + 2 * 11) - 1;
                ushort[] inputs = master.ReadHoldingRegisters(reg2, 2);

                //foreach (var input in inputs) {
                //    Console.WriteLine(input);
                //}

                if (inputs.Length == 2) {
                    int result = (inputs[0] << 16) + inputs[1];
                    Console.WriteLine(result);
                }
            }
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void ReadModbusRTU() {

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);

            //ConnectAndRead(Protocol.MBRTU);
            ModbusRTU = new ModbusRTU(23, 1);
            int response = ModbusRTU.ReadInt32(30, 12);
            Console.WriteLine(response);

            ModbusRTU.ClosePort();
        }

        static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception) args.ExceptionObject;
            Console.WriteLine(e.Message);

            if (ModbusRTU != null && ModbusRTU.ModbusMaster != null)
                ModbusRTU.ClosePort();

            Environment.Exit(1);
        }
    }
}
