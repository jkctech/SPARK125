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
		Scanner scanner;
		VirtualDisplay vc;

		public Spark125()
		{
			InitializeComponent();

			// Update serial ports
			UpdateSerialPorts();

			// Lock sizing
			MaximumSize = Size;
			MinimumSize = Size;
			Text = Properties.Resources.Title;
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
			// Disconnect
			if (scanner != null && scanner.IsReady())
			{
				if (vc != null)
					vc.Close();
				Text = Properties.Resources.Title;
				btn_serial_toggle.Text = "Connect";
				scanner.Close();
			}

			// Connect
			else
			{
				string portname = combo_serial_ports.SelectedItem.ToString();

				try
				{
					scanner = new Scanner(portname);

					Text = string.Format("{2} - {0} {1}", scanner.Model, scanner.Firmware, Properties.Resources.Title);
				}
				catch (System.IO.IOException ex)
				{
					MessageBox.Show(
						string.Format(Strings.Error_SerialConnection, portname, ex.Message),
						Strings.Error_Connection,
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);

					return;
				}

				btn_serial_toggle.Text = "Disconnect";
			}

			btn_VirtualControl.Enabled = scanner.IsReady();
		}

		private void btn_VirtualControl_Click(object sender, EventArgs e)
		{
			if (vc == null)
			{
				vc = new VirtualDisplay(scanner);
				vc.Show();
			}
			vc.FormClosed += Vc_FormClosed;
		}

		private void Vc_FormClosed(object sender, FormClosedEventArgs e)
		{
			vc = null;
		}

		private void btn_serial_auto_Click(object sender, EventArgs e)
		{

		}
	}
}
