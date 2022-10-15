using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AgeAgePlugin
{
    public partial class MainForm : Form
    {
        internal static class AssemblyState
        {
            public const bool IsDebug =
            #if DEBUG
                true;
            #else
               false;
            #endif
            }
        public ManifestVisibleForm form3 { get; set; }
        private string Direct { get; set; }
        private int errorLevel { get; set; }
        private int errorLevel2 { get; set; }
        private decimal beforeVersion { get; set; }
        private string Direct2 { get; set; }
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
        private static bool Flag { get; set; }
        private static bool Flag2 { get; set; }
        private string output { get; set; }
        private decimal PlusVersion { get; set; }
        CancellationTokenSource tokenSource;
        CancellationToken cancelToken;
        CancellationTokenSource tokenSource2;
        CancellationToken cancelToken2;
        public List<FormData> test { get; set; }
        public List<FormData> fd { get; set; }
        public string Error { get; set; }
        public string Error2 { get; set; }
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
            string PackerErr = (executionCondition.PackerExecution() != 9009) ? "" : "・kintone-plugin-packerがインストールされていません";
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
                button4.Text = "実行中";
                tokenSource = new CancellationTokenSource();
                cancelToken = tokenSource.Token;
                tokenSource2 = new CancellationTokenSource();
                cancelToken2 = tokenSource2.Token;
                SaveManifestJson(true);
                backgroundWorker2.RunWorkerAsync();
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                Flag = (Flag) ? false : false;
                Flag2 = (Flag2) ? false : false;
                if (!Flag2) { backgroundWorker2.CancelAsync(); }
                if (!Flag) { backgroundWorker1.CancelAsync(); }
                await ButtonUp();
            }
        }
        private Task<string> ButtonUp()
        {
            Task<string> task = Task.Run(() =>
            {
                while (true)
                {
                    InvokeButton2();
                    if ((Flag && Flag2))
                    {
                        InvokeButton();
                        break;
                    }
                }
                return "Stop";
            });
            return task;
        }
        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Direct = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            Direct = Direct + @"\echo.bat";
            string command = Direct;
            string arguments = UploaderArguments;
            Console.WriteLine(arguments);
            ProcessStartInfo p = new ProcessStartInfo();
            p.Arguments = arguments;
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.StandardOutputEncoding = Encoding.Default; // エンコーディング設定
            p.FileName = command;
            BackgroundWorker worker = (BackgroundWorker)sender;
            p.RedirectStandardOutput = true; // 標準出力をリダイレクト
            p.RedirectStandardError = true; // 標準出力をリダイレクト
            PsInfo = Process.Start(p);
            output = "";
            Invoke(output);
            Task<string> task2;
            Task<string> task3;
            while (!PsInfo.HasExited)
            {
                task2 = Task.Run(async () =>
                {
                    return await Ho(PsInfo);
                });
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
                    await Stop(PsInfo);
                    break;
                }
            }
            //PsInfo.Dispose();


            if (errorLevel != -1)
            {
                task3 = Task.Run(async () =>
                {
                    return await ButtonUp();
                });
                await OutputHandler(PsInfo.StandardError.ReadToEnd());
            }
            else
            {
                task3 = Task.Run(async () =>
                {
                    return await ButtonUp();
                });
            }
            Console.WriteLine("エラー" + PsInfo.HasExited);
            Flag = true;
            if (Error != "")
            {
                e.Cancel = true;
                await Stop(PsInfo);
            }
            if (!PsInfo2.HasExited)
            {
                backgroundWorker2.CancelAsync();
            }
            if (Error != "")
            {
                MessageBox.Show(Error,
                  "kintone-plugin-uploaderのエラー",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Warning);
            }

            //form1.Text += "end!";
        }
        private Task<string> OutputHandler(string err)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)

            Task<string> vs = Task.Run(() => { Error += err; return Error; });
            return vs;
        }
        private Task<string> OutputHandler2(string err)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Task<string> vs = Task.Run(() => { Error2 += err; return Error2; });
            return vs;

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
        public void ButtonUpdate()
        {
            button4.Text = "実行";
            button4.Enabled = true;
            button3.Enabled = true;
            comboBox1.Enabled = true;
            button7.Enabled = true;
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
        public Task<string> Stop(Process PsInfo)
        {
            Task<string> task2 = Task.Run(() =>
            {
                try
                {
                    tokenSource.Cancel();
                    PsInfo.Kill();
                    PsInfo.WaitForExit();
                    errorLevel = PsInfo.ExitCode;
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
                    PsInfo2.Kill();
                    PsInfo2.WaitForExit();
                    errorLevel2 = PsInfo2.ExitCode;
                }
                catch { }
                return "stop";
            });
            return task;
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
                using (var ofd = new OpenFileDialog()
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
            ProcessStartInfo p = new ProcessStartInfo();
            p.Arguments = arguments;
            p.CreateNoWindow = true; // コンソールを開かない
            p.UseShellExecute = false; // シェル機能を使用しない
            p.StandardOutputEncoding = Encoding​.Default; // エンコーディング設定
            p.FileName = command;
            BackgroundWorker worker = (BackgroundWorker)sender;
            p.RedirectStandardOutput = true; // 標準出力をリダイレクト
            p.RedirectStandardError = true; // 標準出力をリダイレクト
            PsInfo2 = Process.Start(p);
            output = "";
            Invoke(output);
            Task<string> task2;
            Task<string> task3;
            while (!PsInfo2.HasExited)
            {
                task2 = Task.Run(async () =>
                {
                    return await Ho2(PsInfo2);
                });
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
                    await Stop2(PsInfo2);
                    break;
                }
            }
            //PsInfo.Dispose();
            Console.WriteLine(worker.CancellationPending);
            if (errorLevel2 != -1)
            {
                task3 = Task.Run(async () =>
                {
                    return await ButtonUp();
                });
                await OutputHandler2(PsInfo2.StandardError.ReadToEnd());
            }
            else
            {
                task3 = Task.Run(async () =>
                {
                    return await ButtonUp();
                });
            }
            Flag2 = true;
            if (Error2 != "")
            {
                e.Cancel = true;
                await Stop2(PsInfo2);
            }
            if (!PsInfo.HasExited)
            {
                backgroundWorker1.CancelAsync();
            }
            if (Error2 != "")
            {
                MessageBox.Show(Error2,
                  "kintone-plugin-packerのエラー",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Warning);
            }
            //form1.Text += "end!";
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
            foreach (string jsFile in CustomizeJson.desktop.js)
            {
                string fileExists = (System.IO.File.Exists(jsFile)) ? "" : "FILE_NOT_FOUND";
                customizeFileLists.Add(new CustomizeFileList { fileDir = jsFile, fileExists = fileExists });
            }
            foreach (string cssFile in CustomizeJson.desktop.css)
            {
                string fileExists = (System.IO.File.Exists(cssFile)) ? "" : "FILE_NOT_FOUND";
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
        private void button17_Click(object sender, EventArgs e)
        {

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
    }
}