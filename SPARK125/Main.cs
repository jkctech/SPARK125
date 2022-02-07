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
		BankEditor be;
		public Logger logger;

		public Spark125()
		{
			InitializeComponent();

			logger = new Logger(tb_debug);

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
				logger.Log("Disconnected from scanner.", Logger.Type.INFO);
			}

			// Connect
			else
			{
				string portname = combo_serial_ports.SelectedItem.ToString();

				try
				{
					logger.Log(string.Format("Connecting to port {0}...", portname), Logger.Type.INFO);
					scanner = new Scanner(portname);
					Text = string.Format("{0} - {1}", Properties.Resources.Title, scanner.Model);
					logger.Log(string.Format("Connected to {0} [{2}] on port {1}.", scanner.Model, portname, scanner.Firmware), Logger.Type.SUCCESS);
				}
				catch (System.IO.IOException ex)
				{
					logger.Log(string.Format(Properties.Resources.Error_SerialConnection, portname, ex.Message), Logger.Type.ERROR);
					return;
				}
				catch (TimeoutException)
				{
					logger.Log(string.Format("Port {0} does not seem to be a compatible scanner!", portname), Logger.Type.ERROR);
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
				vc = new VirtualDisplay(scanner, this);
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
			throw new NotImplementedException();
		}

		private void btn_clear_Click(object sender, EventArgs e)
		{
			tb_debug.Text = "";
		}

		private void btn_BankEditor_Click(object sender, EventArgs e)
		{
			if (be == null)
			{
				be = new BankEditor(this);
				be.Show();
			}
			be.FormClosed += Be_FormClosed;
		}

		private void Be_FormClosed(object sender, FormClosedEventArgs e)
		{
			be = null;
		}
	}
}
