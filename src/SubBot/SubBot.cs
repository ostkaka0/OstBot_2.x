using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OstBot_2_
{
    public abstract class SubBot
    {
        public SubBot()
        {
            new Thread(() =>
                {
                    while (!OstBot.connected)
                        Thread.Sleep(100);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (OstBot.connected)
                    {
                        Update();
                        int time = (int)stopwatch.ElapsedMilliseconds;
                        if (time < 50)
                            Thread.Sleep(50 - time);
                        stopwatch.Reset();
                    }
                }).Start();
        }
        void onMessage(object sender, PlayerIOClient.Message m);
        void onDisconnect(object sender, string reason);
        void Update();
    }
}
