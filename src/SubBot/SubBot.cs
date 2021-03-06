﻿using System;
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
        protected int UpdateSleep = 200;
        public int id = -1;
        private bool enabledValue;
        public bool enabled
        {
            get { return enabledValue; }
            set
            {
                if (value != enabled)
                {
                    enabledValue = value;
                    if (id != -1)
                    {
                        Program.mainForm.Invoke(new Action(() =>
                            Program.mainForm.checkedListBox_SubBots.SetItemChecked(id, value)
                        ));
                    }

                    if (value)
                    {
                        Program.mainForm.WriteToConsole(this.GetType().Name + ".cs is enabled.");
                    }
                    else
                    {
                        Program.mainForm.WriteToConsole(this.GetType().Name + ".cs is disabled.");
                    }
                }
            }
        }

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
                            while (enabled)
                            {
                                Update();
                                int time = (int)stopwatch.ElapsedMilliseconds;
                                if (time < UpdateSleep)
                                    Thread.Sleep(UpdateSleep - time);
                                stopwatch.Reset();
                            }
                            Thread.Sleep(200);
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
        public abstract void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod);
        public abstract void Update();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
