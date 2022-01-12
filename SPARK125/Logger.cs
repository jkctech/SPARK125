using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPARK125
{
    public class Logger
    {
        private RichTextBox _box;
        public enum Type
        {
            DEBUG,
            INFO,
            SUCCESS,
            WARNING,
            ERROR,
            RECEPTION
        };

        public Logger(RichTextBox box)
		{
            _box = box;
		}

        public void Log(string Text, Type Type)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            Color c = Color.White;

            if (Type == Type.DEBUG)
                c = Color.Magenta;
            else if (Type == Type.INFO)
                c = Color.DeepSkyBlue;
            else if (Type == Type.SUCCESS)
                c = Color.Lime;
            else if (Type == Type.WARNING)
                c = Color.Yellow;
            else if (Type == Type.ERROR)
                c = Color.Red;
            else if (Type == Type.RECEPTION)
                c = Color.Orange;

            if (_box.InvokeRequired)
			{
                _box.Invoke(new Action(() => {
                    _box.AppendText(string.Format("[{0}] ", time), Color.Gray);
                    _box.AppendText(string.Format("[{0}] ", Type.ToString()), c);
                    _box.AppendText(Text + "\n", Color.White);
                    _box.ScrollToCaret();
                }));
            }
			else
			{
                _box.AppendText(string.Format("[{0}] ", time), Color.Gray);
                _box.AppendText(string.Format("[{0}] ", Type.ToString()), c);
                _box.AppendText(Text + "\n", Color.White);
                _box.ScrollToCaret();
            }
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
