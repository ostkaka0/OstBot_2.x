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
            SubBotHandler.AddSubBot(this);

            new Task(() =>
                {
                    try
                    {
                        while (!OstBot.connected)
                            Thread.Sleep(1000);

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        while (OstBot.connected)
                        {
                            Update();
                            int time = (int)stopwatch.ElapsedMilliseconds;
                            if (time < 500)
                                Thread.Sleep(500 - time);
                            stopwatch.Reset();
                        }
                    }
                    catch (Exception e)
                    {
                        OstBot.shutdown();
                        throw e;
                    }
                }).Start();
        }

        ~SubBot()
        {
            SubBotHandler.RemoveSubBot(this);
        }

        public abstract void onMessage(object sender, PlayerIOClient.Message m);
        public abstract void onDisconnect(object sender, string reason);
        public abstract void Update();
    }
}
