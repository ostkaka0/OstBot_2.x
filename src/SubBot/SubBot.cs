using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    interface SubBot
    {
        public void onMessage(object sender, PlayerIOClient.Message m);
        public void onDisconnect(object sender, string reason);
    }
}
