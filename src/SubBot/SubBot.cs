using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public interface SubBot
    {
        void onMessage(object sender, PlayerIOClient.Message m);
        void onDisconnect(object sender, string reason);
        void Update();
    }
}
