using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace SPARK125
{
	public partial class VirtualDisplay : Form
	{
		LCD lcd;
		BackgroundWorker screensync;
		Spark125 main;

		private const int _padding = 10;
		private const int _spacing = 5;
		private const int _btnspacing = 2;
		private int _btnwidth;
		private int _btnheight;
		private const int _grid_cols = 4;
		private const int _grid_rows = 5;
		private const int _sliderheight = 200;
		private int _screenonly;
		private Size _lcdSize;
		private Size _size;
		private Scanner _scanner;

		private string[,,] _btnmap_ubc125xlt = {
			{
				{"Hold",	"Close Call",	"H"},
				{"1",		"Pri",			"1"},
				{"2",		"",				"2"},
				{"3",		"Step",			"3"}
			},
			{
				{"Scan",	"",				"S"},
				{"4",		"<",			"4"},
				{"5",		"",				"5"},
				{"6",		">",			"6"}
			},
			{
				{"Srch",	"Svc",			"R"},
				{"7",		"Beep",			"7"},
				{"8",		"",				"8"},
				{"9",		"Mod",			"9"}
			},
			{
				{"Lock-Out","Lock",			"L"},
				{"Enter",	"Program",		"E"},
				{"0",		"",				"0"},
				{"•",		"Clear",		"."}
			},
			{
				{"Power",	"Backlight",	"P"},
				{"←",       "",				"<"},
				{"→",       "",				">"},
				{"Func",	"",				"F"}
			}
		};

		public VirtualDisplay(Scanner scanner, Spark125 mainWindow)
		{
			InitializeComponent();

			FormClosing += VirtualDisplay_FormClosing;

			main = mainWindow;

			int ybase;
			int ybase_grid;

			// Attach scanner
			_scanner = scanner;

			// Initialize LCD
			lcd = new LCD(
				Controls,
				new Point(_padding, _padding),
				24,
				35,
				26
			);

			// Parse testing STS string
			//lcd.ParseSTS("STS,011000, HOLD,,V67 SAR/KNRM/KWC,,CH028   156.3750,, FM,,BNK:1,,0,1,0,0,,,0,,0");

			// Screen syncer
			screensync = new BackgroundWorker();
			screensync.WorkerSupportsCancellation = true;
			screensync.DoWork += ScreenSync_DoWork;
			screensync.RunWorkerAsync();

			// Add LCD size + Padding
			_lcdSize = lcd.GetSize();
			Size = _lcdSize;
			Width += Width - ClientSize.Width + _padding * 2;
			Height += Height - ClientSize.Height + _padding * 2;
			ybase = _lcdSize.Height + _padding + _spacing;

			// Set checkbox locations
			cb_screenonly.Location = new Point(
				_padding,
				ybase
			);
			cb_always_on_top.Location = new Point(
				Width - cb_always_on_top.Width - _padding * 2,
				ybase
			);
			ybase += cb_always_on_top.Height + _padding;
			_screenonly = ybase + (Height - ClientSize.Height);
			ybase_grid = ybase;

			// Slider labels Y
			lbl_volume.Top = ybase;
			lbl_squelch.Top = ybase;
			ybase += _spacing + lbl_squelch.Height;

			// Volume slider
			tb_volume.Left = ClientSize.Width - tb_volume.Width;
			tb_volume.Top = ybase;
			tb_volume.Height = _sliderheight;

			// Squelch slider
			tb_squelch.Left = tb_volume.Left - tb_squelch.Width;
			tb_squelch.Top = ybase;
			tb_squelch.Height = _sliderheight;
			ybase += tb_squelch.Height + _spacing;

			// Slider labels X
			lbl_volume.Left = tb_volume.Left + tb_volume.Width / 2 - lbl_volume.Width / 2 - 5;
			lbl_squelch.Left = tb_squelch.Left + tb_squelch.Width / 2 - lbl_squelch.Width / 2 - 5;

			// Lock & save size
			Height = ybase + Height - ClientSize.Height;
			_size = Size;
			MaximizeBox = false;
			MaximumSize = Size;
			MinimumSize = Size;

			// Build button grid
			int _gridwidth = tb_squelch.Left - _padding * 2;
			int _gridheight = ClientSize.Height - ybase_grid - _padding + _btnspacing;

			_btnwidth = (_gridwidth - ((_grid_cols - 1) * _btnspacing)) / _grid_cols;
			_btnheight = (_gridheight - ((_grid_rows - 1) * _btnspacing)) / _grid_rows;

			// Add top button first
			{
				DualButton b = new DualButton("Click", "");
				b.Click += B_Click;
				b.Key = "^";
				SolidBrush tb = new SolidBrush(Color.Black);
				SolidBrush bb = new SolidBrush(Color.Red);
				b.Size = new Size(_btnwidth, (int)(_btnheight * 0.75)); // CHANGED
				b.Left = _padding + 3 * (_btnwidth + _btnspacing);
				b.Top = ybase_grid - _btnspacing - (int)(_btnheight * 0.75);
				b.BackColor = Color.DarkGray;
				tb.Color = Color.White;
				b.TopBrush = tb;
				b.BottomBrush = bb;
				Controls.Add(b);
			}

			for (int y = 0; y < _grid_rows; y++)
			{
				for (int x = 0; x < _grid_cols; x++)
				{
					DualButton b = new DualButton(_btnmap_ubc125xlt[y,x,0], _btnmap_ubc125xlt[y, x, 1]);

					// Add click event
					b.Click += B_Click;
					b.Key = _btnmap_ubc125xlt[y, x, 2];

					SolidBrush tb = new SolidBrush(Color.Black);
					SolidBrush bb = new SolidBrush(Color.Red);
					
					b.Size = new Size(_btnwidth, _btnheight);
					b.Left = _padding + x * (_btnwidth + _btnspacing);
					b.Top = ybase_grid + y * (_btnheight + _btnspacing);

					// Coloring
					if (x == 0 || (x == 1 && y == 3))
					{
						b.BackColor = Color.Black;
						tb.Color = Color.White;
					}
					else if (x == 3 && y == 4)
					{
						b.BackColor = Color.Orange;
					}
					else if (y == 4 && (x == 1 || x == 2))
					{
						b.BackColor = Color.DarkGray;
					}
					else
					{
						b.BackColor = Color.White;
					}

					b.TopBrush = tb;
					b.BottomBrush = bb;

					Controls.Add(b);
				}
			}
		}

		private void B_Click(object sender, EventArgs e)
		{
			DualButton btn = sender as DualButton;
			_scanner.Command(string.Format("KEY,{0},P", btn.Key));
		}

		private void VirtualDisplay_FormClosing(object sender, FormClosingEventArgs e)
		{
			screensync.CancelAsync();
		}

		private void ScreenSync_DoWork(object sender, DoWorkEventArgs e)
		{
			bool inReception = false;

			// Read in volume & Squelch
			tb_volume.AutoInvoke(() => tb_volume.Value = int.Parse(_scanner.Command("VOL").Substring(4)));
			tb_squelch.AutoInvoke(() => tb_squelch.Value = int.Parse(_scanner.Command("SQL").Substring(4)));

			while (true)
			{
				// Read screen and save buffer for other triggers
				string[] sts = lcd.ParseSTS(_scanner.CommandAsBytes("STS"));
				Thread.Sleep(50);

				//foreach (string str in sts) {
				//	Debug.Write(str + ",");
				//}
				//Debug.WriteLine("");

				// Check triggers
				try
				{
					// Volume & Squelch
					string vs = sts[(int)LCD.BufferElement.R4];
					if (vs.Length >= 6 && vs.Substring(0, 6) == "VOLUME")
						tb_volume.AutoInvoke(() => tb_volume.Value = int.Parse(_scanner.Command("VOL").Substring(4)));
					if (vs.Length >= 7 && vs.Substring(0, 7) == "SQUELCH")
						tb_squelch.AutoInvoke(() => tb_squelch.Value = int.Parse(_scanner.Command("SQL").Substring(4)));

					// Reception detector
					int pow = int.Parse(sts[(int)LCD.BufferElement.ReceptionPower]);
					if (pow == 0)
						inReception = false;
					if (!inReception && pow > 0)
					{
						inReception = true;
						string name = sts[(int)LCD.BufferElement.R1].Trim();
						string freq = sts[(int)LCD.BufferElement.R2].Substring(7);
						freq = freq.Remove(freq.Length - 1);
						string chan = sts[(int)LCD.BufferElement.R2].Split(' ')[0].Substring(2);
						string mod = sts[(int)LCD.BufferElement.R3].Substring(1, 2);

						main.logger.Log(string.Format("{1} Mhz [{3}] \"{0}\"  Channel {2} ", name, freq, chan, mod), Logger.Type.RECEPTION);
					}

				} catch (Exception) { }

				if (screensync.CancellationPending)
					return;
				
				Thread.Sleep(100);
			}
		}

		private void cb_always_on_top_CheckedChanged(object sender, EventArgs e)
		{
			TopMost = cb_always_on_top.Checked;
		}

		private void cb_screenonly_CheckedChanged(object sender, EventArgs e)
		{
			Size s = new Size(
				_size.Width,
				cb_screenonly.Checked ? _screenonly : _size.Height
			);
			
			Size = s;
			MinimumSize = s;
			MaximumSize = s;
		}

		private void tb_volume_ValueChanged(object sender, EventArgs e)
		{
			_scanner.Command("VOL," + tb_volume.Value.ToString());
		}

		private void tb_squelch_ValueChanged(object sender, EventArgs e)
		{
			_scanner.Command("SQL," + tb_squelch.Value.ToString());
		}
	}
}
