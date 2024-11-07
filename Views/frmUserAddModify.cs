using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1
{
    public partial class frmUserAddModify : Form
    {
        private UserHandler user_handler = new UserHandler();
        private MainForm mainForm;

        public int user_id;
        public bool stay_alive = false;
        private bool password_changed = false;

        public string pub_txtName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }
        public string pub_cbRole
        {
            get
            {
                switch (cbRole.SelectedIndex)
                {
                    case 0:
                        return "employee";
                    case 1:
                        return "admin";
                    default:
                        return string.Empty;
                }
            }
            set
            {
                switch (value)
                {
                    case "employee":
                        cbRole.SelectedIndex = 0;
                        break;
                    case "admin":
                        cbRole.SelectedIndex = 1;
                        break;
                    default:
                        cbRole.SelectedIndex = -1;
                        break;
                }
            }
        }

        public string pub_txtUsername
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string pub_btnRegister
        {
            get { return btnBack.Text; }
            set { btnRegister.Text = value; }
        }

        public bool pub_RoleEnabled
        {
            get { return cbRole.Enabled; }
            set { cbRole.Enabled = value; }
        }

        public bool pub_btnDeleteVisible
        {
            get { return btnDelete.Visible; }
            set { btnDelete.Visible = value; }
        }

        public bool pub_btnBackVisible
        {
            get { return btnBack.Visible; }
            set { btnBack.Visible = value; }
        }

        public frmUserAddModify(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!stay_alive)
            {
                //Form Loading
                //lblMenu.Text = "Sales";
                mainForm.PnlFormLoader.Controls.Clear();
                frmUsers FrmUsers_Vrb = new frmUsers(mainForm)
                {
                    Dock = DockStyle.Fill,
                    TopLevel = false,
                    TopMost = true,
                };
                FrmUsers_Vrb.FormBorderStyle = FormBorderStyle.None;
                mainForm.PnlFormLoader.Controls.Add(FrmUsers_Vrb);
                FrmUsers_Vrb.Show();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Invalid Username");
                return;
            }
            if (password_changed || user_id == -1)
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Invalid Password");
                    return;
                }
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match");
                    return;
                }
            }
            if (cbRole.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a user role");
                return;
            }

            if (user_id == -1)
            {
                try
                {
                    user_handler.AddUser(
                        new User(
                            -1,
                            txtUsername.Text,
                            UserHandler.EncryptPassword(txtPassword.Text),
                            txtName.Text,
                            cbRole.SelectedIndex == 1 ? "admin" : "employee",
                            DateTime.Now
                        )
                    );
                    MessageBox.Show("User added successfully.");
                }
                catch (MySqlException exc)
                {
                    MessageBox.Show(
                        $"A database-related error occured: {exc.Message}\n\nFailed to register user.",
                        "Database Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                if (!stay_alive)
                {
                    var mainfrm = new frmUsers(mainForm)
                    {
                        Dock = DockStyle.Fill,
                        TopLevel = false,
                        TopMost = true,
                    };
                    // Go back to the Users form
                    mainForm.PnlFormLoader.Controls.Add(mainfrm);
                    mainfrm.Show();
                    this.Dispose();
                }
            }

            // Update user information
            try
            {
                if (password_changed)
                {
                    user_handler.UpdateUser(
                        new User(
                            user_id,
                            txtUsername.Text,
                            UserHandler.EncryptPassword(txtPassword.Text),
                            txtName.Text,
                            cbRole.SelectedIndex == 1 ? "admin" : "employee",
                            DateTime.Now
                        )
                    );
                }
                else
                {
                    user_handler.UpdateUser(
                        new User(
                            user_id,
                            txtUsername.Text,
                            user_handler.GetUser(user_id).Password,
                            txtName.Text,
                            cbRole.SelectedIndex == 1 ? "admin" : "employee",
                            DateTime.Now
                        )
                    );
                }
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to update user information.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            MessageBox.Show("User updated successfully.");
            if (!stay_alive)
            {
                var mainfrm2 = new frmUsers(mainForm)
                {
                    Dock = DockStyle.Fill,
                    TopLevel = false,
                    TopMost = true,
                };
                // Go back to the Users form
                mainForm.PnlFormLoader.Controls.Add(mainfrm2);
                mainfrm2.Show();
                this.Dispose();
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = chkShowPassword.Checked;
            txtConfirmPassword.UseSystemPasswordChar = chkShowPassword.Checked;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) => password_changed = true;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (user_id == -1)
            {
                MessageBox.Show(
                    "You are not modifying an existing user, so there's nothing to delete."
                );
                return;
            }

            if (
                MessageBox.Show(
                    "Are you sure you want to delete this user?",
                    "Delete User",
                    MessageBoxButtons.YesNo
                ) == DialogResult.Yes
            )
            {
                try
                {
                    user_handler.DeleteUser(user_id);
                    MessageBox.Show("User deleted successfully.");
                    btnBack.PerformClick();
                }
                catch (MySqlException exc)
                {
                    MessageBox.Show(
                        $"A database-related error occured: {exc.Message}\n\nFailed to delete user information.",
                        "Database Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (user_id == -1)
            {
                txtName.Text = string.Empty;
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                cbRole.SelectedIndex = -1;
                return;
            }

            try
            {
                var u = user_handler.GetUser(user_id);

                txtName.Text = u.Name;
                txtUsername.Text = u.Username;
                cbRole.SelectedIndex = u.Role == "admin" ? 1 : 0;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                password_changed = false;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to get user information.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }
    }
}
