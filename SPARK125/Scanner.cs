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
    public class Scanner
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
			Port.ErrorReceived += Port_ErrorReceived;

            _openPort();

            // See if compatible scanner
            Model = Command("MDL").Substring(4);
            Firmware = Command("VER").Substring(4);
        }

        private void _openPort()
		{
            // Attempt to open the serial port
            try
            {
                if (Port.IsOpen)
                    Port.Close();
                Port.Open();
            }
            catch (Exception ex)
            {
                throw new System.IO.IOException(ex.Message);
            }
        }

        public void Close()
		{
            if (Port.IsOpen)
                Port.Close();
        }

		private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
            _openPort();
		}

		public bool IsReady()
        {
            return Port != null && Port.IsOpen;
        }

        private void Write(string command)
		{
            if (!IsReady())
            {
                Close();
                return;
            }

            // Write command
            Port.Write(command + '\r');
        }

        /// <summary>
        /// Get result of a command as a string.
        /// </summary>
        /// <param name="command">Commandstring</param>
        /// <returns>String cast from raw bytes</returns>
        public string Command(string command, int attempt = 10)
        {
			try
			{
                Write(command);
                return Port.ReadTo("\r");
            }
            catch (Exception)
			{
                if (attempt > 0)
                    return Command(command, attempt - 1);
                else
                    throw;
			}
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
                try
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
                catch (Exception)
				{
                    return result;
				}
			}

            return result;
		}
	}
}
