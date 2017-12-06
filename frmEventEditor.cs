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
    public partial class frmEventEditor : Form
    {
        private ScheduledEvent task;

        public frmEventEditor(ScheduledEvent task = null)
        {
            InitializeComponent();
            var time = DateTime.Now;
            dtpTimePicker.Value = new DateTime(2000, 1, 1, time.Hour, time.Minute, 0, 0);
            dtpDatePicker.Value = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, 0);

            if (task != null)
            {
                this.task = task;

                dtpDatePicker.Value = task.time.Date;
                dtpTimePicker.Value = new DateTime(2000, 1, 1, 0, 0, 0, 0) + task.time.TimeOfDay;
                txtTask.Text = task.taskName;
                txtDescription.Text = task.taskDescription;
            }
        }

        private void btnEventEditorCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEventEditorScheduleEvent_Click(object sender, EventArgs e)
        {
            var time = dtpDatePicker.Value.Date.Add(dtpTimePicker.Value.TimeOfDay);
            var name = txtTask.Text;
            var desc = txtDescription.Text;

            if (time <= DateTime.Now)
            {
                MessageBox.Show("The selected time has already passed, please select a different time", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtTask.Text == "")
            {
                MessageBox.Show("Please assign a name to the scheduled event", "No Event Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var task = this.task;
                if (task == null)
                {
                    task = new ScheduledEvent();
                }

                task.time = time;
                task.taskName = name;
                task.taskDescription = desc;

                if (this.task == null)
                {
                    Globals.allTasks.add(task);
                }
                else
                {
                    Globals.allTasks.edit(task);
                }
                this.Close();
            }
        }

        private void frmEventEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
