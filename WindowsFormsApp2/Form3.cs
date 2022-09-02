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
    public partial class Form3 : Form
    {
        private Properties.Settings Default { get; set; }
        public おじょじょぼじゅぼぼ form1 { get; set; }


        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Default = Properties.Settings.Default;
            comboBox1.SelectedItem =おじょじょぼじゅぼぼ.PluginSet  ;
            textBox1.Text = おじょじょぼじゅぼぼ.Json.name.jp;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            おじょじょぼじゅぼぼ.Json.version =Int32.Parse(textBox1.Text);
            おじょじょぼじゅぼぼ.Json.name =textBox2.Text;
            おじょじょぼじゅぼぼ form1 = new おじょじょぼじゅぼぼ();
            form1.SaveManifestJson();
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;
        }
    }
}
