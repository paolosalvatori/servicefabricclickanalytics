using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.AzureCat.Samples.UserEmulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.mainHeaderPanel = new Microsoft.AzureCat.Samples.UserEmulator.HeaderPanel();
            this.grouperServiceEndpoint = new Microsoft.AzureCat.Samples.UserEmulator.Grouper();
            this.cboServiceEndpointUrl = new System.Windows.Forms.ComboBox();
            this.lblServiceEndpointUrl = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.grouperUserSessions = new Microsoft.AzureCat.Samples.UserEmulator.Grouper();
            this.lblEventsPerUserSessionValue = new System.Windows.Forms.Label();
            this.lblEventsPerUserSession = new System.Windows.Forms.Label();
            this.trackbarEventsPerUserSession = new Microsoft.AzureCat.Samples.UserEmulator.CustomTrackBar();
            this.lblEventIntervalInMilliseconds = new System.Windows.Forms.Label();
            this.txtEventIntervalInMilliseconds = new Microsoft.AzureCat.Samples.UserEmulator.NumericTextBox();
            this.lblUserCount = new System.Windows.Forms.Label();
            this.txtUserCount = new Microsoft.AzureCat.Samples.UserEmulator.NumericTextBox();
            this.logHeaderPanel = new Microsoft.AzureCat.Samples.UserEmulator.HeaderPanel();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainMenuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.mainHeaderPanel.SuspendLayout();
            this.grouperServiceEndpoint.SuspendLayout();
            this.grouperUserSessions.SuspendLayout();
            this.logHeaderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(864, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem,
            this.saveLogToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveLogToolStripMenuItem.Text = "Save Log As...";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logWindowToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // logWindowToolStripMenuItem
            // 
            this.logWindowToolStripMenuItem.Checked = true;
            this.logWindowToolStripMenuItem.CheckOnClick = true;
            this.logWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logWindowToolStripMenuItem.Name = "logWindowToolStripMenuItem";
            this.logWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.logWindowToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.logWindowToolStripMenuItem.Text = "&Log Window";
            this.logWindowToolStripMenuItem.Click += new System.EventHandler(this.logWindowToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(864, 22);
            this.statusStrip.TabIndex = 20;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(16, 40);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mainHeaderPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.logHeaderPanel);
            this.splitContainer.Size = new System.Drawing.Size(832, 496);
            this.splitContainer.SplitterDistance = 288;
            this.splitContainer.SplitterWidth = 8;
            this.splitContainer.TabIndex = 21;
            // 
            // mainHeaderPanel
            // 
            this.mainHeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.mainHeaderPanel.Controls.Add(this.grouperServiceEndpoint);
            this.mainHeaderPanel.Controls.Add(this.btnClear);
            this.mainHeaderPanel.Controls.Add(this.btnStart);
            this.mainHeaderPanel.Controls.Add(this.grouperUserSessions);
            this.mainHeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainHeaderPanel.ForeColor = System.Drawing.Color.White;
            this.mainHeaderPanel.HeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.mainHeaderPanel.HeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.mainHeaderPanel.HeaderFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.mainHeaderPanel.HeaderHeight = 24;
            this.mainHeaderPanel.HeaderText = "Configuration";
            this.mainHeaderPanel.Icon = global::Microsoft.AzureCat.Samples.UserEmulator.Properties.Resources.SmallWorld;
            this.mainHeaderPanel.IconTransparentColor = System.Drawing.Color.White;
            this.mainHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.mainHeaderPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.mainHeaderPanel.Name = "mainHeaderPanel";
            this.mainHeaderPanel.Padding = new System.Windows.Forms.Padding(5, 28, 5, 4);
            this.mainHeaderPanel.Size = new System.Drawing.Size(832, 288);
            this.mainHeaderPanel.TabIndex = 0;
            // 
            // grouperServiceEndpoint
            // 
            this.grouperServiceEndpoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grouperServiceEndpoint.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.grouperServiceEndpoint.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouperServiceEndpoint.BackgroundGradientMode = Microsoft.AzureCat.Samples.UserEmulator.Grouper.GroupBoxGradientMode.None;
            this.grouperServiceEndpoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.grouperServiceEndpoint.BorderThickness = 1F;
            this.grouperServiceEndpoint.Controls.Add(this.cboServiceEndpointUrl);
            this.grouperServiceEndpoint.Controls.Add(this.lblServiceEndpointUrl);
            this.grouperServiceEndpoint.CustomGroupBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.grouperServiceEndpoint.GroupImage = null;
            this.grouperServiceEndpoint.GroupTitle = "Service Endpoint";
            this.grouperServiceEndpoint.Location = new System.Drawing.Point(16, 32);
            this.grouperServiceEndpoint.Name = "grouperServiceEndpoint";
            this.grouperServiceEndpoint.Padding = new System.Windows.Forms.Padding(20);
            this.grouperServiceEndpoint.PaintGroupBox = true;
            this.grouperServiceEndpoint.RoundCorners = 4;
            this.grouperServiceEndpoint.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperServiceEndpoint.ShadowControl = false;
            this.grouperServiceEndpoint.ShadowThickness = 3;
            this.grouperServiceEndpoint.Size = new System.Drawing.Size(800, 88);
            this.grouperServiceEndpoint.TabIndex = 159;
            this.grouperServiceEndpoint.CustomPaint += new System.Action<System.Windows.Forms.PaintEventArgs>(this.grouperDeviceManagement_CustomPaint);
            // 
            // cboServiceEndpointUrl
            // 
            this.cboServiceEndpointUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboServiceEndpointUrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServiceEndpointUrl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboServiceEndpointUrl.FormattingEnabled = true;
            this.cboServiceEndpointUrl.Location = new System.Drawing.Point(16, 48);
            this.cboServiceEndpointUrl.Name = "cboServiceEndpointUrl";
            this.cboServiceEndpointUrl.Size = new System.Drawing.Size(768, 21);
            this.cboServiceEndpointUrl.TabIndex = 94;
            // 
            // lblServiceEndpointUrl
            // 
            this.lblServiceEndpointUrl.AutoSize = true;
            this.lblServiceEndpointUrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblServiceEndpointUrl.Location = new System.Drawing.Point(16, 32);
            this.lblServiceEndpointUrl.Name = "lblServiceEndpointUrl";
            this.lblServiceEndpointUrl.Size = new System.Drawing.Size(32, 13);
            this.lblServiceEndpointUrl.TabIndex = 46;
            this.lblServiceEndpointUrl.Text = "URL:";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClear.Location = new System.Drawing.Point(624, 248);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 24);
            this.btnClear.TabIndex = 163;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.btnClear.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStart.Location = new System.Drawing.Point(728, 248);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(88, 24);
            this.btnStart.TabIndex = 162;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.btnStart.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // grouperUserSessions
            // 
            this.grouperUserSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grouperUserSessions.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.grouperUserSessions.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouperUserSessions.BackgroundGradientMode = Microsoft.AzureCat.Samples.UserEmulator.Grouper.GroupBoxGradientMode.None;
            this.grouperUserSessions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.grouperUserSessions.BorderThickness = 1F;
            this.grouperUserSessions.Controls.Add(this.lblEventsPerUserSessionValue);
            this.grouperUserSessions.Controls.Add(this.lblEventsPerUserSession);
            this.grouperUserSessions.Controls.Add(this.trackbarEventsPerUserSession);
            this.grouperUserSessions.Controls.Add(this.lblEventIntervalInMilliseconds);
            this.grouperUserSessions.Controls.Add(this.txtEventIntervalInMilliseconds);
            this.grouperUserSessions.Controls.Add(this.lblUserCount);
            this.grouperUserSessions.Controls.Add(this.txtUserCount);
            this.grouperUserSessions.CustomGroupBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.grouperUserSessions.GroupImage = null;
            this.grouperUserSessions.GroupTitle = "User Sessions";
            this.grouperUserSessions.Location = new System.Drawing.Point(16, 128);
            this.grouperUserSessions.Name = "grouperUserSessions";
            this.grouperUserSessions.Padding = new System.Windows.Forms.Padding(20);
            this.grouperUserSessions.PaintGroupBox = true;
            this.grouperUserSessions.RoundCorners = 4;
            this.grouperUserSessions.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperUserSessions.ShadowControl = false;
            this.grouperUserSessions.ShadowThickness = 3;
            this.grouperUserSessions.Size = new System.Drawing.Size(800, 104);
            this.grouperUserSessions.TabIndex = 161;
            // 
            // lblEventsPerUserSessionValue
            // 
            this.lblEventsPerUserSessionValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventsPerUserSessionValue.AutoSize = true;
            this.lblEventsPerUserSessionValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEventsPerUserSessionValue.Location = new System.Drawing.Point(760, 52);
            this.lblEventsPerUserSessionValue.Name = "lblEventsPerUserSessionValue";
            this.lblEventsPerUserSessionValue.Size = new System.Drawing.Size(0, 13);
            this.lblEventsPerUserSessionValue.TabIndex = 163;
            // 
            // lblEventsPerUserSession
            // 
            this.lblEventsPerUserSession.AutoSize = true;
            this.lblEventsPerUserSession.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEventsPerUserSession.Location = new System.Drawing.Point(400, 32);
            this.lblEventsPerUserSession.Name = "lblEventsPerUserSession";
            this.lblEventsPerUserSession.Size = new System.Drawing.Size(127, 13);
            this.lblEventsPerUserSession.TabIndex = 162;
            this.lblEventsPerUserSession.Text = "Events Per User Session:";
            // 
            // trackbarEventsPerUserSession
            // 
            this.trackbarEventsPerUserSession.BackColor = System.Drawing.Color.Transparent;
            this.trackbarEventsPerUserSession.BorderColor = System.Drawing.Color.Black;
            this.trackbarEventsPerUserSession.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackbarEventsPerUserSession.ForeColor = System.Drawing.Color.Black;
            this.trackbarEventsPerUserSession.IndentHeight = 6;
            this.trackbarEventsPerUserSession.Location = new System.Drawing.Point(392, 44);
            this.trackbarEventsPerUserSession.Maximum = 100;
            this.trackbarEventsPerUserSession.Minimum = 0;
            this.trackbarEventsPerUserSession.Name = "trackbarEventsPerUserSession";
            this.trackbarEventsPerUserSession.Size = new System.Drawing.Size(360, 47);
            this.trackbarEventsPerUserSession.TabIndex = 161;
            this.trackbarEventsPerUserSession.TickColor = System.Drawing.Color.Gray;
            this.trackbarEventsPerUserSession.TickFrequency = 10;
            this.trackbarEventsPerUserSession.TickHeight = 4;
            this.trackbarEventsPerUserSession.TrackerColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(130)))), ((int)(((byte)(198)))));
            this.trackbarEventsPerUserSession.TrackerSize = new System.Drawing.Size(16, 16);
            this.trackbarEventsPerUserSession.TrackLineBrushStyle = Microsoft.AzureCat.Samples.UserEmulator.BrushStyle.LinearGradient;
            this.trackbarEventsPerUserSession.TrackLineColor = System.Drawing.Color.Black;
            this.trackbarEventsPerUserSession.TrackLineHeight = 1;
            this.trackbarEventsPerUserSession.Value = 0;
            this.trackbarEventsPerUserSession.ValueChanged += new Microsoft.AzureCat.Samples.UserEmulator.ValueChangedHandler(this.trackbarEventsPerUserSession_ValueChanged);
            // 
            // lblEventIntervalInMilliseconds
            // 
            this.lblEventIntervalInMilliseconds.AutoSize = true;
            this.lblEventIntervalInMilliseconds.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEventIntervalInMilliseconds.Location = new System.Drawing.Point(208, 32);
            this.lblEventIntervalInMilliseconds.Name = "lblEventIntervalInMilliseconds";
            this.lblEventIntervalInMilliseconds.Size = new System.Drawing.Size(142, 13);
            this.lblEventIntervalInMilliseconds.TabIndex = 61;
            this.lblEventIntervalInMilliseconds.Text = "Event Interval In Millisconds:";
            // 
            // txtEventIntervalInMilliseconds
            // 
            this.txtEventIntervalInMilliseconds.AllowSpace = false;
            this.txtEventIntervalInMilliseconds.Location = new System.Drawing.Point(208, 48);
            this.txtEventIntervalInMilliseconds.Name = "txtEventIntervalInMilliseconds";
            this.txtEventIntervalInMilliseconds.Size = new System.Drawing.Size(176, 20);
            this.txtEventIntervalInMilliseconds.TabIndex = 55;
            // 
            // lblUserCount
            // 
            this.lblUserCount.AutoSize = true;
            this.lblUserCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUserCount.Location = new System.Drawing.Point(16, 32);
            this.lblUserCount.Name = "lblUserCount";
            this.lblUserCount.Size = new System.Drawing.Size(63, 13);
            this.lblUserCount.TabIndex = 58;
            this.lblUserCount.Text = "User Count:";
            // 
            // txtUserCount
            // 
            this.txtUserCount.AllowSpace = false;
            this.txtUserCount.Location = new System.Drawing.Point(16, 48);
            this.txtUserCount.Name = "txtUserCount";
            this.txtUserCount.Size = new System.Drawing.Size(176, 20);
            this.txtUserCount.TabIndex = 54;
            // 
            // logHeaderPanel
            // 
            this.logHeaderPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.logHeaderPanel.Controls.Add(this.lstLog);
            this.logHeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logHeaderPanel.ForeColor = System.Drawing.Color.White;
            this.logHeaderPanel.HeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.logHeaderPanel.HeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.logHeaderPanel.HeaderFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.logHeaderPanel.HeaderHeight = 24;
            this.logHeaderPanel.HeaderText = "Log";
            this.logHeaderPanel.Icon = global::Microsoft.AzureCat.Samples.UserEmulator.Properties.Resources.SmallDocument;
            this.logHeaderPanel.IconTransparentColor = System.Drawing.Color.White;
            this.logHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.logHeaderPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.logHeaderPanel.Name = "logHeaderPanel";
            this.logHeaderPanel.Padding = new System.Windows.Forms.Padding(5, 28, 5, 4);
            this.logHeaderPanel.Size = new System.Drawing.Size(832, 200);
            this.logHeaderPanel.TabIndex = 0;
            // 
            // lstLog
            // 
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(5, 28);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(822, 168);
            this.lstLog.TabIndex = 0;
            this.lstLog.Leave += new System.EventHandler(this.lstLog_Leave);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(864, 561);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Emulator";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.mainHeaderPanel.ResumeLayout(false);
            this.grouperServiceEndpoint.ResumeLayout(false);
            this.grouperServiceEndpoint.PerformLayout();
            this.grouperUserSessions.ResumeLayout(false);
            this.grouperUserSessions.PerformLayout();
            this.logHeaderPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem clearLogToolStripMenuItem;
        private ToolStripMenuItem saveLogToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem logWindowToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private SplitContainer splitContainer;
        private HeaderPanel logHeaderPanel;
        private ListBox lstLog;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        private HeaderPanel mainHeaderPanel;
        private Grouper grouperUserSessions;
        private Label lblEventsPerUserSession;
        private CustomTrackBar trackbarEventsPerUserSession;
        private Label lblEventIntervalInMilliseconds;
        private NumericTextBox txtEventIntervalInMilliseconds;
        private Label lblUserCount;
        private NumericTextBox txtUserCount;
        private Button btnClear;
        private Button btnStart;
        private Grouper grouperServiceEndpoint;
        private Label lblServiceEndpointUrl;
        private ComboBox cboServiceEndpointUrl;
        private Label lblEventsPerUserSessionValue;
    }
}