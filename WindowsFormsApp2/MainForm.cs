using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApp2
{
    public partial class MainForm : Form
    {
        public ManifestVisibleForm form3 { get; set; }
        private string Direct { get; set; }
        private int errorLevel { get; set; }
        private decimal beforeVersion { get; set; }
        private string Direct2 { get; set; }
        public static FormData PluginSet { get; set; }
        public static ManifestJsonData Json { get; set; }
        private string UploaderArguments { get; set; }
        private string PackerArguments { get; set; }
        private FolderBrowserDialog Fbd { get; set; }
        private Properties.Settings Default { get; set; }
        private ComandProcess comandprocess { get; set; }
        private bool Flag { get; set; }
        private bool Flag2 { get; set; }
        private string output { get; set; }
        private decimal PlusVersion { get; set; }
        CancellationTokenSource tokenSource;
        CancellationToken cancelToken;
        CancellationTokenSource tokenSource2;
        CancellationToken cancelToken2;
        public List<FormData> test { get; set; }
        public List<FormData> fd { get; set; }
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
            Default.fd.Add(new FormData { url = textBox2.Text, username = textBox3.Text, password = textBox4.Text, Direct = textBox5.Text, name = comboBox1.Text });
            Default.Save();
            Console.WriteLine(textBox5.Text);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadExecution();
            Default = Properties.Settings.Default;
            CreatePluginList();
            LoadPluginListValue();
            comboBox1.SelectedItem = Default.PluginName;

        }
        private void LoadExecution()
        {
            ExecutionCondition executionCondition = new ExecutionCondition();
            string npmErr = (executionCondition.NpmExecution()==0) ? "" : "・npmがインストールされていません\n\n";
            string uploderErr = (executionCondition.UploaderExecution()==0) ? "" : "・kintone-plugin-uploaderがインストールされていません\n\n";
            string PackerErr = (executionCondition.PackerExecution()==0) ? "" : "・kintone-plugin-packerがインストールされていません"; 
            if (npmErr !="" || uploderErr != "" || PackerErr != "")
            {
                MessageBox.Show(npmErr+uploderErr+PackerErr,
                                "インストールしてください",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "実行")
            {
                beforeVersion = Json.version;
                errorLevel = 0;
                button3.Enabled = false;
                comboBox1.Enabled = false;
                button7.Enabled = false;
                textBox1.Text = (checkBox2.Checked) ? "" : textBox1.Text;
                Controller Controller = new Controller();
                Controller.Direct = textBox5.Text;
                Controller.Url = textBox2.Text;
                Controller.Username = textBox3.Text;
                Controller.Password = textBox4.Text;
                comandprocess = new ComandProcess();
                UploaderArguments = comandprocess.CreateUploaderArguments(Controller);
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
               

                Flag = false;
                Flag2 = false;
                backgroundWorker2.CancelAsync();
                backgroundWorker1.CancelAsync();
                await ButtonUp();
            }
        }
        private Task<string> ButtonUp()
        {
            Task<string> task = Task.Run(() =>
            {
                while (true)
                {
                    if (Flag && Flag2)
                    {
                        InvokeButton();

                        break;
                    }
                    InvokeButton2();
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
            p.FileName = command;
            BackgroundWorker worker = (BackgroundWorker)sender;
            p.RedirectStandardOutput = true; // 標準出力をリダイレクト
            Process PsInfo = Process.Start(p);
            output = "";
            Invoke(output);
            Task<string> task;
            while (!PsInfo.HasExited)
            {
                task = Task.Run(async () =>
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
                    await Stop(PsInfo);
                    break;
                }
            }
            //PsInfo.Dispose();
            Console.WriteLine(worker.CancellationPending);
            errorLevel = PsInfo.ExitCode;

            Flag = true;
            await ButtonUp();
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
            return output;
        }
        public async Task<string> Ho2(Process PsInfo)
        {
            if (cancelToken2.IsCancellationRequested)
            {
                // キャンセルされたらTaskを終了する.
                return "Canceled";
            }
            output = PsInfo.StandardOutput.ReadLine();
            output = output?.Replace("\r\r\n", "\n"); // 改行コードの修正
            if (output != "") Invoke(output);
            return output;
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
            button3.Enabled = true;
            comboBox1.Enabled = true;
            button7.Enabled = true;
            if (errorLevel == 0)
            {
                GetManifestVersion(false);
            }
            else
            {
               Json.version=beforeVersion;
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
            Task<string> task2=Task.Run(() =>
            {
                tokenSource.Cancel();
                PsInfo.Kill();
                PsInfo.WaitForExit();
                return "stop";
            });
            return task2;
        }
        public Task<string> Stop2(Process PsInfo)
        {
            Task<string> task=Task.Run(() =>
            {
                tokenSource2.Cancel();
                PsInfo.Kill();
                PsInfo.WaitForExit();
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

        private void button6_Click(object sender, EventArgs e)
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
                if (textBox5.Text != "" && PluginSet.plusBool)
                {
                    button8.Enabled = true;
                }

            }
            else
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
                button1.Enabled = false;
                button8.Enabled = false;
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
            Direct2 = Direct2 + @"\echo2.bat";
            string command = Direct2;
            string arguments = PackerArguments;
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
                task = Task.Run(async () =>
                {
                    return await Ho2(PsInfo);
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

                    await Stop2(PsInfo);
                    break;
                }
            }
            //PsInfo.Dispose();
            Console.WriteLine(worker.CancellationPending);
            errorLevel = PsInfo.ExitCode;

            Flag2 = true;
            await ButtonUp();
            //form1.Text += "end!";
        }
        public ManifestJsonData GetManifestVersion(bool MassegeBool)
        {
            PluginSet = Default.fd.Find(x => x.name == comboBox1.Text);

            string version = "";
            if (textBox5.Text != "") {
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
                if (comboBox1.Text!="")
                {
                    PluginSet.plusBool = false;
                }
            }
            label6.Text = "ver:";

            return Json;
        }
        public void SaveManifestJson(bool executionBool)
        {
            if (executionBool&&PluginSet.plusBool)
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
            Default.fd.Add(new FormData { url = textBox2.Text, username = textBox3.Text, password = textBox4.Text, Direct = textBox5.Text, name = comboBox1.Text,plus=form3.formData.plus,plusBool=form3.formData.plusBool });
            Default.Save();

        }
    }
}