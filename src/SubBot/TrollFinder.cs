using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class TrollFinder : SubBot
    {
        private Dictionary<int, int> playerPlaced = new Dictionary<int, int>();
        //private Dicitonary<string, Dictionary<int>> 

        public TrollFinder()
        {
            UpdateSleep = 500;
        }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "b":
                case "bc": // bc, bs, pt, lb, br
                case "bs":
                case "pt":
                case "lb":
                case "br":
                    {
                        Block block = new Block(m);

                        lock (playerPlaced)
                        {
                            if (playerPlaced.ContainsKey(block.b_userId))
                                playerPlaced[block.b_userId]++;
                            else
                                playerPlaced.Add(block.b_userId, 1);
                        }
                    }
                    break;

                case "left":
                    {
                        
                    }
                    break;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {
            
        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            
        }

        public override void Update()
        {
            lock (playerPlaced)
            {
                /*Program.mainForm.Invoke(new Action(() =>
                {
                    Program.mainForm.PushPlacedData(playerPlaced);

                }));*/

                List<int> keys = new List<int>();
                foreach (var p in playerPlaced)
                {
                    keys.Add(p.Key);
                }

                foreach (int k in keys)
                {
                    playerPlaced[k] = 0;
                }
                
            }
        }
    }
}
