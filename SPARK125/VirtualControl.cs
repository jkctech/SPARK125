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
				{"Hold",	"Close Call"},
				{"1",		"Pri"},
				{"2",		""},
				{"3",		"Step"}
			},
			{
				{"Scan",	""},
				{"4",		"<"},
				{"5",		""},
				{"6",		">"}
			},
			{
				{"Srch",	"Svc"},
				{"7",		"Beep"},
				{"8",		""},
				{"9",		"Mod"}
			},
			{
				{"Lock-Out","Lock"},
				{"Enter",	"Program"},
				{"0",		""},
				{"•",		"Clear"}
			},
			{
				{"Power",	"Backlight"},
				{"←",       "(5x)"},
				{"→",       "(5x)"},
				{"Func",	""}
			}
		};

		public VirtualDisplay(Scanner scanner)
		{
			InitializeComponent();
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
			lcd.ParseSTS(this, "STS,011000, HOLD,,V67 SAR/KNRM/KWC,,CH028   156.3750,, FM,,BNK:1,,0,1,0,0,,,0,,0");

			// Screen syncer
			screensync = new BackgroundWorker();
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
			lbl_squelch.Left = tb_volume.Left + tb_volume.Width / 2 - lbl_volume.Width / 2 - 5;
			lbl_volume.Left = tb_squelch.Left + tb_squelch.Width / 2 - lbl_squelch.Width / 2 - 5;

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

			for (int y = 0; y < _grid_rows; y++)
			{
				for (int x = 0; x < _grid_cols; x++)
				{
					DualButton b = new DualButton(_btnmap_ubc125xlt[y,x,0], _btnmap_ubc125xlt[y, x, 1]);
					
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

		private void ScreenSync_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				lcd.ParseSTS(this, _scanner.Command("STS"));
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
	}
}
