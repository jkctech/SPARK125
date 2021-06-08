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
		private PictureBox _box;
		
		private bool _debug = false;
		private bool _backlight;

		private int _startx;
		private int _starty;
		private int _width;
		private int _height;
		private int _fontsize;
		
		private const int _paddingx = 5;
		private const int _paddingy = 10;

		private const int _cols = 16;
		private const int _rows = 5;
		
		private Color _color_back_dim = SystemColors.Control;
		private Color _color_back_light = Color.Orange;
		private Color _color_back_invert = Color.Black;

		private Color _color_fore_normal = Color.Black;
		private Color _color_fore_invert_light = Color.Orange;
		private Color _color_fore_invert_dim = SystemColors.Control;

		/// <summary>
		/// Array indexes for specific areas in the STS buffer
		/// </summary>
		private enum BufferElement
		{
			Command=0,
			LayoutMode,
			R0, R0M,
			R1, R1M,
			R2, R2M,
			R3, R3M,
			R4, R4M,
			Unknown1,
			Unknown2,
			Unknown3,
			Unknown4,
			Unknown5,
			Unknown6,
			ReceptionPower,
			Unknown7,
			Backlight
		}

		/// <summary>
		/// Create and place new virtual LCD in form
		/// </summary>
		/// <param name="control">Form control object</param>
		/// <param name="location">Where to place the virtual LCD</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		/// <param name="fontsize">Font size (In pixels)</param>
		public LCD(Control.ControlCollection control, Point location, int width, int height, int fontsize)
		{
			// Copy over values to class instance
			_startx = location.X;
			_starty = location.Y;
			_width = width;
			_height = height;
			_fontsize = fontsize;

			// Default font
			Font font = new Font(FontFamily.GenericMonospace, _fontsize);

			// Loop over total grid (
			for (int y = 0; y < _rows; y++)
			{
				for (int x = 0; x < _cols; x++)
				{
					Label l = new Label();
					
					// Build label properties
					l.Location = new Point(
						_startx + _paddingx + (x * _width),
						_starty + _paddingy + (y * _height)
					);
					
					l.Width = width;
					l.Height = height;

					l.BackColor = _color_back_dim;
					l.ForeColor = _color_fore_normal;

					l.Font = font;
					l.TextAlign = ContentAlignment.BottomCenter;

					// Link label to grid and add to control
					_grid[y, x] = l;
					control.Add(l);
				}
			}

			// LCD Background picturebox
			_box = new PictureBox();
			_box.Location = location;
			_box.Width = _cols * _width + _paddingx * 2;
			_box.Height = _rows * _height + _paddingy * 2;
			_box.BorderStyle = BorderStyle.Fixed3D;
			_box.SendToBack();
			control.Add(_box);
		}

		/// <summary>
		/// Get / Set backlight status
		/// </summary>
		public bool Backlight
		{
			get
			{
				return _backlight;
			}
			set
			{
				if (_backlight != value)
				{
					// Set pictureboc backlight
					if (value)
						_box.BackColor = _color_back_light;
					else
						_box.BackColor = _color_back_dim;

					// Per label mode background
					for (int y = 0; y < _rows; y++)
					{
						for (int x = 0; x < _cols; x++)
						{
							// Debugging mode
							if (_debug)
							{
								_grid[y, x].ForeColor = Color.White;
								if (y % 2 == 0)
									_grid[y, x].BackColor = x % 2 == 0 ? Color.Magenta : Color.Black;
								else
									_grid[y, x].BackColor = x % 2 == 0 ? Color.Black : Color.Magenta;
							}

							// Normal backlight update
							else
							{
								_grid[y, x].BackColor = value ? _color_back_light : _color_back_dim;
							}
						}
					}

					_backlight = value;
				}
			}
		}

		/// <summary>
		/// Parse STS string directly to LCD
		/// </summary>
		/// <param name="raw">Raw STS bytestring</param>
		public void ParseSTS(string raw)
		{
			string[] parts = raw.Split(',');

			PutString(parts[(int)BufferElement.R0], 0);
			PutString(parts[(int)BufferElement.R1], 1);
			PutString(parts[(int)BufferElement.R2], 2);
			PutString(parts[(int)BufferElement.R3], 3);
			PutString(parts[(int)BufferElement.R4], 4);

			Backlight = parts[(int)BufferElement.Backlight] == "3";
		}

		/// <summary>
		/// Put string onto LCD on given cursor location
		/// Overflowing characters will be truncated.
		/// </summary>
		/// <param name="str">String to place</param>
		/// <param name="row">Row</param>
		/// <param name="col">Col</param>
		/// <param name="filltoend">Fill line to end with spaces if needed</param>
		public void PutString(string str, int row, int col = 0, bool filltoend = true)
		{
			int x;
			
			// Copy over string char by char
			for (x = 0; col + x < str.Length; x++)
				_grid[row, col + x].Text = str[x].ToString();

			// Fill to end with empty spaces
			if (filltoend)
			{
				while (col + x < _cols)
				{
					_grid[row, col + x].Text = "";
					x++;
				}
			}
		}
	}
}
