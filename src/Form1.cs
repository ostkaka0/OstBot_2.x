﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace OstBot_2_
{
    public partial class Form1 : Form
    {
        public int runtime = 0;
        string terminalString;
        object terminalStringLock = 0;
        Queue<string[]> sayString = new Queue<string[]>();
        object sayStringLock = 0;


        public Form1()
        {
            InitializeComponent();
            updateComboBoxes(0);
        }

        public void Write(string str)
        {
            lock (terminalStringLock)
            {
                terminalString += str;
            }
        }

        public void WriteLine(string str)
        {
            lock (terminalStringLock)
            {
                terminalString += str + Environment.NewLine;
            }
        }

        public void say(string player, string text)
        {
            lock (sayStringLock)
            {
                sayString.Enqueue(new string[] {player + ": ", text + Environment.NewLine});
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            this.textBox_ChatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);

            backgroundWorker_CodeCracker.ProgressChanged += new ProgressChangedEventHandler
                    (backgroundWorker_CodeCracker_ProgressChanged);

            backgroundWorker_CodeCracker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_CodeCracker_RunWorkerCompleted);

            using (Stream input = File.OpenRead(Environment.CurrentDirectory + @"runtime.dat"))
            using (Stream output = File.OpenWrite(Environment.CurrentDirectory + @"runtime.dat"))
            {
                while (true)
                {
                    int value = input.ReadByte();
                    if (value == -1)
                    {
                        break;
                    }

                    runtime = value + 1;
                    output.WriteByte((byte)runtime);
                }
            }

            Console.WriteLine(runtime);
        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox2.Focused)
                {
                    textBox1.Text += textBox2.Text + System.Environment.NewLine;
                    textBox2.Text = "";
                }
                if (textBox_ChatText.Focused)
                {
                    OstBot.connection.Send(PlayerIOClient.Message.Create("say", new object[] {textBox_ChatText.Text}));
                    say("You: ", textBox_ChatText.Text);
                    textBox_ChatText.Text = "";
                }
            }
        }

        private void updateComboBoxes(int level)
        {
            //this.comboBox_Server.Items.Clear();
            //this.comboBox_Email.Items.Clear();
            //this.comboBox_RoomType.Items.Clear();

            if (!File.Exists("login.txt"))
                File.Create("login.txt").Close();
            StreamReader reader = new StreamReader(System.Environment.CurrentDirectory + @"\login.txt");

            List<string> lines = new List<string>();

            string line;
            string server = "";
            string email = "";
            string roomType = "";

            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
                if (!line.StartsWith("\t"))
                {
                    server = line;

                    if (level == 0)
                        this.comboBox_Server.Items.Add(line);

                    if (server == this.comboBox_Server.Text && level <= 1)
                        this.comboBox_Email.Items.Clear();
                }
                else if (line.StartsWith("\t") && !line.StartsWith("\t\t") && server == this.comboBox_Server.Text)
                {
                    email = line.Replace("\t", "");

                    if (level <= 1)
                        this.comboBox_Email.Items.Add(email);

                    if (email == this.comboBox_Email.Text && level <= 2)
                        this.comboBox_RoomType.Items.Clear();
                }
                else if (line.StartsWith("\t\t") && !line.StartsWith("\t\t\t") && email == this.comboBox_Email.Text)
                {
                    roomType = line.Replace("\t", "");
                    this.comboBox_RoomType.Items.Add(roomType);
                }
                else if (line.StartsWith("\t\t\t") && !line.StartsWith("\t\t\t\t") && email == this.comboBox_Email.Text)
                {
                    this.textBox_Password.Text = line.Replace("\t", "");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.groupBox_Connect.Enabled = false;
            if (!OstBot.connected)
            {
                this.button_Connect.Text = "Connecting...";
                WriteLine("Connecting...");
                OstBot.Connect();
                if (OstBot.connected)
                {
                    this.button_Connect.Text = "Disconnect";
                    this.comboBox_RoomType.Enabled = false;
                    this.comboBox_WorldId.Enabled = false;
                    WriteLine("Connecting succeeded!");
                }
                else
                {
                    this.button_Connect.Text = "Connect";
                    WriteLine("Connecting failed!");
                }
            }
            else
            {
                this.button_Connect.Text = "Disconnecting...";
                WriteLine("Disconnecting...");
                bool reconnect = this.checkBox_Reconnect.Enabled;
                this.checkBox_Reconnect.Enabled = false;
                OstBot.connection.Disconnect();
                this.checkBox_Reconnect.Enabled = reconnect;
                this.checkBox_Reconnect.Enabled = true;
                this.button_Connect.Text = "Connect";
                this.comboBox_RoomType.Enabled = true;
                this.comboBox_WorldId.Enabled = true;
            }
            this.groupBox_Connect.Enabled = true;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            OstBot.Login(comboBox_Server.Text, comboBox_Email.Text, textBox_Password.Text);
        }

        private void comboBox_Server_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            updateComboBoxes(1);
            if (this.comboBox_Email.Items.Count > 0)
                this.comboBox_Email.Text = this.comboBox_Email.Items[0].ToString();
        }

        private void comboBox_Email_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateComboBoxes(2);
            if (this.comboBox_RoomType.Items.Count > 0)
                this.comboBox_RoomType.Text = this.comboBox_RoomType.Items[0].ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (terminalStringLock)
            {
                textBox1.Text += terminalString;
                terminalString = "";
            }
            string lastPlayer = "";
            while (sayString.Count > 0)
            {
                string[] pair = sayString.Dequeue();

                if (pair[0] != lastPlayer)
                {
                    lastPlayer = pair[0];

                    richTextBox1.SelectionFont = richTextBox1.Font;
                    richTextBox1.SelectionStart = richTextBox1.SelectionStart + richTextBox1.SelectionLength;
                    richTextBox1.SelectionLength = 0;

                    richTextBox1.SelectionFont = new Font(richTextBox1.Font,
                    richTextBox1.SelectionFont.Style | FontStyle.Italic);
                    richTextBox1.Text += pair[0];

                    richTextBox1.SelectionStart = richTextBox1.SelectionStart + richTextBox1.SelectionLength;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectionFont = richTextBox1.Font;
                }
                richTextBox1.Text += pair[1];
            }
            //richTextBox1.Text += sayString;
            sayString.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerIOClient.RoomInfo[] roomInfo = OstBot.client.Multiplayer.ListRooms(comboBox_RoomType.Text, new Dictionary<string,string>(), 200000, 0);

            //checkedListBox_Rooms.Items.Clear();
            listView1.Items.Clear();

            Array.Sort(roomInfo, delegate(PlayerIOClient.RoomInfo a, PlayerIOClient.RoomInfo b) { return b.OnlineUsers.CompareTo(a.OnlineUsers); });

            foreach (var room in roomInfo)
            {
                listView1.Items.Add(new ListViewItem(new string[] { room.Id, room.OnlineUsers.ToString(), room.RoomType }));
                //checkedListBox_Rooms.Items.Add(room.Id.ToString());
                //checkedListBox_Rooms.Items.Add(room.OnlineUsers.ToString());
                //checkedListBox_Rooms.Items.Add(room.ToString());
                Console.WriteLine(room.ToString());
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_RoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OstBot.client != null)
            {
                PlayerIOClient.RoomInfo[] roomInfo = OstBot.client.Multiplayer.ListRooms(comboBox_RoomType.Text, new Dictionary<string, string>(), 200000, 0);

                //checkedListBox_Rooms.Items.Clear();
                listView1.Items.Clear();

                Array.Sort(roomInfo, delegate(PlayerIOClient.RoomInfo a, PlayerIOClient.RoomInfo b) { return b.OnlineUsers.CompareTo(a.OnlineUsers); });

                foreach (var room in roomInfo)
                {
                    string name = (!room.RoomData.ContainsKey("name")) ? "" : room.RoomData["name"];
                    string plays = (!room.RoomData.ContainsKey("plays")) ? "" : room.RoomData["plays"];


                    listView1.Items.Add(new ListViewItem(new string[] { name , room.OnlineUsers.ToString(), plays, room.Id}));
                    //checkedListBox_Rooms.Items.Add(room.Id.ToString());
                    //checkedListBox_Rooms.Items.Add(room.OnlineUsers.ToString());
                    //checkedListBox_Rooms.Items.Add(room.ToString());
                    foreach (var pair in room.RoomData)
                    {
                        WriteLine(pair.Key + "\t" + pair.Value);
                    }
                    Console.WriteLine(room.ToString());
                }
            }
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                comboBox_WorldId.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = textBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_Hide_CheckedChanged(object sender, EventArgs e)
        {
            /*if (checkBox_Hide.Checked)
                textBox_CrackedCode.PasswordChar = '*';
            else
                textBox_CrackedCode.PasswordChar = UseSystemPasswordChar;*/

        }

        private string toDigits(int digits, int number)
        {
            string r = number.ToString();

            for (int i = r.Length; i < digits; i++)
                r = "0" + r;

            return r;
        }

        private void button_CrackCode_Click(object sender, EventArgs e)
        {
            button_CrackCode.Enabled = false;

            if (OstBot.connection == null)
                return;

            backgroundWorker_CodeCracker.RunWorkerAsync();
        }

        private void backgroundWorker_CodeCracker_DoWork(object sender, DoWorkEventArgs e)
        {
            int codeMinValue = 0;

            for (int i = 0; i + codeMinValue < numericUpDown_MaxCrackCode.Value; i++)
            {
                    OstBot.connection.Send("access",
                        toDigits((int)numericUpDown_CrackCodeDigits.Value, codeMinValue + i));

                if ((codeMinValue + i) % 2 == 0)
                    Thread.Sleep(1);

                if ((codeMinValue + i) % 100 == 0)
                {
                    WriteLine((codeMinValue + i).ToString());
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue + i);
                }

                if (OstBot.hasCode)
                {
                    OstBot.hasCode = false;
                    codeMinValue = (codeMinValue + i) - 1000;
                    if (codeMinValue < 0)
                        codeMinValue = 0;
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue);
                    break;
                }
            }

            for (int i = 0; i < 1000 && i + codeMinValue < numericUpDown_MaxCrackCode.Value; i++)
            {
                OstBot.connection.Send("access",
                    toDigits((int)numericUpDown_CrackCodeDigits.Value, codeMinValue + i));

                Thread.Sleep(5);

                if ((codeMinValue + i) % 100 == 0)
                {
                    WriteLine((i+codeMinValue).ToString());
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue+i);
                }

                if (OstBot.hasCode)
                {
                    OstBot.hasCode = false;
                    codeMinValue = codeMinValue +i - 100;
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue);
                    break;
                }
            }

            for (int i = 0; i < 100 && i + codeMinValue < numericUpDown_MaxCrackCode.Value; i++)
            {
                OstBot.connection.Send("access",
                    toDigits((int)numericUpDown_CrackCodeDigits.Value, codeMinValue + i));

                Thread.Sleep(50);

                WriteLine((i+codeMinValue).ToString());
                backgroundWorker_CodeCracker.ReportProgress(codeMinValue + i);

                if (OstBot.hasCode)
                {
                    OstBot.hasCode = false;
                    codeMinValue = codeMinValue+  i-10;
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue);
                    break;
                }
            }

            for (int i = 0; i < 10 && i + codeMinValue < numericUpDown_MaxCrackCode.Value; i++)
            {
                OstBot.connection.Send("access",
                    toDigits((int)numericUpDown_CrackCodeDigits.Value, codeMinValue + i));

                Thread.Sleep(500);

                WriteLine((i + codeMinValue).ToString());
                backgroundWorker_CodeCracker.ReportProgress(codeMinValue + i);

                if (OstBot.hasCode)
                {
                    OstBot.hasCode = false;
                    codeMinValue += i;
                    backgroundWorker_CodeCracker.ReportProgress(codeMinValue);
                    break;
                }
            }
        }

        void backgroundWorker_CodeCracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke :)
            // Update the progressBar with the integer supplied to us from the
            // ReportProgress() function.
            progressBar_CodeCracker.Value = e.ProgressPercentage;
            textBox_CrackedCode.Text = toDigits((int)numericUpDown_CrackCodeDigits.Value, e.ProgressPercentage);
            //lblStatus.Text = "Processing......" + progressBar1.Value.ToString() + "%";
        }

        void backgroundWorker_CodeCracker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar_CodeCracker.Value = 0;

            button_CrackCode.Enabled = true;
        }

        private void progressBar_CodeCracker_Click(object sender, EventArgs e)
        {

        }

        private void groupBox_Login_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown_CrackCodeDigits_ValueChanged(object sender, EventArgs e)
        {
            progressBar_CodeCracker.Maximum = (int)Math.Pow(10, (double)numericUpDown_CrackCodeDigits.Value) + 1000 + 100 + 10;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            OstBot.room.setDrawSleep((int)numericUpDown1.Value);
        } 
    }
}
