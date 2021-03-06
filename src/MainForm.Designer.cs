﻿namespace OstBot_2_
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.groupBoxCode = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button_EnterCode = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox_Connect = new System.Windows.Forms.GroupBox();
            this.checkBox_Reconnect = new System.Windows.Forms.CheckBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.comboBox_WorldId = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_RoomType = new System.Windows.Forms.ComboBox();
            this.groupBox_Login = new System.Windows.Forms.GroupBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.button_Login = new System.Windows.Forms.Button();
            this.comboBox_Email = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_Server = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listBox_PlayerList = new System.Windows.Forms.ListBox();
            this.textBox_ChatText = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.checkedListBox_SubBots = new System.Windows.Forms.CheckedListBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox_Code = new System.Windows.Forms.GroupBox();
            this.numericUpDown_CrackCodeDigits = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_MaxCrackCode = new System.Windows.Forms.NumericUpDown();
            this.progressBar_CodeCracker = new System.Windows.Forms.ProgressBar();
            this.textBox_CrackedCode = new System.Windows.Forms.TextBox();
            this.button_CrackCode = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.labelConsole = new System.Windows.Forms.Label();
            this.textBoxConsoleInput = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxCode.SuspendLayout();
            this.groupBox_Connect.SuspendLayout();
            this.groupBox_Login.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox_Code.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CrackCodeDigits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxCrackCode)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(660, 462);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonRefresh);
            this.tabPage1.Controls.Add(this.groupBoxCode);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.groupBox_Connect);
            this.tabPage1.Controls.Add(this.groupBox_Login);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(652, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(549, 407);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(99, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh rooms";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBoxCode
            // 
            this.groupBoxCode.Controls.Add(this.textBox4);
            this.groupBoxCode.Controls.Add(this.button_EnterCode);
            this.groupBoxCode.Location = new System.Drawing.Point(8, 253);
            this.groupBoxCode.Name = "groupBoxCode";
            this.groupBoxCode.Size = new System.Drawing.Size(268, 48);
            this.groupBoxCode.TabIndex = 7;
            this.groupBoxCode.TabStop = false;
            this.groupBoxCode.Text = "Code";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(9, 19);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(172, 20);
            this.textBox4.TabIndex = 5;
            // 
            // button_EnterCode
            // 
            this.button_EnterCode.Location = new System.Drawing.Point(186, 17);
            this.button_EnterCode.Name = "button_EnterCode";
            this.button_EnterCode.Size = new System.Drawing.Size(75, 23);
            this.button_EnterCode.TabIndex = 6;
            this.button_EnterCode.Text = "Enter Code";
            this.button_EnterCode.UseVisualStyleBackColor = true;
            this.button_EnterCode.Click += new System.EventHandler(this.button_EnterCode_Click_1);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(282, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(366, 398);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 115;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "players";
            this.columnHeader2.Width = 46;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Plays";
            this.columnHeader3.Width = 56;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "WorldId";
            this.columnHeader4.Width = 180;
            // 
            // groupBox_Connect
            // 
            this.groupBox_Connect.Controls.Add(this.checkBox_Reconnect);
            this.groupBox_Connect.Controls.Add(this.button_Connect);
            this.groupBox_Connect.Controls.Add(this.comboBox_WorldId);
            this.groupBox_Connect.Controls.Add(this.label5);
            this.groupBox_Connect.Controls.Add(this.label4);
            this.groupBox_Connect.Controls.Add(this.comboBox_RoomType);
            this.groupBox_Connect.Location = new System.Drawing.Point(8, 144);
            this.groupBox_Connect.Name = "groupBox_Connect";
            this.groupBox_Connect.Size = new System.Drawing.Size(268, 103);
            this.groupBox_Connect.TabIndex = 1;
            this.groupBox_Connect.TabStop = false;
            this.groupBox_Connect.Text = "Connect";
            // 
            // checkBox_Reconnect
            // 
            this.checkBox_Reconnect.AutoSize = true;
            this.checkBox_Reconnect.Location = new System.Drawing.Point(9, 73);
            this.checkBox_Reconnect.Name = "checkBox_Reconnect";
            this.checkBox_Reconnect.Size = new System.Drawing.Size(79, 17);
            this.checkBox_Reconnect.TabIndex = 5;
            this.checkBox_Reconnect.Text = "Reconnect";
            this.checkBox_Reconnect.UseVisualStyleBackColor = true;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(94, 73);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(168, 23);
            this.button_Connect.TabIndex = 4;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_WorldId
            // 
            this.comboBox_WorldId.FormattingEnabled = true;
            this.comboBox_WorldId.Location = new System.Drawing.Point(80, 46);
            this.comboBox_WorldId.Name = "comboBox_WorldId";
            this.comboBox_WorldId.Size = new System.Drawing.Size(182, 21);
            this.comboBox_WorldId.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "World Id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Room Type:";
            // 
            // comboBox_RoomType
            // 
            this.comboBox_RoomType.FormattingEnabled = true;
            this.comboBox_RoomType.Location = new System.Drawing.Point(80, 19);
            this.comboBox_RoomType.Name = "comboBox_RoomType";
            this.comboBox_RoomType.Size = new System.Drawing.Size(182, 21);
            this.comboBox_RoomType.TabIndex = 0;
            this.comboBox_RoomType.SelectedIndexChanged += new System.EventHandler(this.comboBox_RoomType_SelectedIndexChanged);
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.Controls.Add(this.textBox_Password);
            this.groupBox_Login.Controls.Add(this.button_Login);
            this.groupBox_Login.Controls.Add(this.comboBox_Email);
            this.groupBox_Login.Controls.Add(this.label3);
            this.groupBox_Login.Controls.Add(this.label2);
            this.groupBox_Login.Controls.Add(this.label1);
            this.groupBox_Login.Controls.Add(this.comboBox_Server);
            this.groupBox_Login.Location = new System.Drawing.Point(8, 6);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size(268, 132);
            this.groupBox_Login.TabIndex = 0;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "Login";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(80, 74);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '‼';
            this.textBox_Password.Size = new System.Drawing.Size(182, 20);
            this.textBox_Password.TabIndex = 7;
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(9, 100);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(253, 23);
            this.button_Login.TabIndex = 6;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // comboBox_Email
            // 
            this.comboBox_Email.FormattingEnabled = true;
            this.comboBox_Email.Location = new System.Drawing.Point(80, 46);
            this.comboBox_Email.Name = "comboBox_Email";
            this.comboBox_Email.Size = new System.Drawing.Size(182, 21);
            this.comboBox_Email.TabIndex = 4;
            this.comboBox_Email.SelectedIndexChanged += new System.EventHandler(this.comboBox_Email_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server:";
            // 
            // comboBox_Server
            // 
            this.comboBox_Server.FormattingEnabled = true;
            this.comboBox_Server.Location = new System.Drawing.Point(80, 19);
            this.comboBox_Server.Name = "comboBox_Server";
            this.comboBox_Server.Size = new System.Drawing.Size(182, 21);
            this.comboBox_Server.TabIndex = 0;
            this.comboBox_Server.SelectedIndexChanged += new System.EventHandler(this.comboBox_Server_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(652, 436);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 413);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drawer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Draw speed:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(131, 19);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(60, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listBox_PlayerList);
            this.tabPage4.Controls.Add(this.textBox_ChatText);
            this.tabPage4.Controls.Add(this.textBox3);
            this.tabPage4.Controls.Add(this.richTextBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(652, 436);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Chat";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listBox_PlayerList
            // 
            this.listBox_PlayerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_PlayerList.FormattingEnabled = true;
            this.listBox_PlayerList.Location = new System.Drawing.Point(613, 3);
            this.listBox_PlayerList.Name = "listBox_PlayerList";
            this.listBox_PlayerList.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listBox_PlayerList.Size = new System.Drawing.Size(155, 353);
            this.listBox_PlayerList.TabIndex = 3;
            this.listBox_PlayerList.SelectedIndexChanged += new System.EventHandler(this.listBox_PlayerList_SelectedIndexChanged);
            // 
            // textBox_ChatText
            // 
            this.textBox_ChatText.Location = new System.Drawing.Point(63, 368);
            this.textBox_ChatText.Name = "textBox_ChatText";
            this.textBox_ChatText.Size = new System.Drawing.Size(544, 20);
            this.textBox_ChatText.TabIndex = 2;
            this.textBox_ChatText.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(8, 368);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(48, 20);
            this.textBox3.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Location = new System.Drawing.Point(8, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(599, 359);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.textBox6);
            this.tabPage6.Controls.Add(this.textBox5);
            this.tabPage6.Controls.Add(this.checkedListBox_SubBots);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(652, 436);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Bot Systems";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(8, 408);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(443, 20);
            this.textBox6.TabIndex = 2;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(6, 6);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(448, 396);
            this.textBox5.TabIndex = 1;
            // 
            // checkedListBox_SubBots
            // 
            this.checkedListBox_SubBots.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkedListBox_SubBots.FormattingEnabled = true;
            this.checkedListBox_SubBots.Location = new System.Drawing.Point(460, 3);
            this.checkedListBox_SubBots.Name = "checkedListBox_SubBots";
            this.checkedListBox_SubBots.Size = new System.Drawing.Size(189, 430);
            this.checkedListBox_SubBots.TabIndex = 0;
            this.checkedListBox_SubBots.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_SubBots_SelectedIndexChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.groupBox_Code);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(652, 436);
            this.tabPage7.TabIndex = 7;
            this.tabPage7.Text = "Crack code";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // groupBox_Code
            // 
            this.groupBox_Code.Controls.Add(this.numericUpDown_CrackCodeDigits);
            this.groupBox_Code.Controls.Add(this.label7);
            this.groupBox_Code.Controls.Add(this.label6);
            this.groupBox_Code.Controls.Add(this.numericUpDown_MaxCrackCode);
            this.groupBox_Code.Controls.Add(this.progressBar_CodeCracker);
            this.groupBox_Code.Controls.Add(this.textBox_CrackedCode);
            this.groupBox_Code.Controls.Add(this.button_CrackCode);
            this.groupBox_Code.Location = new System.Drawing.Point(8, 6);
            this.groupBox_Code.Name = "groupBox_Code";
            this.groupBox_Code.Size = new System.Drawing.Size(267, 131);
            this.groupBox_Code.TabIndex = 4;
            this.groupBox_Code.TabStop = false;
            this.groupBox_Code.Text = "Code";
            // 
            // numericUpDown_CrackCodeDigits
            // 
            this.numericUpDown_CrackCodeDigits.Location = new System.Drawing.Point(182, 48);
            this.numericUpDown_CrackCodeDigits.Name = "numericUpDown_CrackCodeDigits";
            this.numericUpDown_CrackCodeDigits.Size = new System.Drawing.Size(79, 20);
            this.numericUpDown_CrackCodeDigits.TabIndex = 8;
            this.numericUpDown_CrackCodeDigits.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(140, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Digits:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Max:";
            // 
            // numericUpDown_MaxCrackCode
            // 
            this.numericUpDown_MaxCrackCode.Location = new System.Drawing.Point(42, 48);
            this.numericUpDown_MaxCrackCode.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDown_MaxCrackCode.Name = "numericUpDown_MaxCrackCode";
            this.numericUpDown_MaxCrackCode.Size = new System.Drawing.Size(79, 20);
            this.numericUpDown_MaxCrackCode.TabIndex = 5;
            this.numericUpDown_MaxCrackCode.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // progressBar_CodeCracker
            // 
            this.progressBar_CodeCracker.Location = new System.Drawing.Point(6, 103);
            this.progressBar_CodeCracker.MarqueeAnimationSpeed = 1000;
            this.progressBar_CodeCracker.Maximum = 11100;
            this.progressBar_CodeCracker.Name = "progressBar_CodeCracker";
            this.progressBar_CodeCracker.Size = new System.Drawing.Size(255, 23);
            this.progressBar_CodeCracker.TabIndex = 4;
            // 
            // textBox_CrackedCode
            // 
            this.textBox_CrackedCode.Location = new System.Drawing.Point(87, 77);
            this.textBox_CrackedCode.Name = "textBox_CrackedCode";
            this.textBox_CrackedCode.Size = new System.Drawing.Size(174, 20);
            this.textBox_CrackedCode.TabIndex = 3;
            this.textBox_CrackedCode.Text = "Cracked Code";
            // 
            // button_CrackCode
            // 
            this.button_CrackCode.Location = new System.Drawing.Point(6, 74);
            this.button_CrackCode.Name = "button_CrackCode";
            this.button_CrackCode.Size = new System.Drawing.Size(75, 23);
            this.button_CrackCode.TabIndex = 2;
            this.button_CrackCode.Text = "Crack Code";
            this.button_CrackCode.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Location = new System.Drawing.Point(666, 25);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConsole.Size = new System.Drawing.Size(295, 398);
            this.textBoxConsole.TabIndex = 1;
            // 
            // labelConsole
            // 
            this.labelConsole.AutoSize = true;
            this.labelConsole.Location = new System.Drawing.Point(666, 6);
            this.labelConsole.Name = "labelConsole";
            this.labelConsole.Size = new System.Drawing.Size(45, 13);
            this.labelConsole.TabIndex = 2;
            this.labelConsole.Text = "Console";
            // 
            // textBoxConsoleInput
            // 
            this.textBoxConsoleInput.Location = new System.Drawing.Point(666, 429);
            this.textBoxConsoleInput.Name = "textBoxConsoleInput";
            this.textBoxConsoleInput.Size = new System.Drawing.Size(295, 20);
            this.textBoxConsoleInput.TabIndex = 3;
            this.textBoxConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxConsoleInput_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 462);
            this.Controls.Add(this.textBoxConsoleInput);
            this.Controls.Add(this.labelConsole);
            this.Controls.Add(this.textBoxConsole);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "OstBot 2.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxCode.ResumeLayout(false);
            this.groupBoxCode.PerformLayout();
            this.groupBox_Connect.ResumeLayout(false);
            this.groupBox_Connect.PerformLayout();
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.groupBox_Code.ResumeLayout(false);
            this.groupBox_Code.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CrackCodeDigits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxCrackCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox_Login;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.ComboBox comboBox_Email;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_Server;
        private System.Windows.Forms.GroupBox groupBox_Connect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox checkBox_Reconnect;
        public System.Windows.Forms.ComboBox comboBox_WorldId;
        public System.Windows.Forms.ComboBox comboBox_RoomType;
        public System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox_ChatText;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        public System.Windows.Forms.ListBox listBox_PlayerList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.CheckedListBox checkedListBox_SubBots;
        private System.Windows.Forms.GroupBox groupBoxCode;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button_EnterCode;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.GroupBox groupBox_Code;
        private System.Windows.Forms.NumericUpDown numericUpDown_CrackCodeDigits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_MaxCrackCode;
        private System.Windows.Forms.ProgressBar progressBar_CodeCracker;
        private System.Windows.Forms.TextBox textBox_CrackedCode;
        private System.Windows.Forms.Button button_CrackCode;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.Label labelConsole;
        private System.Windows.Forms.TextBox textBoxConsoleInput;
    }
}