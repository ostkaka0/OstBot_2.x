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

        public BotPlayer(PlayerIOClient.Message m) : base(m)
        {

        }

        void Update()
        {
        }
    }
}
