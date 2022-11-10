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
    public partial class ManifestVisibleForm : Form
    {
        private Properties.Settings Default { get; set; }
        public MainForm form1 { get; set; }
        public FormData formData { get; set; }
        public bool CheckBool { get; set; }
        public List<CustomizeFileList> lists { get; set; }
        public List<CustomizeFileList> cssLists { get; set; }
        public string dir { get; set; }
        public MainForm mainForm { get; set; }
        public string srcDir { get; set; }
        public string InputUrl { get; set; }
        public string InputUrlType { get; set; }
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
        public void ListUpdate()
        {
            foreach (CustomizeFileList list in lists)
            {
                string[] vs = { list.fileDir.Replace(dir, ""), list.fileExists };
                listView1.Items.Add(new ListViewItem(vs));
            }
        }
        public void CssListUpdate()
        {
            foreach (CustomizeFileList list in cssLists)
            {
                string[] vs = { list.fileDir.Replace(dir, ""), list.fileExists };
                listView2.Items.Add(new ListViewItem(vs));
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            bool DuplicateErr = false;

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
                    string strResultPath = strFilePath.Replace(dir + srcDir, "");
                    DuplicateErr = (MainForm.Json.desktop.js.Where(x => x == strResultPath).Count() > 0) ? true : false;
                    if (DuplicateErr)
                    {
                        MessageBox.Show("そのファイルは既に登録されています。");
                        return;
                    }
                    string[] vs = { strResultPath, "" };

                    // リストボックスにファイル名を表示
                    listView1.Items.Add(new ListViewItem(vs));
                    MainForm.Json.desktop.js = GetListItem(listView1);
                    mainForm.SaveManifestJson(false);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
            MainForm.Json.desktop.js = GetListItem(listView1);
            mainForm.SaveManifestJson(false);
        }
        private List<string> GetListItem(ListView listView)
        {
            List<string> fileDir = new List<string>();
            if (listView.Items.Count > 0)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    fileDir.Add((item.SubItems[0].Text));
                }
                Console.WriteLine(fileDir);
            }
            return fileDir;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool DuplicateErr = false;
            openFileDialog1.Multiselect = true;

            // フィルターの設定
            openFileDialog1.Filter = "CSSファイル|*.CSS;*.css;*.scss;*.SCSS";

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
                    string strResultPath = strFilePath.Replace(dir + srcDir, "");
                    DuplicateErr = (MainForm.Json.desktop.css.Where(x => x == strResultPath).Count() > 0) ? true : false;
                    if (DuplicateErr)
                    {
                        MessageBox.Show("そのファイルは既に登録されています。");
                        return;
                    }
                    string[] vs = { strResultPath, "" };

                    // リストボックスにファイル名を表示
                    listView2.Items.Add(new ListViewItem(vs));
                    MainForm.Json.desktop.css = GetListItem(listView2);
                    mainForm.SaveManifestJson(false);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 全リストを取得し、選択されているアイテムをリストビューから削除する
            foreach (ListViewItem item in listView2.Items)
            {
                // 選択されているか確認する
                if (item.Selected)
                {

                    listView2.Items.Remove(item);
                }
            }
            MainForm.Json.desktop.css = GetListItem(listView2);
            mainForm.SaveManifestJson(false);
        }
        private void listView1_MouseDoubleClick(object sender, EventArgs e)
        {
            // 全リストを取得し、選択されているアイテムをリストビューから削除する
            foreach (ListViewItem item in listView1.Items)
            {
                // 選択されているか確認する
                if (item.Selected)
                {
                    if (Regex.IsMatch(item.Text, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                    {
                        Console.WriteLine(item.Text);
                        InputUrl = item.Text;
                        InputUrlType = "js";
                        UrlListVisible();
                        item.SubItems[0].Text = InputUrl;
                        item.SubItems[1].Text = "";
                    }
                    else
                    {
                        openFileDialog1.Multiselect = true;
                        bool DuplicateErr = false;

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
                                string strResultPath = strFilePath.Replace(dir + srcDir, "");
                                DuplicateErr = (item.Text != strResultPath && MainForm.Json.desktop.js.Where(x => x == strResultPath).Count() > 0) ? true : false;
                                if (DuplicateErr)
                                {
                                    MessageBox.Show("そのファイルは既に登録されています。");
                                    return;
                                }

                                // リストボックスにファイル名を表示
                                item.SubItems[0].Text = strResultPath;
                                item.SubItems[1].Text = "";
                            }
                        }
                    }
                }
            }
            MainForm.Json.desktop.js = GetListItem(listView1);
            mainForm.SaveManifestJson(false);
        }

        private void CustomizeJs_DoubleClick(object sender, EventArgs e)
        {
            // 全リストを取得し、選択されているアイテムをリストビューから削除する
            foreach (ListViewItem item in listView2.Items)
            {
                // 選択されているか確認する
                if (item.Selected)
                {
                    if (Regex.IsMatch(item.Text, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"))
                    {
                        Console.WriteLine(item.Text);
                        InputUrl = item.Text;
                        InputUrlType = "css";
                        UrlListVisible();
                        item.SubItems[0].Text = InputUrl;
                        item.SubItems[1].Text = "";
                    }
                    else
                    {
                        openFileDialog1.Multiselect = true;
                        bool DuplicateErr = false;

                        // フィルターの設定
                        openFileDialog1.Filter = "CSSファイル|*.CSS;*.css;*.scss;*.SCSS";

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
                                string strResultPath = strFilePath.Replace(dir + srcDir, "");
                                DuplicateErr = (item.Text != strResultPath && MainForm.Json.desktop.css.Where(x => x == strResultPath).Count() > 0) ? true : false;
                                if (DuplicateErr)
                                {
                                    MessageBox.Show("そのファイルは既に登録されています。");
                                    return;
                                }

                                // リストボックスにファイル名を表示
                                item.SubItems[0].Text = strResultPath;
                                item.SubItems[1].Text = "";
                            }
                        }
                    }
                }
            }
            MainForm.Json.desktop.css = GetListItem(listView2);
            mainForm.SaveManifestJson(false);
        }
        public bool UrlListAdd()
        {
            if (!String.IsNullOrEmpty(InputUrl))
            {
                Console.WriteLine(InputUrl);
                string[] vs = { InputUrl, "" };

                // リストボックスにファイル名を表示
                if (InputUrlType == "js")
                {
                    listView1.Items.Add(new ListViewItem(vs));
                    MainForm.Json.desktop.js = GetListItem(listView1);
                    mainForm.SaveManifestCustomize(false);
                }
                else
                {
                    listView2.Items.Add(new ListViewItem(vs));
                    MainForm.Json.desktop.css = GetListItem(listView2);
                    mainForm.SaveManifestCustomize(false);
                }
            }
            return true;
        }
        private void UrlListVisible()
        {
            URLExManifestListAddForm uRLExManifestListAddForm = new URLExManifestListAddForm();
            uRLExManifestListAddForm.manifestVisibleForm = this;
            this.Enabled = false;
            uRLExManifestListAddForm.Text = "URLを変更";
            uRLExManifestListAddForm.DuplicateList = (InputUrlType == "js") ? MainForm.Json.desktop.js : MainForm.Json.desktop.css;
            uRLExManifestListAddForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            URLExManifestListAddForm uRLExManifestListAddForm = new URLExManifestListAddForm();
            uRLExManifestListAddForm.manifestVisibleForm = this;
            this.Enabled = false;
            this.InputUrl = "";
            this.InputUrlType = "js";
            uRLExManifestListAddForm.DuplicateList = MainForm.Json.desktop.js;
            uRLExManifestListAddForm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            URLExManifestListAddForm uRLExManifestListAddForm = new URLExManifestListAddForm();
            uRLExManifestListAddForm.manifestVisibleForm = this;
            this.Enabled = false;
            this.InputUrl = "";
            this.InputUrlType = "css";
            uRLExManifestListAddForm.DuplicateList = MainForm.Json.desktop.css;
            uRLExManifestListAddForm.ShowDialog();
        }
    }
}
