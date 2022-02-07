using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPARK125
{
	public partial class BankEditor : Form
	{
		TabControl banktabs;
		DataGridView[] banks = new DataGridView[10];

		public BankEditor(Spark125 mainWindow)
		{
			InitializeComponent();

			banktabs = _CreateBankTabs();
			Controls.Add(banktabs);
			Resize += BankEditor_Resize;
		}

		private void BankEditor_Resize(object sender, EventArgs e)
		{
			banktabs.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 20);
		}

		private TabControl _CreateBankTabs()
		{
			TabControl ctrl = new TabControl();

			ctrl.Location = new Point(10, 10);
			ctrl.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 20);

			for (int i = 0; i < 10; i++)
			{
				TabPage p = new TabPage(string.Format("Bank {0}", i));

				banks[i] = new DataGridView();
				banks[i].ColumnCount = 8;
				banks[i].RowCount = 51;

				banks[i].Size = ctrl.ClientSize;

				p.Controls.Add(banks[i]);

				ctrl.TabPages.Add(p);
			}

			return ctrl;
		}
	}
}
