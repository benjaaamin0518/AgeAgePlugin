using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ManifestVisibleForm : Form
    {
        private Properties.Settings Default { get; set; }
        public MainForm form1 { get; set; }
        public FormData formData { get; set; }
        public bool CheckBool { get; set; }


        public ManifestVisibleForm()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Text =MainForm.PluginSet.plus.ToString()  ;
            textBox1.Text = MainForm.Json.name.ja;
            textBox2.Text = MainForm.Json.description.ja;
            textBox3.Text = MainForm.Json.version.ToString();
            checkBox1.Checked = MainForm.PluginSet.plusBool;
            ChangeEnabledVersion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("manifest.jsonファイルに変更を加えますがよろしいですか？", "manifest.json変更の確認", MessageBoxButtons.YesNo) == DialogResult.No) { return; }


            MainForm.Json.version = Convert.ToDecimal(textBox3.Text); 
            MainForm.Json.name.ja =textBox1.Text;
            MainForm.Json.description.ja = textBox2.Text;

            formData = new FormData();
            try
            {

                    formData.plus = Convert.ToDecimal(comboBox1.Text);
            }
            catch
            {
                MessageBox.Show("自動で更新するバージョン数を入力してください");
                return;
            }
            form1.SaveManifestJson(false);

            formData.plusBool = comboBox1.Enabled;
            form1.SaveDefault();
            form1.GetManifestVersion(false);
            if (formData.plusBool)
            {
                MessageBox.Show("manifest.jsonファイルを変更しました。\n※自動バージョンアップは「実行」ボタン押下時にmanifest.jsonファイルに変更が行われます");
            }
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.GetManifestVersion(true);
            form1.Enabled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && !(e.KeyChar == (char)Keys.Back))
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }
        private void ChangeEnabledVersion()
        {
            comboBox1.Enabled = CheckBool;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBool = checkBox1.Checked;
            ChangeEnabledVersion();
        }
    }
}
