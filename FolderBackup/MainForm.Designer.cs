namespace FolderBackup
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            btnTargetFolder = new Button();
            label2 = new Label();
            btnSaveFolder = new Button();
            label3 = new Label();
            fsWatcher = new FileSystemWatcher();
            numWaitSec = new NumericUpDown();
            btnStart = new Button();
            chkAutoStart = new CheckBox();
            dlgSelectFolder = new FolderBrowserDialog();
            lblTargetFolder = new Label();
            lblSaveFolder = new Label();
            tmrSave = new System.Windows.Forms.Timer(components);
            label4 = new Label();
            numInterval = new NumericUpDown();
            chkUse7z = new CheckBox();
            btnForce = new Button();
            label5 = new Label();
            cmbSection = new ComboBox();
            btnAddApplication = new Button();
            fsWatcherSub = new FileSystemWatcher();
            lblTargetFolderSub = new Label();
            btnTargetFolderSub = new Button();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)fsWatcher).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWaitSec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fsWatcherSub).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label1.Location = new Point(12, 44);
            label1.Name = "label1";
            label1.Size = new Size(111, 25);
            label1.TabIndex = 0;
            label1.Text = "TargetFolder";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnTargetFolder
            // 
            btnTargetFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTargetFolder.Location = new Point(443, 43);
            btnTargetFolder.Name = "btnTargetFolder";
            btnTargetFolder.Size = new Size(29, 23);
            btnTargetFolder.TabIndex = 2;
            btnTargetFolder.Text = "...";
            btnTargetFolder.UseVisualStyleBackColor = true;
            btnTargetFolder.Click += btnTargetFolder_Click;
            // 
            // label2
            // 
            label2.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label2.Location = new Point(12, 104);
            label2.Name = "label2";
            label2.Size = new Size(111, 25);
            label2.TabIndex = 3;
            label2.Text = "SaveFolder";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnSaveFolder
            // 
            btnSaveFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveFolder.Location = new Point(443, 104);
            btnSaveFolder.Name = "btnSaveFolder";
            btnSaveFolder.Size = new Size(29, 23);
            btnSaveFolder.TabIndex = 5;
            btnSaveFolder.Text = "...";
            btnSaveFolder.UseVisualStyleBackColor = true;
            btnSaveFolder.Click += btnSaveFolder_Click;
            // 
            // label3
            // 
            label3.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label3.Location = new Point(12, 134);
            label3.Name = "label3";
            label3.Size = new Size(111, 25);
            label3.TabIndex = 6;
            label3.Text = "Wait(sec)";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // fsWatcher
            // 
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.SynchronizingObject = this;
            fsWatcher.Changed += fsWatcher_Changed;
            fsWatcher.Created += fsWatcher_Changed;
            fsWatcher.Deleted += fsWatcher_Changed;
            fsWatcher.Renamed += fsWatcher_Renamed;
            // 
            // numWaitSec
            // 
            numWaitSec.Location = new Point(129, 134);
            numWaitSec.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWaitSec.Name = "numWaitSec";
            numWaitSec.Size = new Size(59, 23);
            numWaitSec.TabIndex = 7;
            numWaitSec.TextAlign = HorizontalAlignment.Right;
            numWaitSec.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numWaitSec.ValueChanged += numWaitSec_ValueChanged;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnStart.Location = new Point(394, 164);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(78, 25);
            btnStart.TabIndex = 8;
            btnStart.Text = "監視開始";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // chkAutoStart
            // 
            chkAutoStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkAutoStart.AutoSize = true;
            chkAutoStart.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            chkAutoStart.Location = new Point(293, 164);
            chkAutoStart.Name = "chkAutoStart";
            chkAutoStart.Size = new Size(95, 25);
            chkAutoStart.TabIndex = 9;
            chkAutoStart.Text = "AutoStart";
            chkAutoStart.UseVisualStyleBackColor = true;
            chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;
            // 
            // lblTargetFolder
            // 
            lblTargetFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTargetFolder.AutoEllipsis = true;
            lblTargetFolder.BorderStyle = BorderStyle.FixedSingle;
            lblTargetFolder.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblTargetFolder.Location = new Point(129, 44);
            lblTargetFolder.Name = "lblTargetFolder";
            lblTargetFolder.Size = new Size(308, 22);
            lblTargetFolder.TabIndex = 10;
            lblTargetFolder.DoubleClick += folderLabel_DoubleClick;
            // 
            // lblSaveFolder
            // 
            lblSaveFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSaveFolder.AutoEllipsis = true;
            lblSaveFolder.BorderStyle = BorderStyle.FixedSingle;
            lblSaveFolder.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblSaveFolder.Location = new Point(129, 105);
            lblSaveFolder.Name = "lblSaveFolder";
            lblSaveFolder.Size = new Size(308, 22);
            lblSaveFolder.TabIndex = 11;
            lblSaveFolder.DoubleClick += folderLabel_DoubleClick;
            // 
            // tmrSave
            // 
            tmrSave.Interval = 500;
            tmrSave.Tick += tmrSave_Tick;
            // 
            // label4
            // 
            label4.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label4.Location = new Point(12, 159);
            label4.Name = "label4";
            label4.Size = new Size(111, 25);
            label4.TabIndex = 12;
            label4.Text = "Interval(min)";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numInterval
            // 
            numInterval.Location = new Point(129, 164);
            numInterval.Name = "numInterval";
            numInterval.Size = new Size(59, 23);
            numInterval.TabIndex = 13;
            numInterval.TextAlign = HorizontalAlignment.Right;
            numInterval.ValueChanged += numInterval_ValueChanged;
            // 
            // chkUse7z
            // 
            chkUse7z.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkUse7z.AutoSize = true;
            chkUse7z.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            chkUse7z.Location = new Point(293, 132);
            chkUse7z.Name = "chkUse7z";
            chkUse7z.Size = new Size(93, 25);
            chkUse7z.TabIndex = 14;
            chkUse7z.Text = "use 7z.dll";
            chkUse7z.UseVisualStyleBackColor = true;
            chkUse7z.CheckedChanged += chkUse7z_CheckedChanged;
            // 
            // btnForce
            // 
            btnForce.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnForce.Location = new Point(394, 133);
            btnForce.Name = "btnForce";
            btnForce.Size = new Size(78, 25);
            btnForce.TabIndex = 15;
            btnForce.Text = "即時Backup";
            btnForce.UseVisualStyleBackColor = true;
            btnForce.Click += btnForce_Click;
            // 
            // label5
            // 
            label5.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label5.Location = new Point(12, 9);
            label5.Name = "label5";
            label5.Size = new Size(111, 25);
            label5.TabIndex = 16;
            label5.Text = "Application";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbSection
            // 
            cmbSection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSection.FormattingEnabled = true;
            cmbSection.Location = new Point(129, 9);
            cmbSection.Name = "cmbSection";
            cmbSection.Size = new Size(308, 23);
            cmbSection.TabIndex = 17;
            cmbSection.SelectedIndexChanged += cmbSection_SelectedIndexChanged;
            // 
            // btnAddApplication
            // 
            btnAddApplication.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddApplication.Location = new Point(443, 9);
            btnAddApplication.Name = "btnAddApplication";
            btnAddApplication.Size = new Size(29, 23);
            btnAddApplication.TabIndex = 18;
            btnAddApplication.Text = "...";
            btnAddApplication.UseVisualStyleBackColor = true;
            btnAddApplication.Click += btnAddApplication_Click;
            // 
            // fsWatcherSub
            // 
            fsWatcherSub.EnableRaisingEvents = true;
            fsWatcherSub.IncludeSubdirectories = true;
            fsWatcherSub.SynchronizingObject = this;
            fsWatcherSub.Changed += fsWatcher_Changed;
            fsWatcherSub.Created += fsWatcher_Changed;
            fsWatcherSub.Deleted += fsWatcher_Changed;
            fsWatcherSub.Renamed += fsWatcher_Renamed;
            // 
            // lblTargetFolderSub
            // 
            lblTargetFolderSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTargetFolderSub.AutoEllipsis = true;
            lblTargetFolderSub.BorderStyle = BorderStyle.FixedSingle;
            lblTargetFolderSub.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblTargetFolderSub.Location = new Point(129, 74);
            lblTargetFolderSub.Name = "lblTargetFolderSub";
            lblTargetFolderSub.Size = new Size(308, 22);
            lblTargetFolderSub.TabIndex = 21;
            lblTargetFolderSub.DoubleClick += folderLabel_DoubleClick;
            // 
            // btnTargetFolderSub
            // 
            btnTargetFolderSub.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTargetFolderSub.Location = new Point(443, 73);
            btnTargetFolderSub.Name = "btnTargetFolderSub";
            btnTargetFolderSub.Size = new Size(29, 23);
            btnTargetFolderSub.TabIndex = 20;
            btnTargetFolderSub.Text = "...";
            btnTargetFolderSub.UseVisualStyleBackColor = true;
            btnTargetFolderSub.Click += btnTargetFolderSub_Click;
            // 
            // label7
            // 
            label7.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label7.Location = new Point(12, 74);
            label7.Name = "label7";
            label7.Size = new Size(111, 25);
            label7.TabIndex = 19;
            label7.Text = "Sub";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 201);
            Controls.Add(lblTargetFolderSub);
            Controls.Add(btnTargetFolderSub);
            Controls.Add(label7);
            Controls.Add(btnAddApplication);
            Controls.Add(cmbSection);
            Controls.Add(label5);
            Controls.Add(btnForce);
            Controls.Add(chkUse7z);
            Controls.Add(numInterval);
            Controls.Add(label4);
            Controls.Add(lblSaveFolder);
            Controls.Add(lblTargetFolder);
            Controls.Add(chkAutoStart);
            Controls.Add(btnStart);
            Controls.Add(numWaitSec);
            Controls.Add(label3);
            Controls.Add(btnSaveFolder);
            Controls.Add(label2);
            Controls.Add(btnTargetFolder);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(9999, 240);
            MinimumSize = new Size(500, 240);
            Name = "MainForm";
            Text = "FolderBackup";
            FormClosing += MainForm_FormClosing;
            LocationChanged += MainForm_LocationChanged;
            Resize += MainForm_LocationChanged;
            ((System.ComponentModel.ISupportInitialize)fsWatcher).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWaitSec).EndInit();
            ((System.ComponentModel.ISupportInitialize)numInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)fsWatcherSub).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnTargetFolder;
        private Label label2;
        private Button btnSaveFolder;
        private Label label3;
        private FileSystemWatcher fsWatcher;
        private NumericUpDown numWaitSec;
        private Button btnStart;
        private CheckBox chkAutoStart;
        private FolderBrowserDialog dlgSelectFolder;
        private Label lblSaveFolder;
        private Label lblTargetFolder;
        private System.Windows.Forms.Timer tmrSave;
        private NumericUpDown numInterval;
        private Label label4;
        private Button btnForce;
        private CheckBox chkUse7z;
        private Label label5;
        private ComboBox cmbSection;
        private Button btnAddApplication;
        private FileSystemWatcher fsWatcherSub;
        private Label lblTargetFolderSub;
        private Button btnTargetFolderSub;
        private Label label7;
    }
}