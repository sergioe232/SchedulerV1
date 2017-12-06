using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPickleRick
{
    public partial class frmOverlay : Form
    {
        public frmOverlay()
        {
            InitializeComponent();
        }
        
        private void frmOverlay_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.PrimaryScreen;
            this.Left = screen.WorkingArea.Right - this.Width;
            this.Top = screen.WorkingArea.Top;

            Globals.allTasks.OnAdd += MainListener;
            Globals.allTasks.OnEdit += MainListener;
            Globals.allTasks.OnRemove += MainListener;
            Globals.allTasks.OnEventPassed += MainListener;

            refresh();
        }

        private List<ScheduledEvent> upcomingEventsList;

        private void MainListener(object sender, EventArgs e)
        {
            refresh();
        }

        public void refresh()
        {
            var allUpcomingEvents = Globals.allTasks.getUpcomingEvents();
            upcomingEventsList = allUpcomingEvents.GetRange(0, Math.Min(Globals.settings.maxOverlayTasks, allUpcomingEvents.Count));
            List<Control> controlList = new List<Control>();

            foreach(Control control in this.Controls)
            {
                if (control != btnOverlayDrag)
                {
                    controlList.Add(control);
                }
            }
            foreach (Control control in controlList)
            {
                control.Dispose();
            }

            int yOffset = 0;
            var maxWidth = this.Size.Width - 40;

            foreach (var task in upcomingEventsList)
            {
                var taskDisplay = new ScheduledEventDisplay(task);
                taskDisplay.Location = new System.Drawing.Point(0, yOffset);

                taskDisplay.txtOverlayEventName.MaximumSize = new System.Drawing.Size(maxWidth, 0);

                var taskDisplayHeight = taskDisplay.txtOverlayEventName.Size.Height;
                taskDisplayHeight += taskDisplay.txtOverlayEventTime.Size.Height;
                taskDisplayHeight += taskDisplay.lblOverlayTimeLeft.Size.Height;
                taskDisplay.Size = new System.Drawing.Size(maxWidth, taskDisplayHeight);

                this.Controls.Add(taskDisplay);

                yOffset += taskDisplayHeight + 20;
            }

            this.Size = new System.Drawing.Size(this.Size.Width, yOffset + 40);
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void btnOverlayDrag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
