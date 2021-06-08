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

			lcd = new LCD(
				Controls,
				new Point(200, 10),
				15,
				30,
				15
			);

			lcd.ParseSTS("STS,011000,                ,,BANK9           ,,         SCAN  ⸮,,                ,,                ,,⸮⸮⸮ 2       0   ,,0,1,0,0,,,0,,0");
			
			
		}

		public void UpdateSerialPorts()
		{
			string[] ports = SerialPort.GetPortNames();

			combo_serial_ports.Items.Clear();

			foreach (string port in ports)
			{
				combo_serial_ports.Items.Add(port);
			}

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
	}
}
