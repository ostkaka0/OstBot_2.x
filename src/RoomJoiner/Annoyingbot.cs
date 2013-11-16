using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace OstBot_2_
{
    public class AnnoyingBot : RoomJoiner
    {
        public AnnoyingBot(string gameId, string email, string password)
            : base(gameId, email, password)
        {
        }

        public override void OnMessage(RoomConnection sender, Message m)
        {
            switch (m.Type)
            {
                case "init":
                    Console.WriteLine(sender.Room);
                    sender.Connection.Send("say", @"1. http://ostkaka.weebly.com/eeo" + " 2.");
                    System.Threading.Thread.Sleep(2000);
                    this.connections.Remove(sender.Room);
                    break;
            }
        }
    }
}
