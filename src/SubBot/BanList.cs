using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OstBot_2_
{
    public class BanList : SubBot
    {
        List<string> banList = new List<string>();

        public BanList()
            : base()
        {
            enabled = true;
        }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    lock (banList)
                    {
                        if (!File.Exists("banlist.txt"))
                            File.Create("banlist.txt").Close();
                        StreamReader reader = new StreamReader(System.Environment.CurrentDirectory + @"\banlist.txt");

                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            banList.Add(line);
                        }
                    }
                    break;

                case "add":
                    lock (banList)
                    {
                        if (banList.Contains(m.GetString(1)))
                        {
                            OstBot.connection.Send("say", "/kick " + m.GetString(1));
                        }
                    }
                    break;
            }
        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            if (isBotMod)
            {
                switch (args[0])
                {

                    case "unban":
                        if (args.Length > 1)
                        {
                            lock (banList)
                            {
                                if (banList.Contains(args[1]))
                                {
                                    banList.Remove(args[1]);
                                }
                            }
                        }
                        break;

                    case "ban":
                        if (args.Length > 1)
                        {
                            lock (banList)
                            {
                                banList.Add(args[1]);
                            }
                        }
                        goto case "kick";

                    case "kick":
                        if (args.Length > 1)
                        {
                            OstBot.connection.Send("say", "/kick " + args[1]);
                        }
                        break;
                }
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void Update()
        {

        }
    }
}
