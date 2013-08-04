﻿using System;
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
        public int xp = 0;
        public int digLevel = 1;
        public int money = 0;
        bool betaDig = false;
        bool fastDig = true;

        public int digRange
        {
            get { return ((digLevel > 0 && fastDig) ? 2 : 1) + ((betaDig) ? 1 : 0); }
        }

        public int digStrength
        {
            get { return 1; }
        }

        public BotPlayer(PlayerIOClient.Message m)
            : base(m.GetInt(0), m.GetString(1), m.GetInt(2), m.GetFloat(3), m.GetFloat(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), false, false, 0)
        {
            if(File.Exists(@"data\" + name))
                inventory.Load(@"data\" + name);
        }

        ~BotPlayer()
        {
            inventory.Save(@"data\" + name);
        }

    }
}
