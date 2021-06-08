using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace SPARK125
{
	class LCD
	{
		// Private
		private Label[,] _grid = new Label[_rows, _cols];
		private GroupBox _box = new GroupBox();
		
		private int _startx;
		private int _starty;
		private int _width;
		private int _height;
		private int _fontsize;
		private int _paddingx = 5;
		private int _paddingy = 10;

		private const int _cols = 16;
		private const int _rows = 4;
		
		private Color _color_back_dim = SystemColors.Control;
		private Color _color_back_light = Color.Orange;
		private Color _color_back_invert = Color.Black;

		private Color _color_fore_normal = Color.Black;
		private Color _color_fore_invert_light = Color.Orange;
		private Color _color_fore_invert_dim = SystemColors.Control;

		private enum BufferElement
		{
			Command=0,
			LayoutMode,
			R0,
			R0M,
			R1,
			R1M,
			R2,
			R2M,
			R3,
			R3M
		}

		// Public
		public bool Backlight = true;

		public LCD(Control.ControlCollection control, Point start, int width, int height, int fontsize)
		{
			// Copy over values to class instance
			_startx = start.X;
			_starty = start.Y;
			_width = width;
			_height = height;
			_fontsize = fontsize;

			// Default font
			Font font = new Font(FontFamily.GenericMonospace, _fontsize);

			// Put in groupbox
			_box.Location = start;
			_box.SetBounds(start.X, start.Y, _cols * _width + _paddingx * 2, _rows * _height + _paddingy * 2);
			control.Add(_box);

			// Loop over total grid (16 x 4)
			for (int y = 0; y < _rows; y++)
			{
				for (int x = 0; x < _cols; x++)
				{
					Label l = new Label();
					
					l.Location = new Point(
						_paddingx + (x * _width),
						_paddingy + (y * _height)
					);
					
					l.Width = width;
					l.Height = height;

					// l.BackColor = _color_back_dim;
					l.BackColor = Color.Transparent;
					l.ForeColor = _color_fore_normal;

					l.Font = font;
					l.TextAlign = ContentAlignment.MiddleCenter;

					_grid[y, x] = l;
					_box.Controls.Add(l);
				}
			}
		}

		public void Update()
		{
			if (Backlight)
				_box.BackColor = _color_back_light;
			else
				_box.BackColor = _color_back_dim;
			return;
			for (int y = 0; y < _rows; y++)
			{
				for (int x = 0; x < _cols; x++)
				{
					if (Backlight)
						_grid[y, x].BackColor = _color_back_light;
					else
						_grid[y, x].BackColor = _color_back_dim;
				}
			}
		}

		public void ParseSTS(string raw)
		{
			string[] parts = raw.Split(',');

			PutString(parts[(int)BufferElement.R0], 0);
			PutString(parts[(int)BufferElement.R1], 1);
			PutString(parts[(int)BufferElement.R2], 2);
			PutString(parts[(int)BufferElement.R3], 3);
		}

		public void PutString(string str, int row, int col = 0, bool filltoend = true)
		{
			int x = 0;
			
			for (x = 0; col + x < str.Length; x++)
				_grid[row, col + x].Text = str[x].ToString();

			while (col + x < _cols)
			{
				_grid[row, col + x].Text = "";
				x++;
			}

			Update();
		}
	}
}
