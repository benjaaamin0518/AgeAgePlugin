using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgeAgePlugin
{
    public partial class CustomizeNameSettingForm : Form
    {
        public MainForm form1 { get; set; }
        private Properties.Settings Default { get; set; }
        public string Directory { get; set; }
        public CustomizeNameSettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Directory = "";
            if (textBox1.Text != "")
            {
                int datas = Default.customize.Where(x => x != null).Where(x => x.name == textBox1.Text).ToList().Count;
                if (datas > 0)
                {
                    MessageBox.Show("重複しているカスタマイズ名は追加することが出来ません");
                    return;
                }
                if (checkBox1.Checked)
                {
                    bool Create = CreateCustomize();
                    if (!Create)
                    {
                        MessageBox.Show("カスタマイズ環境を作成することができませんでした");
                        return;
                    }
                }
                MessageBox.Show("追加が完了しました");
                CreateSettingCustomize();
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("カスタマイズ名を入力してください");
                return;
            }
        }
        private void CreateSettingCustomize()
        {
            List<Customize> formData = new List<Customize>();
            formData.Add(new Customize { name = textBox1.Text, Direct = Directory });
            Default.customize.AddRange(formData);
            form1.CreateCustomizeList();
            Default.Save();
        }
        private bool CreateCustomize()
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            //上部に表示する説明テキストを指定する
            Fbd.Description = "カスタマイズ環境を作成するフォルダを指定してください。";
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
                string directory = Fbd.SelectedPath;
                if (directory == "") { return false; }
                string Direct = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                Direct = Direct + @"\CreateCustomize.bat";
                string command = Direct;
                string arguments = Fbd.SelectedPath;
                Console.WriteLine(arguments);
                ProcessStartInfo p = new ProcessStartInfo();
                p.Arguments = arguments;
                p.CreateNoWindow = false; // コンソールを開かない
                p.UseShellExecute = false; // シェル機能を使用しない
                p.FileName = command;
                Process PsInfo = Process.Start(p);
                PsInfo.WaitForExit();
                int exitCode = PsInfo.ExitCode;
                PsInfo.Close();
                if (exitCode != 0) { return false; }
                Directory = Fbd.SelectedPath;
                return true;
            }
            return false;
        }
        private void CustomizeNameSettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;

        }

        private void CustomizeNameSettingForm_Load(object sender, EventArgs e)
        {
            Default = Properties.Settings.Default;
        }
    }
}
