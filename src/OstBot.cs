using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;
using System.Diagnostics;

namespace OstBot_2_
{
    public class OstBot
    {
        public static Client client;
        public static Connection connection;
        public static bool connected = false;
        public static bool isBB = false;
        public static bool hasCode = false;
        public static bool isOwner = false;
        public static string worldKey = "";
        public static Room room = new Room();
        public static Dig dig = new Dig();
        Stopwatch playerTickTimer = new Stopwatch();

        public static Dictionary<string, int> nameList = new Dictionary<string, int>();
        public static Dictionary<int, BotPlayer> playerList = new Dictionary<int, BotPlayer>();
        public static object playerListLock = 0;

        public OstBot()
        {
            playerTickTimer.Start();
            new System.Threading.Thread(() =>
            {
                while (true)
                {
                    if (playerTickTimer.ElapsedMilliseconds >= (1000 / (1000 / Config.physics_ms_per_tick)))
                    {
                        playerTickTimer.Restart();
                        try
                        {
                            foreach (Player player in OstBot.playerList.Values)
                            {
                                player.tick();
                                //Console.WriteLine("Player " + player.name + " has position X" + player.blockX + " Y" + player.blockY);
                            }
                        }
                        catch (Exception e) { }
                    }
                }
            }).Start();
        }

        public static void Login(string server, string email, string password)
        {
            isBB = (server == "blocking-blocks-u4zmkxjugu0ap8xqbymw");

            try
            {
                client = PlayerIO.QuickConnect.SimpleConnect(server, email, password);
            }
            catch (Exception e)
            {
                Program.form1.WriteLine("Error: " + e.ToString());
            }
        }

        public static void Connect()
        {
            try
            {
                Dictionary<string, string> roomData = new Dictionary<string, string>();
                Dictionary<string, string> joinData = new Dictionary<string, string>();

                connection = client.Multiplayer.CreateJoinRoom(Program.form1.comboBox_WorldId.Text , Program.form1.comboBox_RoomType.Text, true, roomData, joinData);
                connected = true;
                connection.OnMessage += new MessageReceivedEventHandler(onMessage);
                connection.OnMessage += new MessageReceivedEventHandler(room.onMessage);
                connection.OnMessage += new MessageReceivedEventHandler(dig.onMessage);
                connection.OnDisconnect += onDisconnect; //=> this.onDisconnect();

                connection.Send("init");
                connection.Send("init2");
            }
            catch (Exception e)
            {
                Program.form1.WriteLine("Error: " + e.ToString());
            }
        }

        private static void onMessage(object sender, PlayerIOClient.Message m)
        {
            //Program.form1.WriteLine(m.ToString());

            switch (m.Type)
            {
                case "init":
                    connected = true;
                    if (isBB)
                    {
                        worldKey = rot13(m[3].ToString());
                        //botPlayerID = m.GetInt(6);
                        //width = m.GetInt(10);
                        //height = m.GetInt(11);
                        hasCode = m.GetBoolean(8);
                        isOwner = m.GetBoolean(9);
                    }
                    else
                    {
                        worldKey = rot13(m[5].ToString());
                        //botPlayerID = m.GetInt(6);
                        //width = m.GetInt(12);
                        //height = m.GetInt(13);
                        hasCode = m.GetBoolean(10);
                        isOwner = m.GetBoolean(11);
                    }
                    hasCode = isOwner;
                    break;

                case "say":
                    lock (playerListLock)
                    {
                        Program.form1.say(playerList[m.GetInt(0)].name, m.GetString(1));
                    }
                    break;

                case "add":
                    lock (playerListLock)
                    {
                        if (!playerList.ContainsKey(m.GetInt(0)))
                        {
                            BotPlayer player = new BotPlayer(m);
                            lock (playerListLock)
                            {
                                playerList.Add(m.GetInt(0), player);
                                nameList.Add(player.name, m.GetInt(0));
                            }
                            Program.form1.listBox_PlayerList.Items.Add(player.name);
                        }
                    }
                    break;

                case "access":
                    hasCode = true;
                    Program.form1.WriteLine("Code Cracked!");
                    //connection.Send("say", "I know the code.");
                    break;
                case "m":
                    {
                        BotPlayer player;
                        int playerID;
                        playerID = int.Parse(m[0].ToString());
                        float playerXPos = float.Parse(m[1].ToString());
                        float playerYPos = float.Parse(m[2].ToString());
                        float playerXSpeed = float.Parse(m[3].ToString());
                        float playerYSpeed = float.Parse(m[4].ToString());
                        float modifierX = float.Parse(m[5].ToString());
                        float modifierY = float.Parse(m[6].ToString());
                        int xDir = int.Parse(m[7].ToString());
                        int yDir = int.Parse(m[8].ToString());
                        if (OstBot.playerList.ContainsKey(playerID))
                        {
                            player = OstBot.playerList[playerID];
                            player.x = playerXPos;
                            player.y = playerYPos;
                            player.speedX = playerXSpeed;
                            player.speedY = playerYSpeed;
                            player.modifierX = modifierX;
                            player.modifierY = modifierY;
                            player.horizontal = xDir;
                            player.vertical = yDir;
                            OstBot.playerList[playerID] = player;
                        }
                    }
                    break;

            }
        }

        private static void onDisconnect(object sender, string reason)
        {
            connected = false;

            Program.form1.listBox_PlayerList.Items.Clear();

            Program.form1.WriteLine("Disconnected by " + sender.ToString() + " with reason: " + reason);

            if (Program.form1.checkBox_Reconnect.Checked)
            {
                Program.form1.button_Connect.Enabled = false;
                Program.form1.WriteLine("Reconnecting...");
                Connect();
                Program.form1.button_Connect.Enabled = true;
            }
            else
            {
                Program.form1.button_Connect.Text = "Connect";
                Program.form1.comboBox_RoomType.Enabled = true;
                Program.form1.comboBox_WorldId.Enabled = true;
            }
        }

        private static string rot13(string str)
        {
            int num = 0;
            string r = "";
            for (int i = 0; i < str.Length; i++)
            {
                num = str[i];
                if ((num >= 0x61) && (num <= 0x7a))
                {
                    if (num > 0x6d)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                else if ((num >= 0x41) && (num <= 90))
                {
                    if (num > 0x4d)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                r = r + ((char)num);
            }
            return r;
        }

        public static object toObject(Message m, uint index)
        {
            string type = m[index].GetType().ToString().Replace("System.", "").Replace("32", "");
            switch (type)
            {
                case "Boolean":
                    return m.GetBoolean(index);
                    break;
                case "Int":
                    return m.GetInt(index);
                    break;
                case "UInt":
                    return m.GetUInt(index);
                    break;
                case "Long":
                    return m.GetLong(index);
                    break;
                case "Ulong":
                    return m.GetULong(index);
                    break;
                case "Double":
                    return m.GetDouble(index);
                    break;
                case "Float":
                    return m.GetFloat(index);
                    break;
                case "Single":
                    return m.GetFloat(index);
                    break;
                case "String":
                    return m.GetString(index);
                    break;
                case "Byte[]":
                    return m.GetByteArray(index);
                    break;

                default:
                    return 0;
                    Console.WriteLine("Type '{0}' not found!",type);
                    break;
            }
        }
    }
}
