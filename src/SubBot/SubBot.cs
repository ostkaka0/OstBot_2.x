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
                        Thread.Sleep(1000);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (OstBot.connected)
                    {
                        Update();
                        int time = (int)stopwatch.ElapsedMilliseconds;
                        if (time < 5000)
                            Thread.Sleep(5000 - time);
                        stopwatch.Reset();
                    }
                }).Start();
        }
        public abstract void onMessage(object sender, PlayerIOClient.Message m);
        public abstract void onDisconnect(object sender, string reason);
        public abstract void Update();
    }
}
