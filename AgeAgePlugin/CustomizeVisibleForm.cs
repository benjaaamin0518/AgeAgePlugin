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
    public partial class CustomizeVisibleForm : Form
    {
        public List<CustomizeFileList> lists { get; set; }
        public List<CustomizeFileList> cssLists { get; set; }
        public string dir { get; set; }
        public MainForm mainForm { get; set; }
        public string InputUrl { get; set; }
        public string InputUrlType { get; set; }
        public CustomizeVisibleForm()
        {
            InitializeComponent();
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
        private void button1_Click(object sender, EventArgs e)
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
                    DuplicateErr=(MainForm.CustomizeJson.desktop.js.Where(x => x == strFilePath).Count()>0)?true:false;
                    if (DuplicateErr)
                    {
                        MessageBox.Show("そのファイルは既に登録されています。");
                        return;
                    }
                    string strResultPath = strFilePath.Replace(dir, "");
                    string[] vs = { strResultPath, "" };

                    // リストボックスにファイル名を表示
                    listView1.Items.Add(new ListViewItem(vs));
                    MainForm.CustomizeJson.desktop.js = GetListItem(listView1);
                    mainForm.SaveManifestCustomize(false);
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
            MainForm.CustomizeJson.desktop.js =GetListItem(listView1);
            mainForm.SaveManifestCustomize(false);
        }
        private void CustomizeVisibleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Enabled = true;
        }
        private List<string> GetListItem(ListView listView)
        {
            List<string> fileDir=new List<string>();
            if (listView.Items.Count > 0)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    string dirStr = Regex.IsMatch(item.SubItems[0].Text, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$") ? item.SubItems[0].Text : dir + item.SubItems[0].Text;
                    fileDir.Add(dirStr);
                }
                Console.WriteLine(fileDir);
            }
            return fileDir;
        }

        private void button4_Click(object sender, EventArgs e)
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
                    DuplicateErr = (MainForm.CustomizeJson.desktop.css.Where(x => x == strFilePath).Count() > 0) ? true : false;
                    if (DuplicateErr)
                    {
                        MessageBox.Show("そのファイルは既に登録されています。");
                        return;
                    }
                    string strResultPath = strFilePath.Replace(dir, "");
                    string[] vs = { strResultPath, "" };

                    // リストボックスにファイル名を表示
                    listView2.Items.Add(new ListViewItem(vs));
                    MainForm.CustomizeJson.desktop.css = GetListItem(listView2);
                    mainForm.SaveManifestCustomize(false);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
            MainForm.CustomizeJson.desktop.css = GetListItem(listView2);
            mainForm.SaveManifestCustomize(false);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
                                string strResultPath = strFilePath.Replace(dir, "");
                                DuplicateErr = (item.Text != strResultPath && MainForm.CustomizeJson.desktop.js.Where(x => x == strFilePath).Count() > 0) ? true : false;
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
            MainForm.CustomizeJson.desktop.js = GetListItem(listView1);
            mainForm.SaveManifestCustomize(false);
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
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
                                string strResultPath = strFilePath.Replace(dir, "");
                                DuplicateErr = (item.Text != strResultPath && MainForm.CustomizeJson.desktop.css.Where(x => x == strFilePath).Count() > 0) ? true : false;
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
            MainForm.CustomizeJson.desktop.css = GetListItem(listView2);
            mainForm.SaveManifestCustomize(false);
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
                    MainForm.CustomizeJson.desktop.js = GetListItem(listView1);
                    mainForm.SaveManifestCustomize(false);
                }
                else
                {
                    listView2.Items.Add(new ListViewItem(vs));
                    MainForm.CustomizeJson.desktop.css = GetListItem(listView2);
                    mainForm.SaveManifestCustomize(false);
                }
            }
            return true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            URLExListAddForm uRLExListAddForm = new URLExListAddForm();
            uRLExListAddForm.customizeVisibleForm = this;
            this.Enabled = false;
            this.InputUrl = "";
            this.InputUrlType = "js";
            uRLExListAddForm.DuplicateList = MainForm.CustomizeJson.desktop.js;
            uRLExListAddForm.ShowDialog();
        }
        private void UrlListVisible()
        {
            URLExListAddForm uRLExListAddForm = new URLExListAddForm();
            uRLExListAddForm.customizeVisibleForm = this;
            this.Enabled = false;
            uRLExListAddForm.Text = "URLを変更";
            uRLExListAddForm.DuplicateList =(InputUrlType=="js")? MainForm.CustomizeJson.desktop.js: MainForm.CustomizeJson.desktop.css;
            uRLExListAddForm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            URLExListAddForm uRLExListAddForm = new URLExListAddForm();
            uRLExListAddForm.customizeVisibleForm = this;
            this.Enabled = false;
            this.InputUrl = "";
            this.InputUrlType = "css";
            uRLExListAddForm.DuplicateList = MainForm.CustomizeJson.desktop.css;
            uRLExListAddForm.ShowDialog();
        }
    }
}
