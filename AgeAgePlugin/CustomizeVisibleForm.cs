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
    }
}
