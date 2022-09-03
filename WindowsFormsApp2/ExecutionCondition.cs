using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class ExecutionCondition
    {
        public int stdErr { get; set; }
        public int NpmExecution()
        {
            stdErr = 0;
            string directory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            directory = directory + @"\echo3.bat";
            string command = directory;
            ProcessStartInfo p = new ProcessStartInfo();
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.FileName = command;
            p.RedirectStandardError = true; // 標準出力をリダイレクト
            Process PsInfo = Process.Start(p);
            PsInfo.WaitForExit();
            stdErr=PsInfo.ExitCode;
            Console.WriteLine(stdErr);
            PsInfo.Close();
            return stdErr;
        }
        public int PackerExecution()
        {
            stdErr = 0;
            string directory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            directory = directory + @"\echo4.bat";
            string command = directory;
            ProcessStartInfo p = new ProcessStartInfo();
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.FileName = command;
            p.RedirectStandardError = true; // 標準出力をリダイレクト
            Process PsInfo = Process.Start(p);
            PsInfo.WaitForExit();
            stdErr = PsInfo.ExitCode;
            PsInfo.Close();
            return stdErr;
        }
        public int UploaderExecution()
        {
            stdErr = 0;
            string directory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            directory = directory + @"\echo5.bat";
            string command = directory;
            ProcessStartInfo p = new ProcessStartInfo();
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.FileName = command;
            p.RedirectStandardError = true; // 標準出力をリダイレクト
            Process PsInfo = Process.Start(p);
            PsInfo.WaitForExit();
            stdErr = PsInfo.ExitCode;
            Console.WriteLine(stdErr);
            PsInfo.Close();
            return stdErr;
        }
    }
}
