using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace OstBot_2_
{
    public class Player
    {
        public int userID;
        public string name = "";
        public int faceID;
        public double x;
        public double y;
        public bool isGod;
        public bool isMod;
        public bool haveChat;
        public int coins;
        public bool friendWithYou;
        public bool purple;
        public int level;

        //public EE.Player eePlayer;

        public Player(PlayerIOClient.Message m)
        {
            this.userID = m.GetInt(0);
            this.name = m.GetString(1);
            this.faceID = m.GetInt(2);
            this.x = m.GetDouble(3);
            this.y = m.GetDouble(4);
            this.isGod = m.GetBoolean(5);
            this.isMod = m.GetBoolean(6);
            this.haveChat = m.GetBoolean(7);
            this.coins = m.GetInt(8);
            if (!OstBot.isBB)
            {
                this.friendWithYou = m.GetBoolean(9);
                this.purple = m.GetBoolean(10);
                this.level = m.GetInt(11);
            }
            else
            {
                this.friendWithYou = false;
                this.purple = false;
                this.level = 0;
            }
            //eePlayer = new EE.Player(userID, name, faceID, x, y, isGod, isMod, haveChat, coins);
        }

        public void move(Message m)
        {
            x = m.GetDouble(1);
            y = m.GetDouble(2);
            /*eePlayer.move(
                m.GetDouble(1), m.GetDouble(2),
                m.GetDouble(3), m.GetDouble(4),
                m.GetDouble(5), m.GetDouble(6),
                m.GetDouble(7), m.GetDouble(8),
                m.GetInt(9));*/
            //Console.WriteLine(eePlayer.x);
        }

        public object[] getData()
        {
            return new object[] { userID, name, faceID, x, y, isGod, isMod, haveChat, coins, friendWithYou, purple, level };
        }
    }
}
