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

namespace AgeAgePlugin
{
    public class ComandProcess
    {
        private string Arguments { get; set; }
  
        public string CreateUploaderArguments(Controller controller)
        {
            Arguments = controller.Direct+" "+controller.Url+" "+controller.Username+" "+controller.Password;
            return Arguments;
        }
        public string CreatePackerArguments(Controller controller)
        {
            Arguments = controller.Direct + " " + "src";
            return Arguments;
        }
    }
}
