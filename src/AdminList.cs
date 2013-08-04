using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    static class AdminList
    {
        static List<string> list = new List<string> { "ostkaka", "botost", "gustav9797", "gbot" };

        public static bool isAdmin(string player)
        {
            return list.Contains(player);
        }

        public static bool isAdmin(int playerId)
        {
            return list.Contains(OstBot.playerList[playerId].name);
        }
    }
}
