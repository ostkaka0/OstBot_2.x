using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class Dig : SubBot
    {
        public class Generator
        {
            public static void Generate()
            {

            }
        }

        void SubBot.onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "say":
                    {
                        int userId = m.GetInt(0);
                        string text = m.GetString(1);
                        if (text.StartsWith("!"))
                        {
                            string[] arg = text.ToLower().Split(' ');

                            switch (arg[0])
                            {
                                case "!betadig":
                                    //lock(OstBot.playerListLock
                                    break;
                            }
                        }
                    }
                    break;

                case "m":
                    {
                        int userId = m.GetInt(0);
                        float playerPosX = m.GetFloat(1);
                        float playerPosY = m.GetFloat(2);
                        float speedX = m.GetFloat(3);
                        float speedY = m.GetFloat(4);
                        float modifierX = m.GetFloat(5);
                        float modifierY = m.GetFloat(6);
                        float horizontal = m.GetFloat(7);
                        float vertical = m.GetFloat(8);
                        int Coins = m.GetInt(9);
                        bool purple = m.GetBoolean(10);
                        bool hasLevitation = m.GetBoolean(11);
                    }
                    break;

            }
        }

        void SubBot.onDisconnect(object sender, string reason)
        {

        }
    }
}
