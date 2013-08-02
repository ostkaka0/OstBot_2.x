﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace OstBot_2_
{
    public class BotPlayer : Player
    {
        Stopwatch betaDigTimer = new Stopwatch();
        Stopwatch playerTickTimer = new Stopwatch();

        public BotPlayer(PlayerIOClient.Message m)
            : base(m.GetInt(0), m.GetString(1), m.GetInt(2), m.GetFloat(3), m.GetFloat(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), false, false, 0)
        {
            playerTickTimer.Start();
            new System.Threading.Thread(() => 
            {
                while (true)
                {
                    if (playerTickTimer.ElapsedMilliseconds >= (1000 / (1000 / Config.physics_ms_per_tick)))
                    {
                        playerTickTimer.Restart();
                        foreach (Player player in OstBot.playerList.Values)
                        {
                            player.tick();
                        }
                    }
                }
            }).Start();
        }

        void Update()
        {
        }
    }
}
