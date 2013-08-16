using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OstBot_2_
{
    public class Program
    {
        public static OstBot ostBot;
        public static Form1 form1;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                ostBot = new OstBot();
                Application.EnableVisualStyles();
                Application.Run(form1 = new Form1());
            }
            catch (Exception e)
            {
                OstBot.shutdown();
                ostBot = null;
                throw e;
            }
        }
    }
}
