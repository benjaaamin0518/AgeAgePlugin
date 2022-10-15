using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgeAgePlugin
{
    public partial class CustomizeVisibleForm : Form
    {
        public List<CustomizeFileList> lists { get; set; }
        public string dir { get; set; }
        public MainForm mainForm { get; set; }
        public CustomizeVisibleForm()
        {
            InitializeComponent();
        }
        public void ListUpdate()
        {
            foreach (CustomizeFileList list in lists)
            {
                string[] vs = { list.fileDir, list.fileExists };
                listView1.Items.Add(new ListViewItem(vs));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;

            // フィルターの設定
            openFileDialog1.Filter = "JSファイル|*.JS;*.js";

            // ダイアログボックスの表示
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたファイルをテキストボックスに表示する
                foreach (string strFilePath in openFileDialog1.FileNames)
                {
                    // ファイルパスからファイル名を取得
                    if (!strFilePath.Contains(dir))
                    {
                        MessageBox.Show("メインフォームで入力したディレクトリの配下のファイルを選択してください");
                        return;
                    }
                    string strResultPath = strFilePath.Replace(dir, "");
                    string[] vs = { strResultPath, "" };

                    // リストボックスにファイル名を表示
                    listView1.Items.Add(new ListViewItem(vs));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 全リストを取得し、選択されているアイテムをリストビューから削除する
            foreach (ListViewItem item in listView1.Items)
            {
                // 選択されているか確認する
                if (item.Selected)
                {
                    listView1.Items.Remove(item);
                }
            }
        }
        private void CustomizeVisibleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Enabled = true;
        }
    }
}
