using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPARK125
{
	public static class Extensions
	{
        public static void AutoInvoke(this System.ComponentModel.ISynchronizeInvoke self, Action action)
        {
            if (self == null) throw new ArgumentNullException("self");
            if (action == null) throw new ArgumentNullException("action");

            if (self.InvokeRequired)
            {
                self.Invoke(action, null);
            }
            else
            {
                action();
            }
        }
    }
}
