using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SharpPayloadWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblParent.Text = Process.GetCurrentProcess().ProcessName;
        }
    }
}