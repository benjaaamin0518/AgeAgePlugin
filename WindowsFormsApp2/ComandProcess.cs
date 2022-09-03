using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class ComandProcess
    {
        private string Arguments { get; set; }
  
        public string CreateArguments(Controller controller)
        {
            Arguments = controller.Direct+" "+controller.Url+" "+controller.Username+" "+controller.Password;
            return Arguments;
        }
    }
}
