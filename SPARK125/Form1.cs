using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace SPARK125
{
	public partial class Form1 : Form
	{
		LCD lcd;
		public Form1()
		{
			InitializeComponent();
			UpdateSerialPorts();

			Shown += Form1_Shown;
		}

		private void Form1_Shown(Object sender, EventArgs e)
		{
			// Initialize LCD
			lcd = new LCD(
				Controls,
				new Point(200, 10),
				14,
				25,
				16
			);

			// Parse testing STS string
			lcd.ParseSTS("STS,011000, HOLD,,V67 SAR/KNRM/KWC,,CH028   156.3750,, FM,,BNK:1,,0,1,0,0,,,0,,0");
		}

		/// <summary>
		/// Get all available COM ports and list them in the dropdown
		/// </summary>
		private void UpdateSerialPorts()
		{
			// Get all available serial ports
			string[] ports = SerialPort.GetPortNames();

			// Wipe and add all ports
			combo_serial_ports.Items.Clear();
			combo_serial_ports.Items.AddRange(ports);

			// Auto select last port
			combo_serial_ports.SelectedIndex = ports.Length - 1;
		}

		private void btn_serial_refresh_Click(object sender, EventArgs e)
		{
			UpdateSerialPorts();
		}

		private void btn_serial_toggle_Click(object sender, EventArgs e)
		{
			string portname = combo_serial_ports.SelectedItem.ToString();

			Scanner scanner;

			try
			{
				scanner = new Scanner(portname);
			}
			catch(System.IO.IOException ex)
			{
				MessageBox.Show(
					string.Format("Could not open serial connection to {0}: {1}", portname, ex.Message),
					"Connection Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);

				return;
			}

			// MessageBox.Show(string.Format("Found {0} on firmware {1}", scanner.Model, scanner.Firmware));

			/*
			foreach (int i in scanner.CommandAsBytes("STS"))
			{
				tb_debug.AppendText(string.Format("{0}: ({1})\n", i.ToString(), (char)i));
			}*/


		}

		private void button1_Click(object sender, EventArgs e)
		{
			while (true)
			{
				lcd.Backlight = !lcd.Backlight;
				Application.DoEvents();
				Thread.Sleep(500);
			}
			
		}
	}
}
