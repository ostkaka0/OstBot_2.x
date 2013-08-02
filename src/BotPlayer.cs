using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class BotPlayer : Player
    {
        Stopwatch betaDigTimer = new Stopwatch();

        public Player(PlayerIOClient.Message m) : Player(m)
        {

        }

        void Update()
        {
        }
    }
}
