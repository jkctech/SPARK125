using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace SPARK125
{
    class Scanner
    {
        public string PortName;
        public SerialPort Port;

        public string Model;
        public string Firmware;

        public Scanner(string portName)
		{
            PortName = portName;
            Port = new SerialPort(PortName, 9600);
            Port.ReadTimeout = 250;

            // Attempt to open the serial port
            try
            {
                Port.Open();
            }
            catch (Exception ex)
            {
                throw new System.IO.IOException(ex.Message);
            }

            // See if compatible scanner
            Model = Command("MDL").Substring(4);
            Firmware = Command("VER").Substring(4);
        }

        public bool IsReady()
        {
            return Port != null && Port.IsOpen;
        }

        private void Write(string command)
		{
            if (!IsReady())
                throw new System.IO.IOException("No active connection.");

            // Write command
            Port.Write(command + '\r');
        }

        public string Command(string command)
        {
            Write(command);

			// Read result
			return Port.ReadTo("\r");
		}

        public List<int> CommandAsBytes(string command)
		{
            // Write
            Write(command);

            // Read
            List<int> result = new List<int>();

            int b = 0;

            while (b != '\r')
			{
                int rb = Port.ReadByte();
                if (rb != -1)
                {
                    b = rb;
                    if (b != '\r')
                        result.Add(rb);
                }
			}

            return result;
		}
	}
}
