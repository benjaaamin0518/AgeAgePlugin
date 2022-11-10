using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgeAgePlugin
{
    public partial class URLExListAddForm : Form
    {
        public CustomizeVisibleForm customizeVisibleForm { get; set; }
        public List<string> DuplicateList { get; set; }
        public URLExListAddForm()
        {
            InitializeComponent();
        }

        private void URLExListAddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            customizeVisibleForm.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(textBox1.Text, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
            {
                MessageBox.Show("URLが正しくありません");
                return;
            }
            bool DuplicateErr = false;

            if (this.Text == "URLを追加")
            {

                DuplicateErr = (DuplicateList.Where(x => x == textBox1.Text).Count() > 0) ? true : false;

                if (DuplicateErr)
                {
                    MessageBox.Show("そのURLは既に登録されています。");
                    return;
                }
                customizeVisibleForm.InputUrl = textBox1.Text;
                customizeVisibleForm.UrlListAdd();
            }
            else
            {
                DuplicateErr = (customizeVisibleForm.InputUrl != textBox1.Text && DuplicateList.Where(x => x == textBox1.Text).Count() > 0) ? true : false;
                if (DuplicateErr)
                {
                    MessageBox.Show("そのURLは既に登録されています。");
                    return;
                }
                customizeVisibleForm.InputUrl = textBox1.Text;
            }
            this.Close();
        }

        private void URLExListAddForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = customizeVisibleForm.InputUrl;
            label1.Text = (this.Text == "URLを変更") ? "変更後のURLを入力" : label1.Text;
        }
    }
}
