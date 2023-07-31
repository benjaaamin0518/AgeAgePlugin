using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace AgeAgePlugin
{
    public partial class MainForm : Form
    {
        private Process process { get; set; }
        private Process process2 { get; set; }
        private Process process3 { get; set; }
        public ManifestVisibleForm form3 { get; set; }
        private string Direct { get; set; }
        private int errorLevel { get; set; }
        private int errorLevel2 { get; set; }
        private int errorLevel3 { get; set; }
        private decimal beforeVersion { get; set; }
        private string Direct2 { get; set; }
        private string Direct3 { get; set; }
        public static FormData PluginSet { get; set; }
        public static Customize PluginSetCustomize { get; set; }
        public static ManifestJsonData Json { get; set; }
        public static CustomizeJsonData CustomizeJson { get; set; }
        private string UploaderArguments { get; set; }
        private string PackerArguments { get; set; }
        private string PackerPpkArguments { get; set; }
        private FolderBrowserDialog Fbd { get; set; }
        private Properties.Settings Default { get; set; }
        private ComandProcess comandprocess { get; set; }
        private Process PsInfo { get; set; }
        private Process PsInfo2 { get; set; }
        private Process PsInfo3 { get; set; }
        private static bool Flag { get; set; }
        private static bool Flag2 { get; set; }
        private static bool Flag3 { get; set; }
        private string output { get; set; }
        private decimal PlusVersion { get; set; }
        CancellationTokenSource tokenSource;
        CancellationToken cancelToken;
        CancellationTokenSource tokenSource2;
        CancellationToken cancelToken2;
        CancellationTokenSource tokenSource3;
        CancellationToken cancelToken3;
        public List<FormData> test { get; set; }
        public List<FormData> fd { get; set; }
        public string Error { get; set; }
        public string Error2 { get; set; }
        public string Error3 { get; set; }
        public bool isformEnabled { get; set; }

        public MainForm()
        {
            InitializeComponent();
            test = new List<FormData>();
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
                GetManifestVersion(true);
                if (textBox5.Text != "" && PluginSet.plusBool)
                {
                    button8.Enabled = true;
                }
                if (textBox5.Text != "")
                {
                    button9.Enabled = true;
                }
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
            Default.fd.RemoveAll(x => x.name == comboBox1.Text);
            Default.fd.Add(new FormData { url = textBox2.Text, username = textBox3.Text, password = textBox4.Text, Direct = textBox5.Text, name = comboBox1.Text, ppk = textBox6.Text });
            Default.Save();
            Console.WriteLine(textBox5.Text);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!AssemblyState.IsDebug)
            {
                LoadExecution();
            }
            Default = Properties.Settings.Default;
            CreatePluginList();
            LoadPluginListValue();
            comboBox1.SelectedItem = Default.PluginName;
            CreateCustomizeList();
            LoadCustomizeListValue();
            comboBox2.SelectedItem = Default.CustomizeName;
        }
        private void LoadExecution()
        {
            InstallConfirmationForm installConfirmationForm = new InstallConfirmationForm();
            installConfirmationForm.Show();
            installConfirmationForm.parsent = 0;
            installConfirmationForm.InstallProgress("npmのインストールを確認しています");
            ExecutionCondition executionCondition = new ExecutionCondition();
            string npmErr = (executionCondition.NpmExecution() != 9009) ? "" : "・npmがインストールされていません\n\n";
            installConfirmationForm.parsent = 20;
            installConfirmationForm.InstallProgress("create-pluginのインストールを確認しています");
            string createErr = (executionCondition.CreateExecution() != 9009) ? "" : "・create-pluginがインストールされていません\n\n";
            installConfirmationForm.parsent = 40;
            installConfirmationForm.InstallProgress("kintone-plugin-uploaderのインストールを確認しています");
            string uploderErr = (executionCondition.UploaderExecution() != 9009) ? "" : "・kintone-plugin-uploaderがインストールされていません\n\n";
            installConfirmationForm.parsent = 60;
            installConfirmationForm.InstallProgress("kintone-plugin-packerのインストールを確認しています");
            string PackerErr = (executionCondition.PackerExecution() != 9009) ? "" : "・kintone-plugin-packerがインストールされていません\n\n";
            installConfirmationForm.parsent = 80;
            installConfirmationForm.InstallProgress("kintone-customize-uploaderのインストールを確認しています");
            string CustomizeErr = (executionCondition.CustomizeExecution() != 9009) ? "" : "・kintone-customize-uploaderがインストールされていません";
            installConfirmationForm.parsent = 100;
            installConfirmationForm.InstallProgress("ソフトを起動しています...");
            if (npmErr != "" || uploderErr != "" || PackerErr != "" || CustomizeErr!="")
            {
                MessageBox.Show(npmErr + createErr + uploderErr + PackerErr + CustomizeErr,
                                "インストールしてください",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            installConfirmationForm.Close();
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "実行" && comboBox1.Enabled==true)
            {
                if (!button8.Enabled) { MessageBox.Show("manifest.jsonが読み込まれていません"); return; }
                beforeVersion = Json.version;
                Error = "";
                Error2 = "";
                errorLevel = 0;
                errorLevel2 = 0;
                Flag = false;
                Flag2 = false;
                button3.Enabled = false;
                comboBox1.Enabled = false;
                button7.Enabled = false;
                textBox1.Text = (checkBox2.Checked) ? "" : textBox1.Text;
                Controller Controller = new Controller();
                Controller.Direct = textBox5.Text;
                Controller.Url = textBox2.Text;
                Controller.Username = textBox3.Text;
                Controller.Password = textBox4.Text;
                Controller.ppk = textBox6.Text;
                comandprocess = new ComandProcess();
                UploaderArguments = comandprocess.CreateUploaderArguments(Controller);
                PackerPpkArguments = comandprocess.CreatePackerPpkArguments(Controller);
                PackerArguments = comandprocess.CreatePackerArguments(Controller);
                button4.Text = "実行を終了";
                tokenSource = new CancellationTokenSource();
                cancelToken = tokenSource.Token;
                tokenSource2 = new CancellationTokenSource();
                cancelToken2 = tokenSource2.Token;
                SaveManifestJson(true);
                backgroundWorker2.RunWorkerAsync();
                backgroundWorker1.RunWorkerAsync();
                output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] -----プラグインアップロード開始-----" + "\r\n";
                Invoke(output);
            }
            else
            {
               DialogResult message= MessageBox.Show("実行を終了してもよろしいですか?",
                                "終了の確認",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning);
                if (message==DialogResult.OK)
                {
                    Flag = (Flag) ? Flag : false;
                    Flag2 = (Flag2) ? Flag2 : false;
                    if (!Flag2) { backgroundWorker2.CancelAsync(); }
                    if (!Flag) { backgroundWorker1.CancelAsync(); }
                    await ButtonUp();
                }
            }
        }
        private Task<string> ButtonUp()
        {
            Task<string> task = Task.Run(() =>
            {
                while (true)
                {
                    InvokeButton2();
                    try { Flag = process.HasExited; }
                    catch { Flag = true; };
                    try { Flag2 = process2.HasExited; }
                    catch { Flag2 = true; };
                    Console.WriteLine(Flag);
                    Console.WriteLine("Flag2"+Flag2);
                    if ((Flag && Flag2))
                    {
                        InvokeButton();
                        break;
                    }
                }
                try
                {
                    isformEnabled = true;
                    this.FormEnabled();
                    process.Dispose();
                    process2.Dispose();
                }
                catch { };
                return "Stop";
            });
            output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] -----プラグインアップロード終了-----" + "\r\n";
            Invoke(output);
            return task;
        }
        private Task<string> CustomizeButtonUp()
        {
            Task<string> task = Task.Run(() =>
            {
                while (true)
                {
                    CsInvokeButton();
                    try { Flag3 = process3.HasExited; }
                    catch { Flag3 = true; };
                    if ((Flag3))
                    {
                        CsNormalInvokeButton();
                        break;
                    }
                }

                Console.WriteLine("Complete!!");
                try
                {
                    isformEnabled = true;
                    FormEnabled();
                    process3.Dispose();
                }
                catch { };

                return "Stop";
            });
            output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] -----カスタマイズファイルアップロード終了-----" + "\r\n";
            Invoke(output);
            return task;
        }
        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Direct = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            Direct = Direct + @"\echo.bat";
            string command = Direct;
            string arguments = UploaderArguments;
            Console.WriteLine(arguments);
            using (process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = command,

                    UseShellExecute = false,
                    CreateNoWindow = true,

                    RedirectStandardOutput = true, // ログ出力に必要な設定(1)
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8, // エンコーディング設定
                    StandardErrorEncoding = Encoding.UTF8, // エンコーディング設定
                    Arguments = arguments
                };
                process.OutputDataReceived += OnStdOut;
                process.ErrorDataReceived += OnStdError;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                BackgroundWorker worker = (BackgroundWorker)sender;
                output = "";
                Invoke(output);
                Task<string> task3;
                while (!process.HasExited)
                {
                    worker = (BackgroundWorker)sender;
                    //キャンセル判定
                    // senderの値はbgWorkerの値と同じ
                    Console.WriteLine("test2" + worker.CancellationPending);
                    // 時間のかかる処理
                    // キャンセルされてないか定期的にチェック
                    if (worker.CancellationPending || Flag2)
                    {
                        task3 = Task.Run(async () =>
                        {
                            return await ButtonUp();
                        });
                        e.Cancel = true;
                        await Stop(process);
                        try
                        {
                            process.WaitForExit();
                        }
                        catch { };
                        break;
                    }
                }
                //PsInfo.Dispose();
                try
                {
                    process.WaitForExit();
                }
                catch { };
                Flag = true;

                if (errorLevel != -1)
                {
                    task3 = Task.Run(async () =>
                    {
                        return await ButtonUp();
                    });
                }
                else
                {
                    task3 = Task.Run(async () =>
                    {
                        return await ButtonUp();
                    });
                }
                if (Error != "")
                {
                    isformEnabled = false;
                    this.FormEnabled();
                    MessageBox.Show(Error,
                      "kintone-plugin-uploaderのエラー",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
                    isformEnabled = true;
                    this.FormEnabled();
                }
                if (Error != "")
                {
                    e.Cancel = true;
                    await Stop(process);
                }
                try
                {
                    if (!process2.HasExited)
                    {
                        backgroundWorker2.CancelAsync();
                    }
                    process.CancelOutputRead(); // 使い終わったら止める
                    process.CancelErrorRead();
                }
                catch { }


                //form1.Text += "end!";
            }
        }
        public async Task<string> Ho(Process PsInfo)
        {
            await Task.Run(() =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    // キャンセルされたらTaskを終了する.
                    return "Canceled";
                }
                try
                {
                    output = PsInfo.StandardOutput.ReadLine();
                    output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                    if (output != "") Invoke(output);
                }
                catch { }
                return "";
            });
            return "";
        }
        public async Task<string> Ho2(Process PsInfo)
        {
            await Task.Run(() =>
            {
                if (cancelToken2.IsCancellationRequested)
                {
                    // キャンセルされたらTaskを終了する.
                    return "Canceled";
                }
                try
                {
                    output = PsInfo2.StandardOutput.ReadLine();
                    output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                    if (output != "") Invoke(output);
                }
                catch { }
                return "";
            });
            return "";
        }
        public async Task<string> Ho3(Process PsInfo)
        {
            await Task.Run(() =>
            {
                if (cancelToken3.IsCancellationRequested)
                {
                    // キャンセルされたらTaskを終了する.
                    return "Canceled";
                }
                try
                {
                    output = PsInfo3.StandardOutput.ReadLine();
                    output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                    if (output != "") Invoke(output);
                }
                catch { }
                return "";
            });
            return "";
        }
        public void Invoke(string output)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.UpdateText));
            }
            else
            {
                this.UpdateText();
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
                this.ButtonUpdate();
                return "out!";
            }
        }
        public string CsNormalInvokeButton()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.CsNormalButtonUpdate));
                return "ok";
            }
            else
            {
                this.CsNormalButtonUpdate();
                return "out!";
            }
        }
        private void InvokePluginButton()
        {
            while (!comboBox1.Enabled)
                {
                }
                button4.Text = "実行";
                button4.Enabled = true;
                button3.Enabled = true;
                button7.Enabled = true;
        }
        private void InvokeCustomizeButton()
        {
            while (!comboBox2.Enabled)
            {
                button17.Text = "終了中";
                button17.Enabled = false;
            }
            button17.Text = "実行";
            button11.Enabled = true;
            button15.Enabled = true;
            button17.Enabled = true;
        }
        public void ButtonUpdate()
        {
            comboBox1.Enabled = true;
            Console.WriteLine(errorLevel);
            if (!(errorLevel != -1))
            {
                GetManifestVersion(false);
            }
            if (!(errorLevel2 != -1))
            {
                GetManifestVersion(false);
            }
            else
            {
                Json.version = beforeVersion;
                SaveManifestJson(false);
            }
            Task task = Task.Run(() =>
            {
                this.Invoke(new Action(this.InvokePluginButton));
            });
        }
        public void CsNormalButtonUpdate()
        {
            button17.Text = "終了中";
            button17.Enabled = false;

            comboBox2.Enabled = true;
            Task task = Task.Run(() =>
            {
                this.Invoke(new Action(this.InvokeCustomizeButton));
            });
            Console.WriteLine(errorLevel);
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
                this.ButtonUpdate2();
                return "out";
            }
        }
        public string CsInvokeButton()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.CsButtonUpdate));
                return "ok";
            }
            else
            {
                this.CsButtonUpdate();
                return "out";
            }
        }
        public string FormEnabled()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.InvokeFormEnabled));
                return "ok";
            }
            else
            {
                this.InvokeFormEnabled();
                return "out";
            }
        }
        public void InvokeFormEnabled()
        {
            this.Enabled = isformEnabled;
        }
        public void ButtonUpdate2()
        {
            //button4.Enabled = false;
            button4.Text = "終了中";
            button4.Enabled = false;
        }
        public void CsButtonUpdate()
        {
            button17.Text = "終了中";
            button17.Enabled = false;
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
        public Task<string> Stop(Process PsInfo)
        {
            Task<string> task2 = Task.Run(() =>
            {
                try
                {
                    tokenSource.Cancel();
                    KillProcessTree(process);
                    process.WaitForExit();
                    errorLevel = process.ExitCode;
                }
                catch { }
                return "stop";
            });
            return task2;
        }
        public Task<string> Stop2(Process PsInfo)
        {
            Task<string> task = Task.Run(() =>
            {
                try
                {
                    tokenSource2.Cancel();
                    KillProcessTree(process2);
                    process2.WaitForExit();
                    errorLevel2 = process2.ExitCode;
                }
                catch { }
                return "stop";
            });
            return task;
        }
        public Task<string> Stop3(Process PsInfo)
        {
            Task<string> task = Task.Run(() =>
            {
                try
                {
                    tokenSource3.Cancel();
                    KillProcessTree(process3);
                    process3.WaitForExit();
                    errorLevel3 = process3.ExitCode;
                }
                catch { }
                return "stop";
            });
            return task;
        }
        private void KillProcessTree(System.Diagnostics.Process process)
        {
            string taskkill = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "taskkill.exe");
            using (var procKiller = new System.Diagnostics.Process())
            {
                procKiller.StartInfo.FileName = taskkill;
                procKiller.StartInfo.Arguments = string.Format("/PID {0} /T /F", process.Id);
                procKiller.StartInfo.CreateNoWindow = true;
                procKiller.StartInfo.UseShellExecute = false;
                procKiller.Start();
                procKiller.WaitForExit();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("test");
            groupBox1.Visible = (checkBox1.Checked) ? false : true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            PluginNameSettingForm form2 = new PluginNameSettingForm();
            form2.form1 = this;
            form2.Show();
            this.Enabled = false;
        }
        public void CreatePluginList()
        {
            comboBox1.Items.Clear();
            IEnumerable<FormData> sortfd = Default.fd.OrderBy(x => x.name);
            foreach (FormData data in sortfd)
            {
                comboBox1.Items.Add(data.name);
            }
            if (Default.PluginName != null)
            {
                comboBox1.SelectedItem = Default.PluginName;
            }
        }
        public void CreateCustomizeList()
        {
            comboBox2.Items.Clear();
            IEnumerable<Customize> sortfd = Default.customize.OrderBy(x => x.name);
            foreach (Customize data in sortfd)
            {
                comboBox2.Items.Add(data.name);
            }
            if (Default.CustomizeName != null)
            {
                comboBox2.SelectedItem = Default.CustomizeName;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            LogSaveFile();
        }
        private void LogSaveFile()
        {
            try
            {
                using (var ofd = new SaveFileDialog()
                {
                    FileName = "Log_" + DateTime.Now.ToString("yyMMdd") + ".txt",
                    Filter = "Text files (*.txt)|*.txt",
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                })
                {
                    if (ofd.FileName == "")
                    {
                        MessageBox.Show("ファイル名を入力してください");
                        return;
                    }
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        // テキストファイル出力（新規作成）
                        using (StreamWriter sw = new StreamWriter(ofd.FileName, false))
                        {
                            sw.WriteLine(textBox1.Text);
                            MessageBox.Show("出力が完了しました");
                        }
                    }
                    else
                    {
                        Console.WriteLine("キャンセルされました");
                    }
                }
            }
            // 例外処理
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Default.PluginName = comboBox1.Text;
            Default.Save();
            LoadPluginListValue();
            GetManifestVersion(true);
        }
        private void LoadPluginListValue()
        {
            FormData SettingValue = Default.fd.Find(x => x.name == comboBox1.Text);
            if (SettingValue != null)
            {
                textBox2.Text = SettingValue.url;
                textBox3.Text = SettingValue.username;
                textBox4.Text = SettingValue.password;
                textBox5.Text = SettingValue.Direct;
                PluginSet = SettingValue;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = true;
                button7.Enabled = true;
                textBox6.Text = SettingValue.ppk;
                if (textBox5.Text != "" && PluginSet.plusBool)
                {
                    button8.Enabled = true;
                }
                if (textBox5.Text != "")
                {
                    button9.Enabled = true;
                }
                else
                {
                    button9.Enabled = false;
                }
            }
            else
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
                button1.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            FormData SettingValue = Default.fd.Find(x => x.name == comboBox1.Text);
            if (SettingValue != null)
            {
                if (MessageBox.Show("削除しますか？", "プラグインの設定の削除", MessageBoxButtons.YesNo) == DialogResult.No) { return; }
                Default.fd.RemoveAll(x => x.name == comboBox1.Text);
                Default.Save();
                comboBox1.SelectedIndex = comboBox1.SelectedIndex - 1;
                CreatePluginList();
                LoadPluginListValue();
                GetManifestVersion(true);
            }
            else
            {
                return;
            }
        }
        private async void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Direct2 = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            if (textBox6.Text != "")
            {
                Direct2 = Direct2 + @"\echo8.bat";
            }
            else
            {
                Direct2 = Direct2 + @"\echo2.bat";
            }
            string command = Direct2;
            string arguments = (textBox6.Text != "") ? PackerPpkArguments : PackerArguments;
            Console.WriteLine(arguments);
            using ( process2 = new Process())
            {
                process2.StartInfo = new ProcessStartInfo()
                {
                    FileName = command,

                    UseShellExecute = false,
                    CreateNoWindow = true,

                    RedirectStandardOutput = true, // ログ出力に必要な設定(1)
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8, // エンコーディング設定
                    StandardErrorEncoding = Encoding.UTF8, // エンコーディング設定
                    Arguments = arguments
                };
                process2.OutputDataReceived += OnStdOut2;
                process2.ErrorDataReceived += OnStdError2;
                process2.Start();
                process2.BeginOutputReadLine();
                process2.BeginErrorReadLine();
                BackgroundWorker worker = (BackgroundWorker)sender;
                output = "";
                Invoke(output);
                Task<string> task3;
                while (!process2.HasExited)
                {
                    worker = (BackgroundWorker)sender;
                    //キャンセル判定
                    // senderの値はbgWorkerの値と同じ
                    Console.WriteLine("test" + worker.CancellationPending);
                    // 時間のかかる処理
                    // キャンセルされてないか定期的にチェック
                    if (worker.CancellationPending)
                    {
                        task3 = Task.Run(async () =>
                        {
                            return await ButtonUp();
                        });
                        e.Cancel = true;
                        await Stop2(process2);
                        try
                        {
                            process2.WaitForExit();
                        }
                        catch { };
                        break;
                    }
                }
                try
                {
                    process2.WaitForExit();
                }
                catch { };
                //PsInfo.Dispose();
                Console.WriteLine(worker.CancellationPending);
                Flag2 = true;

                if (errorLevel2 != -1)
                {
                    task3 = Task.Run(async () =>
                    {
                        return await ButtonUp();
                    });
                }
                else
                {
                    task3 = Task.Run(async () =>
                    {
                        return await ButtonUp();
                    });
                }
                if (Error2 != "")
                {
                    isformEnabled = false;
                    this.FormEnabled();
                    MessageBox.Show(Error2,
                      "kintone-plugin-packerのエラー",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
                    isformEnabled = true;
                    this.FormEnabled();
                }


                if (Error2 != "")
                {
                    e.Cancel = true;
                    await Stop2(process2);
                }
                try
                {
                    if (!process.HasExited)
                    {
                        backgroundWorker1.CancelAsync();
                    }
                    process2.CancelOutputRead(); // 使い終わったら止める
                    process2.CancelErrorRead();
                }
                catch { }

                //form1.Text += "end!";
            }
        }
        public ManifestJsonData GetManifestVersion(bool MassegeBool)
        {
            PluginSet = Default.fd.Find(x => x.name == comboBox1.Text);
            if (textBox5.Text != "")
            {
                try
                {
                    using (var sr = new StreamReader(textBox5.Text + @"\src\manifest.json"))
                    {
                        var jsonData = sr.ReadToEnd();
                        Json = System.Text.Json.JsonSerializer.Deserialize<ManifestJsonData>(jsonData);
                        if (PluginSet.plusBool)
                        {
                            PlusVersion = (Json.version + PluginSet.plus);
                            label6.Text = "ver:" + Json.version + " →" + PlusVersion;
                        }
                        else
                        {
                            label6.Text = "ver:" + Json.version;
                        }
                        button8.Enabled = true;
                        Console.WriteLine(Json.name.ja);
                        return Json;
                    }
                }
                catch
                {
                    button8.Enabled = false;
                    PluginSet.plusBool = false;
                    if (MassegeBool)
                    {
                        MessageBox.Show("manifest.jsonファイルが読み込めません");
                    }
                }
            }
            else
            {
                button8.Enabled = false;
                if (comboBox1.Text != "")
                {
                    PluginSet.plusBool = false;
                }
            }
            label6.Text = "ver:";
            return Json;
        }
        public void SaveManifestJson(bool executionBool)
        {
            if (executionBool && PluginSet.plusBool)
            {
                Json.version = PlusVersion;
            }
            try
            {
                string JsonStr = System.Text.Json.JsonSerializer.Serialize<ManifestJsonData>(Json);
                // テキストファイル出力（新規作成）
                using (StreamWriter sw = new StreamWriter(textBox5.Text + @"\src\manifest.json", false))
                {
                    sw.WriteLine(JsonStr);
                    Console.WriteLine(JsonStr);
                }
            }
            // 例外処理
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            PluginSet = Default.fd.Find(x => x.name == comboBox1.Text);
            Console.WriteLine(PluginSet.plusBool);
            form3 = new ManifestVisibleForm();
            form3.form1 = this;
            form3.Show();
            this.Enabled = false;
            form3.srcDir = @"\src\";
            List<CustomizeFileList> customizejsFileLists = new List<CustomizeFileList>();
            List<CustomizeFileList> customizeCssFileLists = new List<CustomizeFileList>();
            Console.WriteLine(Json);
            string fileExists = "";
            foreach (string jsFile in Json.desktop.js)
            {
                bool response = false;
                if (Regex.IsMatch(jsFile, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        response = client.GetAsync(jsFile).Result.IsSuccessStatusCode;
                        Console.WriteLine(response);
                        fileExists = (response) ? "" : "URL_NOT_SUCCESSFUL";
                    }
                }
                else
                {
                    fileExists = (System.IO.File.Exists(textBox5.Text + form3.srcDir + jsFile)) ? "" : "FILE_NOT_FOUND";
                }
                customizejsFileLists.Add(new CustomizeFileList { fileDir = jsFile, fileExists = fileExists });
            }
            foreach (string cssFile in Json.desktop.css)
            {
                bool response = false;
                if (Regex.IsMatch(cssFile, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        response = client.GetAsync(cssFile).Result.IsSuccessStatusCode;
                        Console.WriteLine(response);
                        fileExists = (response) ? "" : "URL_NOT_SUCCESSFUL";
                    }
                }
                else
                {
                    fileExists = (System.IO.File.Exists(textBox5.Text + form3.srcDir + cssFile)) ? "" : "FILE_NOT_FOUND";
                }
                customizeCssFileLists.Add(new CustomizeFileList { fileDir = cssFile, fileExists = fileExists });
            }
            form3.lists = customizejsFileLists;
            form3.cssLists = customizeCssFileLists;
            form3.dir = textBox5.Text;
            form3.mainForm = this;
            form3.ListUpdate();
            form3.CssListUpdate();
        }
        public void SaveDefault()
        {
            PluginSet = Default.fd.Find(x => x.name == comboBox1.Text);
            Default.fd.RemoveAll(x => x.name == MainForm.PluginSet.name);
            Default.fd.Add(new FormData { url = textBox2.Text, username = textBox3.Text, password = textBox4.Text, Direct = textBox5.Text, name = comboBox1.Text, plus = form3.formData.plus, plusBool = form3.formData.plusBool });
            Default.Save();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog()
                {
                    FileName = textBox6.Text,
                    Filter = "ppk files (*.ppk)|*.ppk",
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName == "")
                        {
                            MessageBox.Show("ファイルを選択してください");
                            return;
                        }
                        // ファイルパスからファイル名を取得
                        if (!ofd.FileName.Contains(textBox5.Text))
                        {
                            MessageBox.Show("フォームで入力したディレクトリの配下のppkファイルを選択してください");
                            return;
                        }
                        textBox6.Text = Path.GetFileName(ofd.FileName);
                    }
                    else
                    {
                        Console.WriteLine("キャンセルされました");
                    }
                }
            }
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            GetManifestVersion(false);
            textBox6.Text = button8.Enabled ? textBox6.Text : "";
        }

        private void tab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(button4.Text!="実行"　|| button17.Text != "実行")
            {
                e.Cancel = true;
            } 
        }

        private void button16_Click(object sender, EventArgs e)
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
                textBox11.Text = Fbd.SelectedPath;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (button14.Text == "表示")
            {
                button14.Text = "非表示";
                // システムのパスワード文字を設定
                textBox10.UseSystemPasswordChar = false;
            }
            else
            {
                // システムのパスワード文字を設定
                textBox10.UseSystemPasswordChar = true;
                button14.Text = "表示";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = (checkBox4.Checked) ? false : true;
        }
        private void LoadCustomizeListValue()
        {
            Customize SettingValue = Default.customize.Find(x => x.name == comboBox2.Text);
            if (SettingValue != null)
            {
                textBox8.Text = SettingValue.url;
                textBox9.Text = SettingValue.username;
                textBox10.Text = SettingValue.password;
                textBox11.Text = SettingValue.Direct;
                textBox7.Enabled = (textBox7.Text != "") ? true : false;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox10.Enabled = true;
                textBox11.Enabled = true;
                button11.Enabled = true;
                button15.Enabled = true;
                button16.Enabled = true;
                button17.Enabled = true;
                button12.Enabled = (textBox7.Text != "") ? true: false;
            }
            else
            {
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                textBox7.Text = "";
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                button12.Enabled = false;
                textBox7.Enabled = false;
                button11.Enabled = false;
                button15.Enabled = false;
                button16.Enabled = false;
                button17.Enabled = false;
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            CustomizeVisibleForm customizeVisibleForm = new CustomizeVisibleForm();
            customizeVisibleForm.Show();
            List<CustomizeFileList> customizeFileLists = new List<CustomizeFileList>();
            List<CustomizeFileList> customizeCssFileLists = new List<CustomizeFileList>();
            Console.WriteLine(CustomizeJson);
            string fileExists="";
            foreach (string jsFile in CustomizeJson.desktop.js)
            {
                bool response=false;
                if (Regex.IsMatch(jsFile, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        response = client.GetAsync(jsFile).Result.IsSuccessStatusCode;
                        Console.WriteLine(response);
                        fileExists = (response) ? "" : "URL_NOT_SUCCESSFUL";
                    }
                }
                else
                {
                    fileExists = (System.IO.File.Exists(jsFile)) ? "" : "FILE_NOT_FOUND";
                }
                customizeFileLists.Add(new CustomizeFileList { fileDir = jsFile, fileExists = fileExists });
            }
            foreach (string cssFile in CustomizeJson.desktop.css)
            {
                bool response = false;
                if (Regex.IsMatch(cssFile, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        response = client.GetAsync(cssFile).Result.IsSuccessStatusCode;
                        Console.WriteLine(response);
                        fileExists = (response) ? "" : "URL_NOT_SUCCESSFUL";
                    }
                }
                else
                {
                    fileExists = (System.IO.File.Exists(cssFile)) ? "" : "FILE_NOT_FOUND";
                }
                customizeCssFileLists.Add(new CustomizeFileList { fileDir = cssFile, fileExists = fileExists });
            }
            customizeVisibleForm.lists = customizeFileLists;
            customizeVisibleForm.cssLists = customizeCssFileLists;
            customizeVisibleForm.dir = textBox11.Text;
            customizeVisibleForm.mainForm = this;
            customizeVisibleForm.ListUpdate();
            customizeVisibleForm.CssListUpdate();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            CustomizeNameSettingForm form2 = new CustomizeNameSettingForm();
            form2.form1 = this;
            form2.Show();
            this.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Customize SettingValue = Default.customize.Find(x => x.name == comboBox2.Text);
            if (SettingValue != null)
            {
                if (MessageBox.Show("削除しますか？", "カスタマイズの設定の削除", MessageBoxButtons.YesNo) == DialogResult.No) { return; }
                Default.customize.RemoveAll(x => x.name == comboBox2.Text);
                Default.Save();
                comboBox2.SelectedIndex = comboBox2.SelectedIndex - 1;
                CreateCustomizeList();
                LoadCustomizeListValue();
                GetManifestCustomize(true);
            }
            else
            {
                return;
            }
        }
        private CustomizeJsonData GetManifestCustomize(bool MassegeBool)
        {
            PluginSetCustomize = Default.customize.Find(x => x.name == comboBox2.Text);
            if (textBox11.Text != "")
            {
                try
                {
                    using (var sr = new StreamReader(textBox11.Text + @"\dest\customize-manifest.json"))
                    {
                        var jsonData = sr.ReadToEnd();
                        CustomizeJson = System.Text.Json.JsonSerializer.Deserialize<CustomizeJsonData>(jsonData);
                        button12.Enabled = true;
                        textBox7.Text = CustomizeJson.app;
                        textBox7.Enabled = true;
                        button12.Enabled = true;
                        return CustomizeJson;
                    }
                }
                catch
                {
                    button8.Enabled = false;
                    if (MassegeBool)
                    {
                        button12.Enabled = false;
                        MessageBox.Show("customize-manifest.jsonファイルが読み込めません");
                    }
                }
            }
            else
            {
                button12.Enabled = false;

            }
            return CustomizeJson;
        }
        public void SaveManifestCustomize(bool executionBool)
        {
            try
            {
                string JsonStr = System.Text.Json.JsonSerializer.Serialize<CustomizeJsonData>(CustomizeJson);
                // テキストファイル出力（新規作成）
                using (StreamWriter sw = new StreamWriter(textBox11.Text + @"\dest\customize-manifest.json", false))
                {
                    sw.WriteLine(JsonStr);
                    Console.WriteLine(JsonStr);
                }
            }
            // 例外処理
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
            }
        }
        //実行ボタン
        private async void button17_Click(object sender, EventArgs e)
        {
            if (button17.Text == "実行" && comboBox2.Enabled == true)
            {
                if (!button12.Enabled) { MessageBox.Show("manifest.jsonが読み込まれていません"); return; }
                Error3 = "";
                errorLevel3 = 0;
                Flag3 = false;
                button11.Enabled = false;
                comboBox2.Enabled = false;
                button15.Enabled = false;
                textBox1.Text = (checkBox3.Checked) ? "" : textBox1.Text;
                Controller Controller = new Controller();
                Controller.Direct = textBox11.Text;
                Controller.Url = textBox8.Text;
                Controller.Username = textBox9.Text;
                Controller.Password = textBox10.Text;
                comandprocess = new ComandProcess();
                UploaderArguments = comandprocess.CreateUploaderArguments(Controller);
                button17.Text = "実行を終了";
                tokenSource3 = new CancellationTokenSource();
                cancelToken3 = tokenSource3.Token;
                SaveManifestCustomize(true);
                backgroundWorker3.RunWorkerAsync();
                output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] -----カスタマイズファイルアップロード開始-----" + "\r\n";
                Invoke(output);
            }
            else
            {
                DialogResult message = MessageBox.Show("実行を終了してもよろしいですか?",
                 "終了の確認",
                 MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Warning);
                if (message == DialogResult.OK)
                {
                    Flag3 = (Flag3) ? Flag3 : false;
                    if (!Flag3) { backgroundWorker3.CancelAsync(); }
                    await CustomizeButtonUp();
                }
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            Default.customize.RemoveAll(x => x.name == comboBox2.Text);
            Default.customize.Add(new Customize { url = textBox8.Text, username = textBox9.Text, password = textBox10.Text, Direct = textBox11.Text, name = comboBox2.Text });
            Default.Save();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            LogSaveFile();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Default.CustomizeName = comboBox2.Text;
            Default.Save();
            LoadCustomizeListValue();
            GetManifestCustomize(false);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            CustomizeJson.app = textBox7.Text;
            SaveManifestCustomize(false);
            GetManifestCustomize(true);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            GetManifestCustomize(true);
            textBox11.Text = button12.Enabled ? textBox11.Text : textBox11.Text;
        }
        public void OnStdOut(object sender, DataReceivedEventArgs e)
        {
            try
            {
                output = e.Data;
                if (output.IndexOf("plugin.zip をアップロードしました!") >= 0)
                {
                    // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("プラグインのアップロードが完了しました!")
                        .AddText("kintoneの環境に正常に反映されました。")
                        .Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
                }
                output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] " + output + "\r\n";
                if (output != "") Invoke(output);
            }
            catch { };
        }
        public void OnStdError(object sender, DataReceivedEventArgs e)
        {
            Error += e.Data;
            Console.WriteLine(Error);
        }
        public void OnStdOut2(object sender, DataReceivedEventArgs e)
        {
            try
            {
                output = e.Data;
                output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                output = "["+DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒")+"] "+ output + "\r\n";
                if (output != "") Invoke(output);
            }
            catch { };
        }
        public void OnStdError2(object sender, DataReceivedEventArgs e)
        {
            Error2 += e.Data;
        }
        public void OnStdOut3(object sender, DataReceivedEventArgs e)
        {
                try
                {
                    output = e.Data;
                if (output.IndexOf("運用環境に反映しました!") >= 0)
                {
                    // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("カスタマイズファイルのアップロードが完了しました!")
                        .AddText("kintoneの環境に正常に反映されました。")
                        .Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
                }
                output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
                output = "[" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒") + "] " + output + "\r\n";
                if (output != "") Invoke(output);
                }
                catch { };
        }
        public void OnStdError3(object sender, DataReceivedEventArgs e)
        {
            Error3+=e.Data;
        }
        private async void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            Direct3 = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            Direct3 = Direct3 + @"\CustomizeProcess.bat";
            
            string command = Direct3;
            string arguments = UploaderArguments;
            Console.WriteLine(arguments);
            using ( process3 = new Process())
            {
                process3.StartInfo = new ProcessStartInfo()
                {
                    FileName = command,

                    UseShellExecute = false,
                    CreateNoWindow = true,
                 
                    RedirectStandardOutput = true, // ログ出力に必要な設定(1)
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8, // エンコーディング設定
                    StandardErrorEncoding = Encoding.UTF8, // エンコーディング設定
                    Arguments = arguments
                };
                process3.OutputDataReceived += OnStdOut3;
                process3.ErrorDataReceived += OnStdError3;
                process3.Start();
                process3.BeginOutputReadLine();
                process3.BeginErrorReadLine();
                BackgroundWorker worker = (BackgroundWorker)sender;
                output = "";
                Invoke(output);
                Task<string> task3;
                while (!process3.HasExited)
                {

                    //await Ho3(process);

                    worker = (BackgroundWorker)sender;
                    //キャンセル判定
                    // senderの値はbgWorkerの値と同じ
                    Console.WriteLine("test" + worker.CancellationPending);
                    // 時間のかかる処理
                    // キャンセルされてないか定期的にチェック
                    if (worker.CancellationPending)
                    {
                        task3 = Task.Run(async () =>
                        {
                            return await CustomizeButtonUp();
                        });
                        e.Cancel = true;
                        await Stop3(process3);
                        try
                        {
                            process3.WaitForExit();
                        }
                        catch { };
                        break;
                    }
                }
                //PsInfo.Dispose();
                try
                {
                    process3.WaitForExit();
                }
                catch { };
                Console.WriteLine(worker.CancellationPending);
                if (Error3 != "")
                {
                    isformEnabled = false;
                    this.FormEnabled();
                    MessageBox.Show(Error3,
                      "kintone-customize-uploaderのエラー",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
                    isformEnabled = true;
                    this.FormEnabled();
                }
                Flag3 = true;
                if (errorLevel3 != -1)
                {
                    task3 = Task.Run(async () =>
                    {
                        return await CustomizeButtonUp();
                    });
                }
                else
                {
                    task3 = Task.Run(async () =>
                    {
                        return await CustomizeButtonUp();
                    });
                }
                try
                {
                    process3.CancelOutputRead(); // 使い終わったら止める
                    process3.CancelErrorRead();                }
                catch { };
            }
        }
        private void Proc_Exited(object sender, EventArgs e)
        {
            var proc = (Process)sender;
            Console.WriteLine("$HasExited Status is {proc.HasExited} (Exited Event)");
        }
    }
}