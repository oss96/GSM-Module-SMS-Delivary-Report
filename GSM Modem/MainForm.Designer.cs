namespace GSM_Modem {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConnect = new System.Windows.Forms.Button();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.btnListSMS = new System.Windows.Forms.Button();
            this.btnRefreshPorts = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.programStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.copyStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBoxAutoRefresh = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.dataGridViewSMS = new System.Windows.Forms.DataGridView();
            this.ColumnCheckBoxCell = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPhoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDischarge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnResponse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSMS)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(960, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 27);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(93, 28);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(198, 21);
            this.comboBoxComPort.TabIndex = 3;
            // 
            // btnListSMS
            // 
            this.btnListSMS.Location = new System.Drawing.Point(378, 26);
            this.btnListSMS.Name = "btnListSMS";
            this.btnListSMS.Size = new System.Drawing.Size(75, 23);
            this.btnListSMS.TabIndex = 4;
            this.btnListSMS.Text = "List SMS";
            this.btnListSMS.UseVisualStyleBackColor = true;
            this.btnListSMS.Click += new System.EventHandler(this.btnListSMS_Click);
            // 
            // btnRefreshPorts
            // 
            this.btnRefreshPorts.Location = new System.Drawing.Point(297, 27);
            this.btnRefreshPorts.Name = "btnRefreshPorts";
            this.btnRefreshPorts.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshPorts.TabIndex = 5;
            this.btnRefreshPorts.Text = "Refresh";
            this.btnRefreshPorts.UseVisualStyleBackColor = true;
            this.btnRefreshPorts.Click += new System.EventHandler(this.btnRefreshports_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programStatus,
            this.copyStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(960, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // programStatus
            // 
            this.programStatus.Name = "programStatus";
            this.programStatus.Size = new System.Drawing.Size(45, 17);
            this.programStatus.Text = "Status: ";
            // 
            // copyStatus
            // 
            this.copyStatus.Name = "copyStatus";
            this.copyStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // checkBoxAutoRefresh
            // 
            this.checkBoxAutoRefresh.AutoSize = true;
            this.checkBoxAutoRefresh.Location = new System.Drawing.Point(459, 30);
            this.checkBoxAutoRefresh.Name = "checkBoxAutoRefresh";
            this.checkBoxAutoRefresh.Size = new System.Drawing.Size(88, 17);
            this.checkBoxAutoRefresh.TabIndex = 7;
            this.checkBoxAutoRefresh.Text = "Auto Refresh";
            this.checkBoxAutoRefresh.UseVisualStyleBackColor = true;
            this.checkBoxAutoRefresh.CheckedChanged += new System.EventHandler(this.checkBoxAutoRefresh_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(873, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear Table";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dataGridViewSMS
            // 
            this.dataGridViewSMS.AllowUserToAddRows = false;
            this.dataGridViewSMS.AllowUserToDeleteRows = false;
            this.dataGridViewSMS.AllowUserToResizeRows = false;
            this.dataGridViewSMS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSMS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSMS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSMS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCheckBoxCell,
            this.ColumnID,
            this.ColumnPhoneNumber,
            this.ColumnStatus,
            this.ColumnTimeStamp,
            this.ColumnDischarge,
            this.ColumnResponse});
            this.dataGridViewSMS.Location = new System.Drawing.Point(12, 58);
            this.dataGridViewSMS.Name = "dataGridViewSMS";
            this.dataGridViewSMS.ReadOnly = true;
            this.dataGridViewSMS.RowHeadersVisible = false;
            this.dataGridViewSMS.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewSMS.Size = new System.Drawing.Size(936, 385);
            this.dataGridViewSMS.TabIndex = 9;
            this.dataGridViewSMS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSMS_CellClick);
            this.dataGridViewSMS.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSMS_CellValueChanged);
            this.dataGridViewSMS.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewSMS_ColumnHeaderMouseClick);
            this.dataGridViewSMS.SelectionChanged += new System.EventHandler(this.dataGridViewSMS_SelectionChanged);
            // 
            // ColumnCheckBoxCell
            // 
            this.ColumnCheckBoxCell.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnCheckBoxCell.HeaderText = "";
            this.ColumnCheckBoxCell.Name = "ColumnCheckBoxCell";
            this.ColumnCheckBoxCell.ReadOnly = true;
            this.ColumnCheckBoxCell.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnCheckBoxCell.Width = 5;
            // 
            // ColumnID
            // 
            this.ColumnID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnID.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnID.HeaderText = "ID";
            this.ColumnID.Name = "ColumnID";
            this.ColumnID.ReadOnly = true;
            this.ColumnID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnID.Width = 43;
            // 
            // ColumnPhoneNumber
            // 
            this.ColumnPhoneNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnPhoneNumber.HeaderText = "Phone Number";
            this.ColumnPhoneNumber.Name = "ColumnPhoneNumber";
            this.ColumnPhoneNumber.ReadOnly = true;
            this.ColumnPhoneNumber.Width = 95;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Width = 62;
            // 
            // ColumnTimeStamp
            // 
            this.ColumnTimeStamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnTimeStamp.HeaderText = "Time Stamp";
            this.ColumnTimeStamp.Name = "ColumnTimeStamp";
            this.ColumnTimeStamp.ReadOnly = true;
            this.ColumnTimeStamp.Width = 81;
            // 
            // ColumnDischarge
            // 
            this.ColumnDischarge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnDischarge.HeaderText = "Discharge";
            this.ColumnDischarge.Name = "ColumnDischarge";
            this.ColumnDischarge.ReadOnly = true;
            this.ColumnDischarge.Width = 80;
            // 
            // ColumnResponse
            // 
            this.ColumnResponse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnResponse.HeaderText = "Response";
            this.ColumnResponse.Name = "ColumnResponse";
            this.ColumnResponse.ReadOnly = true;
            this.ColumnResponse.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(553, 26);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(124, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete selected entries";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(960, 468);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dataGridViewSMS);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.checkBoxAutoRefresh);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnRefreshPorts);
            this.Controls.Add(this.btnListSMS);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "GSM Delivery Reporter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSMS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Button btnListSMS;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel programStatus;
        private System.Windows.Forms.CheckBox checkBoxAutoRefresh;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dataGridViewSMS;
        private System.Windows.Forms.ToolStripStatusLabel copyStatus;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnCheckBoxCell;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPhoneNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDischarge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnResponse;
    }
}

