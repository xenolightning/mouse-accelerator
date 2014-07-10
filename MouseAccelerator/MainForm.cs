using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MouseAccelerator
{
    public partial class MainForm : Form
    {
        public const string MouseAccelerationTooltip = "Mouse Acceleration";

        public bool IsMouseAccelerationActive
        {
            get { return MouseHelper.GetMouse()[2] > 0; }
        }

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;

            RefreshMouseState();
        }

        private void RefreshMouseState()
        {
            notifyIcon.Text = String.Format("{0} - {1}", MouseAccelerationTooltip, IsMouseAccelerationActive ? "On" : "Off");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visible = false;
            notifyIcon.Visible = true;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int speed = 0;
            MouseHelper.GetMouseSpeed(out speed);

            if (IsMouseAccelerationActive)
                MouseHelper.SetMouse(0, 0, false);
            else
                MouseHelper.SetMouse(6, 10, true);

            MouseHelper.SetMouseSpeed((uint)speed);

            RefreshMouseState();
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshMouseState();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Close();
        }
    }
}
