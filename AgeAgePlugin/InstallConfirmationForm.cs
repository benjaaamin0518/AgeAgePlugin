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
    public partial class InstallConfirmationForm : Form
    {
        public int parsent { get; set; }
        public InstallConfirmationForm()
        {
            InitializeComponent();
        }
        public void InstallProgress(string LabelText)
        {
            progressBar1.Value = parsent;

            label1.Text = LabelText;
        }
    }
}
