using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;
using System.IO;

namespace OstBot_2_
{
    public class BotPlayer : Player
    {
        Stopwatch betaDigTimer = new Stopwatch();
        public Inventory inventory = new Inventory(100);
        protected int xp = 0;
        protected int xpRequired;
        protected int digLevel = 1;
        protected int money = 0;
        protected bool betaDig = false;
        protected bool fastDig = true;

        public BotPlayer(PlayerIOClient.Message m)
            : base(m.GetInt(0), m.GetString(1), m.GetInt(2), m.GetFloat(3), m.GetFloat(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), false, false, 0)
        {
            if (File.Exists(@"data\" + name))
                inventory.Load(@"data\" + name);

            xpRequired = getXpRequired(digLevel);
        }

        ~BotPlayer()
        {
            inventory.Save(@"data\" + name);
        }

        public int digRange
        {
            get { return ((digLevel > 0 && fastDig) ? 2 : 1) + ((betaDig) ? 1 : 0); }
        }

        public int digStrength
        {
            get { return 1; }
        }

        public int xp_
        {
            get { return xp; }
            set
            {
                if (value > xp)
                {
                    xp = value;
                    if (xp >= xpRequired)
                        xpRequired = getXpRequired(++digLevel);
                    else
                        xpRequired = getXpRequired((digLevel = getLevel(xp)));
                }
            }
        }

        public int xpRequired_
        {
            get { return xpRequired; }
        }

        public int level_
        {
            get { return digLevel; }
        }

        public int money_
        {
            get { return money; }
            set { money = value; }
        }

        private int getLevel(int xp)
        {
            int level = 0;

            while (xp > getXpRequired(level))
                level++;

            return level;
        }

        private int getXpRequired(int level)
        {
            return Fibonacci(level + 2) * 8;
        }

        private int Fibonacci(int i)
        {
            if (i < 1)
            {
                return 0;
            }
            else
            {
                int j = 1;
                int k = 0;


                for (int l = 1; l < i; l++)
                {
                    j += k;
                    k = j;
                }

                return j;
            }
        }
    }
}
