using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPickleRick
{
    class Globals
    {
        public static ScheduledEvents allTasks;
        public static frmOverlay overlay;
        public static Settings settings;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Globals.settings = new Settings();
            Globals.allTasks = new ScheduledEvents();
            Globals.overlay = new frmOverlay();
            Application.Run(new frmScheduler());
            Globals.settings.Save();
            Globals.allTasks.Save();
        }
    }
}
