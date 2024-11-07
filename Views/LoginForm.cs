using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1
{
    public partial class LoginForm : Form
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

        public Point mouseLocation;

        public LoginForm()
        {
            InitializeComponent();
            //Border
            Region = Region.FromHrgn(
                CreateRoundRectRGN(0, 0, Width, Height, 25, 25)
            );
        }

        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Text = "Logging in...";
            btnLogin.Enabled = false;
            UserHandler user_handler;
            User user;
            try
            {
                user_handler = new UserHandler();
                user = user_handler.Login(txtUsername.Text, txtPassword.Text);
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to log in.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                btnLogin.Text = "Login";
                btnLogin.Enabled = true;
                return;
            }

            if (user == null)
            {
                MessageBox.Show(
                    "Invalid Username or Password",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                btnLogin.Text = "Login";
                btnLogin.Enabled = true;
                return;
            }

            var mainForm = new MainForm(user);
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
            this.Hide();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked == true) txtPassword.PasswordChar = '\0';
            else txtPassword.PasswordChar = '*';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e) =>
            txtUsername.Text = txtUsername.Text.Replace("\n", "").Replace("\r", "").Trim();

        private void txtPassword_TextChanged(object sender, EventArgs e) =>
            txtPassword.Text = txtPassword.Text.Replace("\n", "").Replace("\r", "").Trim();

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(e.X, e.Y);
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int newX = this.Left + (e.X - mouseLocation.X);
                int newY = this.Top + (e.Y - mouseLocation.Y);

                this.Location = new Point(newX, newY);
            }
        }

        private void topPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseLocation = Point.Empty;
        }
    }
}
