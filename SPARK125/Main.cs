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
	public partial class Spark125 : Form
	{
		public Spark125()
		{
			InitializeComponent();
			UpdateSerialPorts();
		}

		/// <summary>
		/// Get all available COM ports and list them in the dropdown
		/// </summary>
		private void UpdateSerialPorts()
		{
			// Wipe and add all ports
			combo_serial_ports.Items.Clear();
			combo_serial_ports.Items.AddRange(SerialPort.GetPortNames());

			// Auto select last port
			combo_serial_ports.SelectedIndex = combo_serial_ports.Items.Count - 1;
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
					string.Format(Strings.Error_SerialConnection, portname, ex.Message),
					Strings.Error_Connection,
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
			VirtualDisplay vc = new VirtualDisplay();
			vc.Show();
		}
	}
}
