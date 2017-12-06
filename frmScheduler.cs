using System;
using System.Collections;
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
    public partial class frmScheduler : Form
    {
        public frmScheduler()
        {
            InitializeComponent();
        }
        
        private void listview_OnAddListener(ScheduledEvent task, EventArgs e)
        {
            var item = new ListViewItem(task.time.ToString("M/d/yyyy h:mm tt"));
            item.SubItems.Add(task.taskName);
            item.SubItems.Add(task.taskDescription);
            item.Tag = task;

            if (task.time > DateTime.Now)
            {
                lviewUpcomingEvents.Items.Add(item);
                lviewUpcomingEvents.Sort();
            }
            else
            {
                lviewPastEvents.Items.Add(item);
                lviewPastEvents.Sort();
            }
        }

        private void listview_OnEditListener(ScheduledEvent task, EventArgs e)
        {
            foreach(ListViewItem item in lviewUpcomingEvents.Items)
            {
                if((ScheduledEvent)item.Tag == task)
                {
                    item.SubItems[0].Text = task.time.ToString("M/d/yyyy h:mm tt");
                    item.SubItems[1].Text = task.taskName;
                    item.SubItems[2].Text = task.taskDescription;
                }
            }
            lviewUpcomingEvents.Sort();
        }

        private void listview_OnEventPassedListener(ScheduledEvent task, EventArgs e)
        {
            foreach (ListViewItem lvItem in lviewUpcomingEvents.Items)
            {
                if ((ScheduledEvent)lvItem.Tag == task)
                {
                    lviewUpcomingEvents.Items.Remove(lvItem);
                    break;
                }
            }

            var item = new ListViewItem(task.time.ToString("M/d/yyyy h:mm tt"));
            item.SubItems.Add(task.taskName);
            item.Tag = task;
            
            lviewPastEvents.Items.Add(item);
            lviewPastEvents.Sort();

            trayIcon.BalloonTipTitle = "This event has just passed.";
            trayIcon.BalloonTipText = "Event: " + task.taskName + "\nDesrciption: " + task.taskDescription + "\nTime: " + task.time;
            trayIcon.ShowBalloonTip(5000);
        }

        private void listview_OnRemoveListener(ScheduledEvent task, EventArgs e)
        {
            foreach (ListViewItem item in lviewUpcomingEvents.Items)
            {
                if((ScheduledEvent)item.Tag == task)
                {
                    lviewUpcomingEvents.Items.Remove(item);
                    break;
                }
            }

            foreach (ListViewItem item in lviewPastEvents.Items)
            {
                if ((ScheduledEvent)item.Tag == task)
                {
                    lviewPastEvents.Items.Remove(item);
                    break;
                }
            }
        }

        private void trayIcon_OnEventNoticeListener(ScheduledEvent task, EventArgs e)
        {
            trayIcon.BalloonTipTitle = "This event is about to start";
            trayIcon.BalloonTipText = "Event: " + task.taskName + "\nDesrciption: " + task.taskDescription + "\nTime: " + task.time;
            trayIcon.ShowBalloonTip(5000);
        }

        private void frmScheduler_Load(object sender, EventArgs e)
        {
            this.lviewUpcomingEvents.ListViewItemSorter = new ListViewItemComparer(0, lviewUpcomingEvents.Sorting);
            this.lviewPastEvents.ListViewItemSorter = new ListViewItemComparer(0, lviewPastEvents.Sorting);
            lblAreEventsScheduled.Parent = lviewUpcomingEvents;
            StartColor = Globals.settings.schedulerColor1;
            EndColor = Globals.settings.schedulerColor2;

            Globals.allTasks.OnEdit += listview_OnEditListener;
            Globals.allTasks.OnAdd += listview_OnAddListener;
            Globals.allTasks.OnEventPassed += listview_OnEventPassedListener;
            Globals.allTasks.OnRemove += listview_OnRemoveListener;
            Globals.allTasks.OnEventNotice += trayIcon_OnEventNoticeListener;

            Globals.settings.Load();
            Globals.allTasks.Load();

            if (Globals.settings.isOverlayEnabled == false)
                Globals.overlay.Hide();
            else
                Globals.overlay.Show();
        }

        private void tmrTime_Tick(object sender, EventArgs e)
        {
            lblTimeDisplay.Text = DateTime.Now.ToString("MMMM dd, yyyy  h:mm:ss tt");     
        }

        private void msExit_Click(object sender, EventArgs e)
        {
            this.DestroyHandle();
        }

        private void msAboutUs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Scheduler was developed by Sergio Enriquez\n\nBugs can be reported to sergioe232@gmail.com", "About Us", MessageBoxButtons.OK);
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            var eventEditor = new frmEventEditor();
            eventEditor.Text = "Add Event";
            eventEditor.ShowDialog();
        }

        private void btnEditEvent_Click(object sender, EventArgs e)
        {
            var items = lviewUpcomingEvents.SelectedItems;
            var count = lviewUpcomingEvents.SelectedItems.Count;

            if (count == 1)
            {
                var eventEditor = new frmEventEditor((ScheduledEvent)items[0].Tag);
                eventEditor.Text = "Edit Event";
                eventEditor.ShowDialog();
            }
            else if(count > 1)
            {
                MessageBox.Show("You can only delete one event at a time.", "Edit Event", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("You must select an event to edit.", "Edit Event", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void pastEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlScheduler.SelectTab(tabPagePast);
        }

        private void schedulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlScheduler.SelectTab(tabPageUpcoming);
        }

        //Column Click event for Upcoming Events

        private int sortUpcomingColumn = -1;

        private void lviewUpcomingEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortUpcomingColumn)
            {
                sortUpcomingColumn = e.Column;
                lviewUpcomingEvents.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (lviewUpcomingEvents.Sorting == SortOrder.Ascending)
                    lviewUpcomingEvents.Sorting = SortOrder.Descending;
                else
                    lviewUpcomingEvents.Sorting = SortOrder.Ascending;
            }

            lviewUpcomingEvents.Sort();

            this.lviewUpcomingEvents.ListViewItemSorter = new ListViewItemComparer(e.Column, lviewUpcomingEvents.Sorting);
        }

        //Column Click event for Past Events

        private int sortPastColumn = -1;

        private void lviewPastEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortPastColumn)
            {
                sortPastColumn = e.Column;
                lviewPastEvents.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (lviewPastEvents.Sorting == SortOrder.Ascending)
                    lviewPastEvents.Sorting = SortOrder.Descending;
                else
                    lviewPastEvents.Sorting = SortOrder.Ascending;
            }

            lviewPastEvents.Sort();

            this.lviewPastEvents.ListViewItemSorter = new ListViewItemComparer(e.Column, lviewPastEvents.Sorting);
        }

        public class ListViewItemComparer : IComparer
        {

            private int col;
            private SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }
            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y)
            {
                int returnVal = -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                ((ListViewItem)y).SubItems[col].Text);

                if (order == SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }


        }

        private void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this event?", "Delete Event", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                foreach (ListViewItem item in lviewUpcomingEvents.SelectedItems)
                {                 
                    Globals.allTasks.remove((ScheduledEvent)item.Tag);
                }
            }
        }

        private void lviewUpcomingEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lviewUpcomingEvents.SelectedIndices.Count > 0)
            {
                btnDeleteEvent.Enabled = true;
                btnEditEvent.Enabled = true;
            }
            else
            {
                btnDeleteEvent.Enabled = false;
                btnEditEvent.Enabled = false;
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DestroyHandle();
        }

        private void frmScheduler_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void sendToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private frmSettings settings; 

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings = new frmSettings();
            settings.ShowDialog();
        }

        private frmCalendar calendar;

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar = new frmCalendar();
            calendar.Show();
        }

        private void tmrListViewNotice_Tick(object sender, EventArgs e)
        {
            if (lviewUpcomingEvents.Items.Count == 0)
            {
                lblAreEventsScheduled.Visible = true;
            }
            else
            {
                lblAreEventsScheduled.Visible = false;
            }
        }

        System.Drawing.Color StartColor;
        System.Drawing.Color EndColor;

        public System.Drawing.Color PageStartColor
        {
            get
            {
                return StartColor;
            }
            set
            {
                StartColor = value;
                RepaintControls();
            }
        }


        public System.Drawing.Color PageEndColor
        {
            get
            {
                return EndColor;
            }
            set
            {
                EndColor = value;
                RepaintControls();
            }
        }

        private void RepaintControls()
        {
            StartColor = Globals.settings.schedulerColor1;
            EndColor = Globals.settings.schedulerColor2;
            foreach (TabPage ctl in tabControlScheduler.TabPages) { 
                System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
                gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new
                Point(0, 0),
                new Point(ctl.Width, ctl.Height), PageStartColor, PageEndColor);

                Bitmap bmp = new Bitmap(ctl.Width, ctl.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gradBrush, new Rectangle(0, 0, ctl.Width,
            ctl.Height));
            ctl.BackgroundImage = bmp;
            ctl.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            lblTimeDisplay.ForeColor = Globals.settings.schedulerTextColor;
            lblUpcomingEvents.ForeColor = Globals.settings.schedulerTextColor;
            lblPastEventsLog.ForeColor = Globals.settings.schedulerTextColor;
            // Calling the base class OnPaint
            //base.OnPaint(pe);
            
            RepaintControls();
        }
    }
}
