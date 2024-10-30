using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;

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

        public LoginForm()
        {
            InitializeComponent();
            //Border
            Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRGN(0, 0, Width, Height, 25, 25)
            );
        }

        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Text = "Logging in...";
            btnLogin.Enabled = false;
            var user_handler = new UserHandler();
            var user = user_handler.Login(txtUsername.Text, txtPassword.Text);
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
    }
}
