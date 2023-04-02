using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using static System.Net.Mime.MediaTypeNames;

namespace Emu
{
    public partial class Emu : Form
    {
        private readonly object stepLock = new object();
        private readonly EmuService emuService;

        public Emu(EmuService emuService)
        {
            InitializeComponent();
            this.emuService = emuService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            var folderName = $"Backup_{DateTime.Now.ToString("yyyyMMdd")}";
            var fullPath = textBox1.Text + "\\" + folderName;

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }

            Directory.CreateDirectory(fullPath);

            var progress = new Progress<int>(i =>
            {
                progressBar1.Value += i;
            });

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = emuService.GetProgressMaxValue() + 1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            emuService.SaveProgamsAndUpdates(fullPath);

            var backupDefaultFoldersCompletionTask = emuService.BackupDefaultFolders(fullPath, progress);            

            var backupGameDataCompletionTask = emuService.BackupGameData(fullPath, progress);            

            await Task.WhenAll(backupDefaultFoldersCompletionTask, backupGameDataCompletionTask);
        }

        private void addFileRadio_CheckedChanged(object sender, EventArgs e)
        {
            button2.Text = "Select File";
            addFolderFileButton.Text = "Add File";
            recursiveBox.Hide();
            textBox2.Clear();
        }

        private void addFolderRadio_CheckedChanged(object sender, EventArgs e)
        {
            button2.Text = "Select Folder";
            addFolderFileButton.Text = "Add Folder";
            recursiveBox.Show();
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (addFolderRadio.Checked)
            {
                var dialog = new FolderBrowserDialog();

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textBox2.Text = dialog.SelectedPath;
                }
            }
            else
            {
                var dialog = new OpenFileDialog();
                dialog.Multiselect = false;

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textBox2.Text = dialog.FileName;
                }
            }
        }

        private void addFolderFileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                return;
            }

            if (addFolderRadio.Checked)
            {
                dataGridView1.Rows.Add(textBox2.Text, recursiveBox.Checked ? "True" : "False");
            }
            else
            {
                dataGridView1.Rows.Add(textBox2.Text, "N/A");
            }

            textBox2.Clear();
        }
    }
}