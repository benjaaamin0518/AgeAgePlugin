using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class おじょじょじょじょぼじゅぼぼ : Form

    {
        private string Direct { get; set; }
        private string Arguments { get; set; }
        private FolderBrowserDialog Fbd { get; set; }
        private Properties.Settings Default { get; set; }
        private ComandProcess comandprocess { get; set; }
        private bool Flag { get; set; }
        private string output { get; set; }
        CancellationTokenSource tokenSource;
        CancellationToken cancelToken;

        public おじょじょじょじょぼじゅぼぼ()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            Fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            Fbd.Description = "フォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            Fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            Fbd.SelectedPath = @"C:\Windows";
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            Fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (Fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                Console.WriteLine(Fbd.SelectedPath);
                textBox5.Text = Fbd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "表示")
            {
                button2.Text = "非表示";
                // システムのパスワード文字を設定
                textBox4.UseSystemPasswordChar = false;
            }
            else
            {
                // システムのパスワード文字を設定
                textBox4.UseSystemPasswordChar = true;
                button2.Text = "表示";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Default.password = textBox4.Text;
            Default.username = textBox3.Text;
            Default.url = textBox2.Text;
            Default.directory = textBox5.Text;
            Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Default = Properties.Settings.Default;
            textBox2.Text = Default.url;
            textBox3.Text = Default.username;
            textBox4.Text = Default.password;
            textBox5.Text = Default.directory;

        }

        private async  void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "実行")
            {
                Controller Controller = new Controller();
                Controller.Direct = textBox5.Text;
                Controller.Url = textBox2.Text;
                Controller.Username = textBox3.Text;
                Controller.Password = textBox4.Text;
                comandprocess = new ComandProcess();
                Arguments = comandprocess.CreateArguments(Controller);
                button4.Text = "実行中";
                tokenSource = new CancellationTokenSource();
                cancelToken = tokenSource.Token;
                backgroundWorker1.RunWorkerAsync();

            }
            else
            {

                Flag = false;
                backgroundWorker1.CancelAsync();
                await ButtonUp();



            }
        }
        private Task<string> ButtonUp (){
            Task<string> task = Task.Run(() =>
            {
                while (true)
                {
                    if (Flag)
                    {
                        InvokeButton();
                        break;
                    }
                }
                return "Stop";
            });

            return task;       
}

        private  async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Direct = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            Direct = Direct + @"\echo.bat";
            string command = Direct;

            string arguments = Arguments;
            Console.WriteLine(arguments);
            ProcessStartInfo p = new ProcessStartInfo();
            p.Arguments = arguments;
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.FileName = command;
            BackgroundWorker worker = (BackgroundWorker)sender;
            p.RedirectStandardOutput = true; // 標準出力をリダイレクト
            Process PsInfo = Process.Start(p);
            output = "";
            Invoke(output);
            Task<string> task;
            while (!PsInfo.HasExited)
            {
                 task =Task.Run(async() =>
                {
                    return await Ho(PsInfo);
                });




                worker = (BackgroundWorker)sender;

                //キャンセル判定
                // senderの値はbgWorkerの値と同じ

                Console.WriteLine(worker.CancellationPending);
                // 時間のかかる処理
                // キャンセルされてないか定期的にチェック
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    InvokeButton2();

                    Stop(PsInfo); 
                    break;

                }


            }

            //PsInfo.Dispose();
            Console.WriteLine(worker.CancellationPending);

            Flag = worker.CancellationPending;

            //form1.Text += "end!";

        }
        public async Task<string> Ho(Process PsInfo)
        {
            if (cancelToken.IsCancellationRequested)
            {
                // キャンセルされたらTaskを終了する.
               
                return "Canceled";
            }
            output = PsInfo.StandardOutput.ReadLine();
            output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
            if (output != "") Invoke(output);

            return  output;
        }
        public  void CreOutput(Process PsInfo)
        {

        }
        public void Invoke(string output)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.UpdateText));
            }
            else
            {
            }

        }
        public string InvokeButton()
        {
            if (this.InvokeRequired)
            {

                    this.Invoke(new Action(this.ButtonUpdate));
                return "ok";
            }
            else
            {

                    return "out!";
           
            }

        }
        public void ButtonUpdate()
        {

            button4.Text = "実行";
            button4.Enabled = true;
        }
        public string InvokeButton2()
        {
            if (this.InvokeRequired)
            {
          
                    this.Invoke(new Action(this.ButtonUpdate2));
                    return "ok";
           
            }
            else
            {
                return "out"; 
            }

        }
        public void ButtonUpdate2()
        {
            //button4.Enabled = false;

            button4.Enabled = false;
            button4.Text = "終了中";
        }
        public void UpdateText()
        {
            textBox1.Text += output;
            //カレット位置を末尾に移動
            textBox1.SelectionStart = textBox1.Text.Length;
            //テキストボックスにフォーカスを移動
            textBox1.Focus();
            //カレット位置までスクロール
            textBox1.ScrollToCaret();
        }
        public  string Stop(Process PsInfo)
        {
     
                tokenSource.Cancel();



                PsInfo.Kill();
                PsInfo.WaitForExit();
                return "stop";
         
        }





    }
}