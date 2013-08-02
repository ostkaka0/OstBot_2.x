using System;
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

        public BotPlayer(int ID, string name, int frame, float xPos, float yPos, bool isGod, bool isMod, bool bla, int coins, bool purple, bool isFriend, int level)
            : base(ID, name, frame, xPos, yPos, isGod, isMod, bla, coins, purple, isFriend, level)
        {
            new System.Threading.Thread(() => { }).Start();
        }

        void Update()
        {
        }
    }
}
