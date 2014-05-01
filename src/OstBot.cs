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
        public static string title = "";
        public static string owner = "";

        public static Room room;
        public static Dig dig;
        public static Commands commands;
        public static BanList banList;
        public static TrollFinder trollFinder;

        public static MazeGenerator mazeGenerator;
        public static MazeDig mazeDig;
        Stopwatch playerTickTimer = new Stopwatch();

        public static PlayerPhysics playerPhysics;
        public static Zombies zombies;

        public static Random r = new Random();

        public static Dictionary<string, int> nameList = new Dictionary<string, int>();
        public static Dictionary<int, BotPlayer> playerList = new Dictionary<int, BotPlayer>();
        //public static Dictionary<int, BotPlayer> playerListTemp = new Dictionary<int, BotPlayer>();
        public static Dictionary<string, int> leftNameList = new Dictionary<string, int>();
        public static Dictionary<int, Player> leftPlayerList = new Dictionary<int, Player>();

        public static List<SubBot> subBotRegister = new List<SubBot>();

        private static Queue<string> listToSay = new Queue<string>();

        public OstBot()
        {
/*<<<<<<< HEAD
            //return;

            playerTickTimer.Start();
            zombieUpdateStopWatch.Start();
            zombieDrawStopWatch.Start();
            Stopwatch oneZombie = new Stopwatch();
            new System.Threading.Thread(() =>
            {
                //try
                //{
                while (true)
                {
                    if (playerTickTimer.ElapsedMilliseconds >= Config.physics_ms_per_tick)
                    {
                        playerTickTimer.Restart();
                        //try
                        //{
                        lock (playerList)
                        {
                            playerListTemp = new Dictionary<int, BotPlayer>(playerList);
                            foreach (Player player in OstBot.playerList.Values)
                            {
                                player.tick();
                                //Console.WriteLine("Player " + player.name + " has position X" + player.blockX + " Y" + player.blockY);
                            }
                        }
                        //}
                        //catch (Exception e) { throw e; }
                    }
                }
                /*}
                catch (Exception e)
                {
                    int line = new StackTrace(e, true).GetFrame(0).GetFileLineNumber();
                    shutdown();
                    if (e != null)
                        throw e;
                    else
                        throw null;
                }* /
=======
>>>>>>> b3253e88aeee9a1d6c456fc9330785b0a14ecc3b*/

        }

        ~OstBot()
        {
            shutdown();
            lock (playerList)
            {
                foreach (var pair in playerList)
                {
                    pair.Value.Save();
                }
            }
        }

        public static void shutdown()
        {
            if (playerList != null)
            {
                lock (OstBot.playerList)
                {
                    foreach (var pair in OstBot.playerList)
                        pair.Value.Save();
                }
            }
        }

        public static void Login(string server, string email, string password)
        {
            isBB = (server != "everybody-edits-su9rn58o40itdbnw69plyw");

            try
            {
                client = PlayerIO.QuickConnect.SimpleConnect(server, email, password);
            }
            catch (Exception e)
            {
                Program.console.WriteLine("Error: " + e.ToString());
            }
        }

        public static void Connect()
        {
            try
            {
                Dictionary<string, string> roomData = new Dictionary<string, string>();
                Dictionary<string, string> joinData = new Dictionary<string, string>();

                connection = client.Multiplayer.CreateJoinRoom(Program.form1.comboBox_WorldId.Text, Program.form1.comboBox_RoomType.Text, true, roomData, joinData);
                connected = true;
                connection.OnMessage += new MessageReceivedEventHandler(onMessage);
                connection.OnDisconnect += new DisconnectEventHandler(onDisconnect);

                room = new Room();
                banList = new BanList();
                trollFinder = new TrollFinder();
                mazeGenerator = new MazeGenerator();
                mazeDig = new MazeDig();
                dig = new Dig();
                commands = new Commands();
                playerPhysics = new PlayerPhysics();
                zombies = new Zombies();
                new Redstone();

                if (isBB)
                {
                    connection.Send("botinit");
                }
                else
                {
                    connection.Send("init");
                    connection.Send("init2");
                }
            }
            catch (Exception e)
            {
                Program.console.WriteLine("Error: " + e.ToString());
                //throw e;

            }
        }

        private static void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        if (isBB)
                        {
                            owner = m.GetString(0);
                            title = m.GetString(1);
                            worldKey = rot13(m[3].ToString());
                            hasCode = m.GetBoolean(8);
                            isOwner = m.GetBoolean(9);
                        }
                        else
                        {
                            owner = m.GetString(0);
                            title = m.GetString(1);
                            worldKey = rot13(m[5].ToString());
                            hasCode = m.GetBoolean(10);
                            isOwner = m.GetBoolean(11);
                        }
                        hasCode |= isOwner;
                        {
                            string roomData = " - " + title + " ¦ by: " + owner;
                            Program.form1.Invoke(new Action(() =>
                                Program.form1.Text = Program.form1.Text.Substring(0, 10) + roomData
                                ));

                            Program.console.Invoke(new Action(() =>
                                Program.console.Text = Program.console.Text.Substring(0, 7) + roomData
                                ));
                        }

                        break;
                    }
                case "say":
                    {
                        lock (playerList)
                        {
                            if (playerList.ContainsKey(m.GetInt(0)))
                                Program.form1.say(playerList[m.GetInt(0)].name, m.GetString(1));
                        }
                        int playerId = m.GetInt(0);
                        SubBotHandler.onCommand(sender, m.GetString(1).Substring(1), playerId);
                        break;
                    }
                case "add":
                    {
                        BotPlayer player = new BotPlayer(m);
                        lock (playerList)
                        {
                            if (!playerList.ContainsKey(m.GetInt(0)))
                                playerList.Add(m.GetInt(0), player);
                            else
                                return;

                        }
                        lock (nameList)
                        {
                            if (nameList.ContainsKey(player.name))
                            {
                                nameList.Remove(player.name);
                            }
                            nameList.Add(player.name, m.GetInt(0));
                            Program.form1.say("System", player.name + " joined!");
                            lock (Program.form1.lambdaFunctionQueue)
                            {
                                Program.form1.lambdaFunctionQueue.Enqueue((Form1 form1) =>
                                    {
                                        lock (Program.form1.listBox_PlayerList.Items)
                                            Program.form1.listBox_PlayerList.Items.Add(player.name);
                                    });
                            }

                        }
                    }
                    break;
                case "left":
                    {
                        int tempKey = m.GetInt(0);
                        Player player;

                        lock (playerList)
                        {
                            if (playerList.ContainsKey(tempKey))
                                player = playerList[tempKey];
                            else
                                return;
                        }
                        string name = player.name;
                        Program.form1.say("System", name + " left!");
                        lock (Program.form1.lambdaFunctionQueue)
                        {
                            Program.form1.lambdaFunctionQueue.Enqueue((Form1 form1) =>
                                {
                                    lock (Program.form1.listBox_PlayerList.Items)
                                        Program.form1.listBox_PlayerList.Items.Remove(name);
                                    lock (leftPlayerList)
                                    {
                                        if (leftPlayerList.ContainsKey(tempKey))
                                            leftPlayerList.Remove(tempKey);
                                        leftPlayerList.Add(tempKey, (Player)player);
                                    }
                                    lock (leftNameList)
                                    {
                                        if (leftNameList.ContainsKey(name))
                                            leftNameList.Remove(name);
                                        leftNameList.Add(name, tempKey);
                                    }
                                    lock (playerList)
                                    {
                                        if (playerList.ContainsKey(tempKey))
                                            playerList.Remove(tempKey);
                                    }
                                    lock (nameList)
                                    {
                                        if (nameList.ContainsKey(name))
                                            nameList.Remove(name);
                                    }
                                });
                        }

                    }
                    break;
                case "teleport":
                    {
                        BotPlayer _loc_5 = null;
                        int playerId = m.GetInt(0);
                        double xPos = m.GetInt(1);
                        double yPos = m.GetInt(2);
                        /*if (param2 == myid)
                        {
                            player.setPosition(param3, param4);
                        }
                        else
                        {*/
                        _loc_5 = playerList[playerId];
                        //if (_loc_5)
                        //{
                        _loc_5.setPosition(xPos, yPos);
                        //}
                        //}
                        //return;
                    }
                    break;
                case "tele":
                    {
                        int playerId = 0;
                        int xPos = 0;
                        int yPos = 0;
                        BotPlayer player = null;
                        bool _loc_2 = m.GetBoolean(0);
                        uint _loc_3 = 1;
                        while (_loc_3 < m.Count)
                        {

                            playerId = m.GetInt(_loc_3);
                            xPos = m.GetInt((_loc_3 + 1));
                            yPos = m.GetInt(_loc_3 + 2);
                            if (playerList.ContainsKey(playerId))
                            {
                                player = playerList[playerId];
                                if (player != null)
                                {
                                    player.x = xPos;
                                    player.y = yPos;
                                    player.respawn();
                                    if (_loc_2)
                                    {
                                        player.resetCoins();
                                        player.purple = false;
                                    }
                                }
                                /*if (_loc_4 == myid)
                                {
                                    player.x = _loc_5;
                                    player.y = _loc_6;
                                    this.x = -_loc_6;
                                    this.y = -_loc_6;
                                    player.respawn();
                                    if (_loc_2)
                                    {
                                        player.resetCoins();
                                        player.purple = false;
                                        world.hidePurple = false;
                                        world.resetCoins();
                                        world.resetSecrets();
                                    }
                                }*/
                            }
                            _loc_3 += 3;
                        }
                    }// end function
                    break;
                case "kill": //error
                    {
                        BotPlayer _loc_3 = null;
                        int playerId = m.GetInt(0);

                        if (playerList.ContainsKey(playerId))
                        {
                            _loc_3 = playerList[playerId];
                            _loc_3.killPlayer();
                        }
                    }
                    break;
                case "access":
                    hasCode = true;
                    Program.console.WriteLine("Code Cracked!");
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
                        lock (playerList)
                        {
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
                    }
                    break;

            }

            SubBotHandler.OnMessage(sender, m);
        }

        private static void onDisconnect(object sender, string reason)
        {
            connected = false;

            SubBotHandler.OnDisconnect(sender, reason);

            lock (playerList)
            {
                foreach (var pair in playerList)
                {
                    pair.Value.Save();
                }

                nameList.Clear();
                playerList.Clear();
            }
            lock (Program.form1.lambdaFunctionQueue)
            {
                Program.form1.lambdaFunctionQueue.Enqueue((Form1 form1) =>
                    {
                        lock (Program.form1.listBox_PlayerList.Items)
                            Program.form1.listBox_PlayerList.Items.Clear();
                    });
            }

            Program.console.WriteLine("Disconnected by " + sender.ToString() + " with reason: " + reason);

            if (Program.form1.checkBox_Reconnect.Checked)
            {
                Program.form1.button_Connect.Enabled = false;
                Program.console.WriteLine("Reconnecting...");
                Connect();
                Program.form1.button_Connect.Enabled = true;
            }
            else
            {
                lock (Program.form1.lambdaFunctionQueue)
                {
                    Program.form1.lambdaFunctionQueue.Enqueue((Form1 form1) =>
                        {
                            lock (Program.form1.button_Connect.Text)
                                Program.form1.button_Connect.Text = "Connect";
                            Program.form1.comboBox_RoomType.Enabled = true;
                            Program.form1.comboBox_WorldId.Enabled = true;
                        });
                }
            }
        }

        public static void Say(string message)
        {
            listToSay.Enqueue(message);
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
                case "Int":
                    return m.GetInt(index);
                case "UInt":
                    return m.GetUInt(index);
                case "Long":
                    return m.GetLong(index);
                case "Ulong":
                    return m.GetULong(index);
                case "Double":
                    return m.GetDouble(index);
                case "Float":
                    return m.GetFloat(index);
                case "Single":
                    return m.GetFloat(index);
                case "String":
                    return m.GetString(index);
                case "Byte[]":
                    return m.GetByteArray(index);

                default:
                    return 0;
                    Console.WriteLine("Type '{0}' not found!", type);
                    break;
            }
        }
    }
}
