namespace Emu
{
    partial class Emu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backupFolder = new Button();
            textBox1 = new TextBox();
            button1 = new Button();
            progressBar1 = new ProgressBar();
            button2 = new Button();
            textBox2 = new TextBox();
            addFolderRadio = new RadioButton();
            addFileRadio = new RadioButton();
            recursiveBox = new CheckBox();
            addFolderFileButton = new Button();
            dataGridView1 = new DataGridView();
            FolderFilePath = new DataGridViewTextBoxColumn();
            Recursive = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // backupFolder
            // 
            backupFolder.Location = new Point(12, 12);
            backupFolder.Name = "backupFolder";
            backupFolder.Size = new Size(149, 23);
            backupFolder.TabIndex = 0;
            backupFolder.Text = "Select Backup Folder";
            backupFolder.UseVisualStyleBackColor = true;
            backupFolder.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(167, 12);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(371, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(12, 415);
            button1.Name = "button1";
            button1.Size = new Size(149, 23);
            button1.TabIndex = 2;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(167, 415);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(371, 23);
            progressBar1.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(12, 98);
            button2.Name = "button2";
            button2.Size = new Size(149, 23);
            button2.TabIndex = 4;
            button2.Text = "Select Folder";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(167, 99);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(371, 23);
            textBox2.TabIndex = 5;
            // 
            // addFolderRadio
            // 
            addFolderRadio.AutoSize = true;
            addFolderRadio.Checked = true;
            addFolderRadio.Location = new Point(12, 73);
            addFolderRadio.Name = "addFolderRadio";
            addFolderRadio.Size = new Size(83, 19);
            addFolderRadio.TabIndex = 6;
            addFolderRadio.TabStop = true;
            addFolderRadio.Text = "Add Folder";
            addFolderRadio.UseVisualStyleBackColor = true;
            addFolderRadio.CheckedChanged += addFolderRadio_CheckedChanged;
            // 
            // addFileRadio
            // 
            addFileRadio.AutoSize = true;
            addFileRadio.Location = new Point(101, 73);
            addFileRadio.Name = "addFileRadio";
            addFileRadio.Size = new Size(68, 19);
            addFileRadio.TabIndex = 7;
            addFileRadio.TabStop = true;
            addFileRadio.Text = "Add File";
            addFileRadio.UseVisualStyleBackColor = true;
            addFileRadio.CheckedChanged += addFileRadio_CheckedChanged;
            // 
            // recursiveBox
            // 
            recursiveBox.AutoSize = true;
            recursiveBox.Location = new Point(167, 128);
            recursiveBox.Name = "recursiveBox";
            recursiveBox.Size = new Size(76, 19);
            recursiveBox.TabIndex = 8;
            recursiveBox.Text = "Recursive";
            recursiveBox.UseVisualStyleBackColor = true;
            // 
            // addFolderFileButton
            // 
            addFolderFileButton.Location = new Point(12, 127);
            addFolderFileButton.Name = "addFolderFileButton";
            addFolderFileButton.Size = new Size(149, 23);
            addFolderFileButton.TabIndex = 9;
            addFolderFileButton.Text = "Add Folder";
            addFolderFileButton.UseVisualStyleBackColor = true;
            addFolderFileButton.Click += addFolderFileButton_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { FolderFilePath, Recursive });
            dataGridView1.Location = new Point(12, 156);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(526, 253);
            dataGridView1.TabIndex = 10;
            // 
            // FolderFilePath
            // 
            FolderFilePath.HeaderText = "Folder/File Path";
            FolderFilePath.Name = "FolderFilePath";
            FolderFilePath.ReadOnly = true;
            FolderFilePath.Width = 390;
            // 
            // Recursive
            // 
            Recursive.HeaderText = "Recursive";
            Recursive.Name = "Recursive";
            Recursive.ReadOnly = true;
            Recursive.Width = 90;
            // 
            // Emu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(559, 450);
            Controls.Add(dataGridView1);
            Controls.Add(addFolderFileButton);
            Controls.Add(recursiveBox);
            Controls.Add(addFileRadio);
            Controls.Add(addFolderRadio);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(backupFolder);
            Name = "Emu";
            Text = "Emu";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button backupFolder;
        private TextBox textBox1;
        private Button button1;
        private ProgressBar progressBar1;
        private Button button2;
        private TextBox textBox2;
        private RadioButton addFolderRadio;
        private RadioButton addFileRadio;
        private CheckBox recursiveBox;
        private Button addFolderFileButton;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn FolderFilePath;
        private DataGridViewTextBoxColumn Recursive;
    }
}