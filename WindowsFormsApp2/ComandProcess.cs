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
        private string Direct { get; set; }
        private string Arguments { get; set; }
        private Process PsInfo { get; set; }
        CancellationToken cancelToken;
        private CancellationTokenSource TokenSource { get; set; }
        CancellationTokenSource cancelTokensource;//キャンセル判定用
  
        public async void Main(TextBox form)
        {


        }       
        public string CreateArguments(Controller controller)
        {
            Arguments = controller.Direct+" "+controller.Url+" "+controller.Username+" "+controller.Password;
            return Arguments;
        }
        public void ClosePsInfoCommand()
        {
            if (cancelTokensource != null)
            {
                cancelTokensource.Cancel();//キャンセルを発行
             
            }


        }
        public void HeavyMethod(CancellationToken token, TextBox form1)
        {
            //キャンセル用トークン作成
        }
    }
    
}
