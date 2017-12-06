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
    public partial class frmCalendar : Form
    {
        public frmCalendar()
        {
            InitializeComponent();
        }

        private void tmrTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
            lblBarTime.Text = DateTime.Now.ToShortTimeString();
            lblDateBar.Text = (DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
            lblBarDayName.Text = (" " + DateTime.Today.ToString("ddd").Substring(0, 2));

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 18)
            {
                pbTopRight.BackgroundImage = ProjectPickleRick.Properties.Resources.backgroundday;
            }
            else
                pbTopRight.BackgroundImage = ProjectPickleRick.Properties.Resources.backgroundnight;

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 8)
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.EarlyMorning;
            }
            else if(DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 12)
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.Morning;
            }
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.Afternoon;
            }
            else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 22)
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.Evening;
            }
            else if ((DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 24) || (DateTime.Now.Hour >= 1 && DateTime.Now.Hour < 6))
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.Night;
            }
            else if (DateTime.Now.Hour == 24)
            {
                pbTopRight.Image = ProjectPickleRick.Properties.Resources.Midnight;
            }
        }

        private void btnOpenScheduler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (Today.Day == DateTime.Today.Day)
            {
                panel1.BackColor = Color.Yellow;
                lblToday.ForeColor = Color.Black;
                lblTodayNumber.ForeColor = Color.Black;
            }
            else
            {
                panel1.BackColor = Color.Transparent;
                lblToday.ForeColor = Color.White;
                lblTodayNumber.ForeColor = Color.White;
            }
                
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.DodgerBlue;
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.DodgerBlue;
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            panel3.BackColor = Color.DodgerBlue;
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            panel3.BackColor = Color.Transparent;
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            panel4.BackColor = Color.DodgerBlue;
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.DodgerBlue;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Transparent;
        }

        private void panel6_MouseEnter(object sender, EventArgs e)
        {
            panel6.BackColor = Color.DodgerBlue;
        }

        private void panel6_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void panel7_MouseEnter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.DodgerBlue;
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        DateTime Day0 = DateTime.Today.AddDays(-1);
        DateTime Today = DateTime.Today;
        DateTime Day1 = DateTime.Today.AddDays(1);
        DateTime Day2 = DateTime.Today.AddDays(2);
        DateTime Day3 = DateTime.Today.AddDays(3);
        DateTime Day4 = DateTime.Today.AddDays(4);
        DateTime Day5 = DateTime.Today.AddDays(5);

        private void frmCalendar_Load(object sender, EventArgs e)
        {
            lblBarTime.BackColor = ColorTranslator.FromHtml("#161616");
            lblDateBar.BackColor = ColorTranslator.FromHtml("#161616");
            lblBarDayName.BackColor = ColorTranslator.FromHtml("#161616");

            refreshUpcomingEvents();
            refreshCalendar();
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            addWeek();
            refreshUpcomingEvents();
            refreshCalendar();
        }

        private void addWeek()
        {
            Today = Today.AddDays(7);
            Day0 = Day0.AddDays(7);
            Day1 = Day1.AddDays(7);
            Day2 = Day2.AddDays(7);
            Day3 = Day3.AddDays(7);
            Day4 = Day4.AddDays(7);
            Day5 = Day5.AddDays(7);
        }

        private void refreshCalendar()
        {
            lblCurrentMonth.Text = Today.Month.ToString();
            lblCurrentYear.Text = Today.Year.ToString();

            lblToday.Text = Today.ToString("ddd");
            lblDay0.Text = Day0.ToString("ddd");
            lblDay1.Text = Day1.ToString("ddd");
            lblDay2.Text = Day2.ToString("ddd");
            lblDay3.Text = Day3.ToString("ddd");
            lblDay4.Text = Day4.ToString("ddd");
            lblDay5.Text = Day5.ToString("ddd");

            lblTodayNumber.Text = Today.Day.ToString();
            lblDay0Number.Text = Day0.Day.ToString();
            lblDay1Number.Text = Day1.Day.ToString();
            lblDay2Number.Text = Day2.Day.ToString();
            lblDay3Number.Text = Day3.Day.ToString();
            lblDay4Number.Text = Day4.Day.ToString();
            lblDay5Number.Text = Day5.Day.ToString();

            if (Today.Day == DateTime.Today.Day)
            {
                panel1.BackColor = Color.Yellow;
                lblToday.ForeColor = Color.Black;
                lblTodayNumber.ForeColor = Color.Black;
            }
            else
            {
                panel1.BackColor = Color.Transparent;
                lblToday.ForeColor = Color.White;
                lblTodayNumber.ForeColor = Color.White;
            }
        }

        private void refreshUpcomingEvents()
        {
            var upcomingEventsList = Globals.allTasks.getUpcomingEvents();
            pictureBoxDay0.Visible = false;
            pictureBoxToday.Visible = false;
            pictureDay1.Visible = false;
            pictureDay2.Visible = false;
            pictureDay3.Visible = false;
            pictureDay4.Visible = false;
            pictureDay5.Visible = false;

            foreach (var task in upcomingEventsList)
            {
                var taskDate = task.time.Date;
                
                if (taskDate == Day0.Date)
                {
                    pictureBoxDay0.Visible = true;
                    continue;
                }
                if (taskDate == Today.Date)
                {
                    pictureBoxToday.Visible = true;
                    continue;
                }
                else if (taskDate == Day1.Date)
                {
                    pictureDay1.Visible = true;
                    continue;
                }
                else if (taskDate == Day2.Date)
                {
                    pictureDay2.Visible = true;
                    continue;
                }
                else if (taskDate == Day3.Date)
                {
                    pictureDay3.Visible = true;
                    continue;
                }
                else if (taskDate == Day4.Date)
                {
                    pictureDay4.Visible = true;
                    continue;
                }
                else if (taskDate == Day5.Date)
                {
                    pictureDay5.Visible = true;
                    continue;
                }
            }
        }

        private void subtractWeek()
        {
            Today = Today.AddDays(-7);
            Day0 = Day0.AddDays(-7);
            Day1 = Day1.AddDays(-7);
            Day2 = Day2.AddDays(-7);
            Day3 = Day3.AddDays(-7);
            Day4 = Day4.AddDays(-7);
            Day5 = Day5.AddDays(-7);
        }

        private void btnPreviousWeek_Click(object sender, EventArgs e)
        {
            subtractWeek();
            refreshUpcomingEvents();
            refreshCalendar();
        }       

        private String checkUpcomingEvents(DateTime day)
        {
            String eventsData = null;
            var upcomingEventsList = Globals.allTasks.getUpcomingEvents();

            foreach (var task in upcomingEventsList)
            {
                var taskDate = task.time.Date;
                if(taskDate == day.Date)
                {
                    eventsData = eventsData + taskDate + "\nEvent: " + task.taskName + "\nTime: " + task.time.ToShortTimeString() + "\n\n";
                }
            }

            if(eventsData == null)
            {
                eventsData = "No Events Scheduled";
            }
            return eventsData;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Today), "Events for " + Today.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day0), "Events for " + Day0.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day1), "Events for " + Day1.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day2), "Events for " + Day2.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day3), "Events for " + Day3.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel6_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day4), "Events for " + Day4.ToShortDateString(), MessageBoxButtons.OK);
        }

        private void panel7_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(checkUpcomingEvents(Day5), "Events for " + Day5.ToShortDateString(), MessageBoxButtons.OK);
        }
    }
}
