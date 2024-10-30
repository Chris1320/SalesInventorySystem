using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1
{
    public partial class MainForm : Form
    {
        User user = null;

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

        public MainForm(User user)
        {
            this.user = user;

            InitializeComponent();
            //Border
            Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRGN(0, 0, Width, Height, 25, 25)
            );
            lblUserName.Text = user.Username;
            lblRole.Text = user.Role;
            btnUsers.Visible = user.Role == "admin";
            navSales(); //Default Navigation
        }

        private void navSales()
        {
            //Default Navigation
            pnlNav.Height = btnSales.Height;
            pnlNav.Top = btnSales.Top;
            pnlNav.Left = btnSales.Left;
            btnSales.BackColor = Color.FromArgb(46, 51, 73);

            //Form Loading
            lblMenu.Text = "Sales";
            this.PnlFormLoader.Controls.Clear();
            frmSales FrmSales_Vrb = new frmSales()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true,
            };
            FrmSales_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmSales_Vrb);
            FrmSales_Vrb.Show();
        }

        private void btnSales_Click(object sender, EventArgs e) => navSales(); //Default Navigation

        private void btnInventory_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnInventory.Height;
            pnlNav.Top = btnInventory.Top;
            pnlNav.Left = btnInventory.Left;
            btnInventory.BackColor = Color.FromArgb(46, 51, 73);

            //Form Loading
            lblMenu.Text = "Inventory";
            this.PnlFormLoader.Controls.Clear();
            frmInventory FrmInventory_Vrb = new frmInventory()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true,
            };
            FrmInventory_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmInventory_Vrb);
            FrmInventory_Vrb.Show();
        }

        private void btnSales_Leave(object sender, EventArgs e) =>
            btnSales.BackColor = Color.FromArgb(24, 30, 54);

        private void btnInventory_Leave(object sender, EventArgs e) =>
            btnInventory.BackColor = Color.FromArgb(24, 30, 54);

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes) Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e) //btnUsers
        {
            pnlNav.Height = btnUsers.Height;
            pnlNav.Top = btnUsers.Top;
            pnlNav.Left = btnUsers.Left;
            btnUsers.BackColor = Color.FromArgb(46, 51, 73);

            //Form Loading
            lblMenu.Text = "Users";
            this.PnlFormLoader.Controls.Clear();
            frmUsers FrmUsers_Vrb = new frmUsers(this)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true,
            };
            FrmUsers_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmUsers_Vrb);
            FrmUsers_Vrb.Show();
        }

        private void btnUsers_Leave(object sender, EventArgs e) =>
            btnUsers.BackColor = Color.FromArgb(24, 30, 54);

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to log out?",
                "Log Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.No) return;

            var f = new LoginForm();
            f.Closed += (s, args) => this.Close();
            this.Hide();
            f.Show();
        }
    }
}
