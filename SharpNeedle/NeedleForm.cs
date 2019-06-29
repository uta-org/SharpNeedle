using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SpotifySharper.Lib;

namespace SharpNeedle
{
    public partial class NeedleForm : Form
    {
        public NeedleForm()
        {
            InitializeComponent();
        }

        private void LoadProcessList()
        {
            listProcesses.Items.Clear();

            Process[] procs = Process.GetProcesses();

            foreach (Process proc in procs)
            {
                ListViewItem li = new ListViewItem();
                li.Text = string.Format("{0} - {1}", proc.ProcessName, proc.Id);
                li.Tag = proc;
                listProcesses.Items.Add(li);
            }

            listProcesses.Sorting = SortOrder.Ascending;
            listProcesses.Sort();

            int? pid = Extensions.FindProcessId();
            if (!pid.HasValue)
                throw new Exception("The Spotify process couldn't be found!");

            int? processIndex = listProcesses.Items.Cast<ListViewItem>()
                .FirstOrDefault(item => (item.Tag as Process)?.Id == pid.Value)?.Index;

            if (!processIndex.HasValue)
                throw new Exception($"Couldn't find any process in the list with PID: '{pid.Value}'!");

            listProcesses.Items[processIndex.Value].Selected = true;
        }

        private void btnLoadProcesses_Click(object sender, EventArgs e)
        {
            LoadProcessList();
        }

        private void NeedleForm_Load(object sender, EventArgs e)
        {
            LoadProcessList();
        }

        private void listProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listProcesses.SelectedItems.Count != 1)
                return;

            ResetProcessInfo();
            ListViewItem selectedItem = listProcesses.SelectedItems[0];

            Process selectedProcess = (Process)selectedItem.Tag;

            try
            {
                lblModuleBase.Text = selectedProcess.MainModule.BaseAddress.ToString("X");
            }
            catch
            {
                // ignored
            }

            try
            {
                lblThreads.Text = selectedProcess.Threads.Count.ToString();
            }
            catch
            {
                // ignored
            }
        }

        private void ResetProcessInfo()
        {
            lblModuleBase.Text = "N/A";
            lblThreads.Text = "N/A";
        }

        private void btnInjectPayload_Click(object sender, EventArgs e)
        {
            if (listProcesses.SelectedItems.Count != 1)
            {
                MessageBox.Show("You must select one process target", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process targetProcess = (Process)listProcesses.SelectedItems[0].Tag;

            string domainFilePath = Application.StartupPath;//Set the directory containing the dll
            string payloadFilePath = Application.StartupPath;

            PayloadInjector injector = new PayloadInjector(targetProcess, domainFilePath, textboxDomain.Text, payloadFilePath, textboxPayload.Text, textboxArgs.Text);
            injector.InjectAndForget();
        }
    }
}