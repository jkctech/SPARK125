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
		private const int _rows = 6;
		
		private Color _color_back_dim = SystemColors.Control;
		private Color _color_back_light = Color.Orange;
		private Color _color_back_invert = Color.Black;

		private Color _color_fore_normal = Color.Black;
		private Color _color_fore_invert_light = Color.Orange;
		private Color _color_fore_invert_dim = SystemColors.Control;

		/// <summary>
		/// Array indexes for specific areas in the STS buffer
		/// </summary>
		public enum BufferElement
		{
			Command=0,
			LayoutMode,
			R0, R0M,
			R1, R1M,
			R2, R2M,
			R3, R3M,
			R4, R4M,
			R5, R5M,
			Unknown3,
			Unknown4,
			Unknown5,
			Unknown6,
			ReceptionPower,
			Unknown7,
			Unknown8,
			Unknown9,
			Backlight
		}

		public Dictionary<int, string> UnidenDict = new Dictionary<int, string>()
		{
			{128, "█"},
			{129, "↑"},
			{130, "↓"},
			{133, "╳"},
			{134, ""},
			{135, "C"},
			{136, "C"},
			{139, "F"},
			{140, "P"},
			{141, "H"},
			{142, "O"},
			{143, "L"},
			{144, "D"},
			{145, "+"},
			{146, "C"},
			{147, "T"},
			{148, "L"},
			{149, "L"},
			{150, "/"},
			{151, "O"},
			{152, ""},
			{153, "A"},
			{154, "M"},
			{155, ""},
			{156, "F"},
			{161, "P"},
			{162, "‼"},
			{166, "-"},
			{167, "+"},
			{168, "-"},
			{169, "+"},
			{170, "+"},
			{171, "+"},
			{172, "X"},
			{173, "X"},
			{177, "["},
			{178, "_"},
			{179, "]"},
			{181, "C"},
			{182, "C"},
			{205, "B"},
			{206, "N"},
			{207, "K"},
			{216, "P"},
			{217, "R"},
			{218, "I"}
		};

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

			// Loop over total grid
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

					l.Width = _width;
					l.Height = _height;

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

		public Size GetSize()
		{
			return new Size(_box.Width, _box.Height);
		}

		public Point GetLocation()
		{
			return new Point(_startx, _starty);
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
		/// Parse Uniden characters to normal equivalent.
		/// </summary>
		/// <param name="raw">Raw STS bytestring</param>
		private string ParseUnidenChars(List<int> raw)
		{
			string result = "";

			foreach (int b in raw)
			{
				if (UnidenDict.ContainsKey(b))
					result += UnidenDict[b];
				else
					result += (char)b;
			}

			return result;
		}

		public string[] ParseSTS(string raw)
		{
			string[] parts = raw.Split(',');

			try
			{
				PutString(parts[(int)BufferElement.R0], 0);
				PutString(parts[(int)BufferElement.R1], 1);
				PutString(parts[(int)BufferElement.R2], 2);
				PutString(parts[(int)BufferElement.R3], 3);
				PutString(parts[(int)BufferElement.R4], 4);
				PutString(parts[(int)BufferElement.R5], 5);

				Backlight = parts[(int)BufferElement.Backlight] == "3";
			}
			catch (Exception) { }
			Debug.WriteLine(raw);
			return parts;
		}

		public string[] ParseSTS(List<int> raw)
		{
			return ParseSTS(ParseUnidenChars(raw));
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
			Label field;

			if (_grid[0, 0].InvokeRequired)
			{
				// Copy over string char by char
				for (x = 0; col + x < str.Length; x++)
				{
					field = _grid[row, col + x];
					field.AutoInvoke(() => field.Text = str[x].ToString());
				}

				// Fill to end with empty spaces
				if (filltoend)
				{
					while (col + x < _cols)
					{
						field = _grid[row, col + x];
						field.AutoInvoke(() => field.Text = "");
						x++;
					}
				}
			}
			else
			{
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
}
