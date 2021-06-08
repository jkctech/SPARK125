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

        /// <summary>
        /// Get result of a command in a list of bytes
        /// </summary>
        /// <param name="command">Commandstring</param>
        /// <returns>List of Proprietary + ASCII bytes</returns>
        public List<int> CommandAsBytes(string command)
		{
            // Write command
            Write(command);

            // Read result
            List<int> result = new List<int>();

            int b = 0;

            // While not at the terminating byte...
            while (b != '\r')
			{
                int rb = Port.ReadByte();
               
                // Only save if not -1 (No char available)
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
