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

            var procs = Process.GetProcesses();

            foreach (var proc in procs)
            {
                var li = new ListViewItem();
                li.Text = $@"{proc.ProcessName} - {proc.Id}";
                li.Tag = proc;
                listProcesses.Items.Add(li);
            }

            listProcesses.Sorting = SortOrder.Ascending;
            listProcesses.Sort();

            {
                var pid = Extensions.FindProcessId();
                if (!pid.HasValue)
                    throw new Exception("The Spotify process couldn't be found!");

                var proc = listProcesses.Items.Cast<ListViewItem>()
                    .FirstOrDefault(item => (item.Tag as Process)?.Id == pid.Value);
                var processIndex = proc?.Index;

                if (!processIndex.HasValue)
                    throw new Exception($"Couldn't find any process in the list with PID: '{pid.Value}'!");

                listProcesses.Items[processIndex.Value].Selected = true;

                Text += $@" [Selected PID {pid.Value} | {(proc.Tag as Process)?.MainWindowTitle}]";
            }
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
            var selectedItem = listProcesses.SelectedItems[0];

            var selectedProcess = (Process) selectedItem.Tag;

            try
            {
                lblModuleBase.Text = selectedProcess.MainModule?.BaseAddress.ToString("X");
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
                MessageBox.Show("You must select one process target", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var targetProcess = (Process) listProcesses.SelectedItems[0].Tag;

            var domainFilePath = Application.StartupPath; //Set the directory containing the dll
            var payloadFilePath = Application.StartupPath;

            var injector = new PayloadInjector(targetProcess, domainFilePath, textboxDomain.Text, payloadFilePath,
                textboxPayload.Text, textboxArgs.Text);
            injector.InjectAndForget();
        }
    }
}