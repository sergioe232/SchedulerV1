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
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Globals.settings.Save();
            this.Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            numericOverlayEvents.Value = Globals.settings.maxOverlayTasks;
            numericNoticeMinutes.Value = Globals.settings.eventNoticeMinutes;

            if (Globals.settings.isOverlayEnabled == true)
            {
                chkOverlay.Checked = true;
            }

            else if (Globals.settings.isOverlayEnabled == false)
            {
                chkOverlay.Checked = false;
            }

        }

        private void chkOverlay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOverlay.Checked == false)
            {
                Globals.settings.isOverlayEnabled = false;
                Globals.overlay.Hide();
            }
            else if (chkOverlay.Checked == true)
            {
                Globals.settings.isOverlayEnabled= true;
                Globals.overlay.Show();
            }
        }

        private void numericOverlayEvents_ValueChanged(object sender, EventArgs e)
        {
            Globals.settings.maxOverlayTasks = (int)numericOverlayEvents.Value;
            Globals.overlay.refresh();
        }

        private void numericNoticeMinutes_ValueChanged(object sender, EventArgs e)
        {
            Globals.settings.eventNoticeMinutes = (int)numericNoticeMinutes.Value;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Globals.settings.overlayBackgroundColor;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.BackColor = Globals.settings.overlayTextColor;
        }

        private void btnOverlayBackColor_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            Globals.settings.overlayBackgroundColor = colorPicker.Color;
            panel1.BackColor = colorPicker.Color;
            Globals.overlay.refresh();
        }

        private void btnOverlayForeColor_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            Globals.settings.overlayTextColor= colorPicker.Color;
            panel2.BackColor = colorPicker.Color;
            Globals.overlay.refresh();
        }

        private void btnSchColor1_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            Globals.settings.schedulerColor1 = colorPicker.Color;
            panel3.BackColor = colorPicker.Color;
        }

        private void btnSchColor2_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            Globals.settings.schedulerColor2 = colorPicker.Color;
            panel4.BackColor = colorPicker.Color;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            panel3.BackColor = Globals.settings.schedulerColor1;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            panel4.BackColor = Globals.settings.schedulerColor2;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            panel5.BackColor = Globals.settings.schedulerTextColor;
        }

        private void btnSchTextColor_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            Globals.settings.schedulerTextColor = colorPicker.Color;
            panel5.BackColor = colorPicker.Color;
        }
    }
}
