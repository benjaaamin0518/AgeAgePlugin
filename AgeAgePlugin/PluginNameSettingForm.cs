﻿using System;
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
    public partial class PluginNameSettingForm : Form
    {
        public MainForm form1 { get; set; }
        private Properties.Settings Default { get; set; }
        public string Directory { get; set; }
        public PluginNameSettingForm()
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
                int datas = Default.fd.Where(x => x != null).Where(x => x.name == textBox1.Text).ToList().Count;
                if (datas > 0)
                {
                    MessageBox.Show("重複しているプラグイン名は追加することが出来ません");
                    return;
                }
                if (checkBox1.Checked)
                {
                    bool Create = CreatePlugin();
                    if (!Create)
                    {
                        MessageBox.Show("プラグインを作成することができませんでした");
                        return;
                    }
                }
                MessageBox.Show("追加が完了しました");
                CreateSettingPlugin();
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("プラグイン名を入力してください");
                return;
            }
        }
        private void CreateSettingPlugin()
        {
            List<FormData> formData = new List<FormData>();
            formData.Add(new FormData { name = textBox1.Text,plusBool=false,Direct=Directory });
            Default.fd.AddRange(formData);
            form1.CreatePluginList();
            Default.Save();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Default = Properties.Settings.Default;

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;
        }
        private bool CreatePlugin()
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog  Fbd = new FolderBrowserDialog();
            //上部に表示する説明テキストを指定する
            Fbd.Description = "プラグインを作成するフォルダを指定してください。";
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
                Direct = Direct + @"\echo6.bat";
                string command = Direct;
                string arguments = Fbd.SelectedPath+" "+textBox1.Text;
                Console.WriteLine(arguments);
                ProcessStartInfo p = new ProcessStartInfo();
                p.Arguments = arguments;
                p.CreateNoWindow = false; // コンソールを開かない
                p.UseShellExecute = false; // シェル機能を使用しない
                p.FileName = command;
                Process PsInfo = Process.Start(p);
                PsInfo.WaitForExit();
                int exitCode=PsInfo.ExitCode;
                PsInfo.Close();
                if (exitCode != 0) { return false; }
                Directory = Fbd.SelectedPath + @"\"+textBox1.Text;
                return true;
            }
            return false;
        }
    }
}
