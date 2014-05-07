using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace OstBot_2_
{
    public class Program
    {
        public static OstBot ostBot;
        public static MainForm mainForm;

        [STAThread]
        static void Main(string[] args)
        {

            /*try
            {*/
            Application.EnableVisualStyles();
            ostBot = new OstBot();
            mainForm = new MainForm();
            Application.Run(mainForm);
            /*}
            catch (Exception e)
            {
                OstBot.shutdown();
                ostBot = null;
                throw e;
            }*/
        }
    }
}
