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
    public partial class Form2 : Form
    {
        public おじょじょぼじゅぼぼ form1 { get; set; }
        private Properties.Settings Default { get; set; }
        public Form2()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                int datas = Default.fd.Where(x => x != null).Where(x => x.name == textBox1.Text).ToList().Count;
                if (datas > 0)
                {
                    MessageBox.Show("重複しているプラグイン名は追加することが出来ません");
                    return;
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
            formData.Add(new FormData { name = textBox1.Text,plusBool=false });
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
    }
}
