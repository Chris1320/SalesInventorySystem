using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;

namespace SalesInventorySystem_WAM1
{
    public partial class SplashScreen : Form
    {
        //Border
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRGN(
            int nLeftRect,
            int nTopTect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public SplashScreen()
        {
            InitializeComponent();

            //Border
            Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRGN(0, 0, Width, Height, 25, 25)
            );
            if (ee.c())
                lblCopyright.Text = ee.a();
        }

        private void tmrLoad_Tick(object sender, EventArgs e)
        {
            pnlLoading.Width += 10;

            if (pnlLoading.Width >= 599)
            {
                tmrLoad.Stop();
                var newfrm = new LoginForm();
                newfrm.Closed += (s, args) => this.Close();
                this.Hide();
                newfrm.Show();
            }
        }
    }
}
