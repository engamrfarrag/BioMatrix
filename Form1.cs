using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AttendanceService.Dispatcher;
using AttendanceService.HAL.Driver;
using AttendanceService.Managers;
using AttendanceService.Providers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        DevicesManager m;
        private bool allowshowdisplay = false;

        //[DllImport("user32")]
        //static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        //[DllImport("user32")]
        //static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        //const int MF_BYCOMMAND = 0;
        //const int MF_DISABLED = 2;
        //const int SC_CLOSE = 0xF060;
        public Form1()
        {
            InitializeComponent();
            m = new DevicesManager(new DefaultDeviceProvider(), new LogsManager());

            //var sm = GetSystemMenu(Handle, false);
            //EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);

        }
        private const int CP_DISABLE_CLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CP_DISABLE_CLOSE_BUTTON;
                return cp;
            }
        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.allowshowdisplay = true;
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }
    }
}
