using System;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1
{
    public partial class frmUserAddModify : Form
    {
        private DatabaseHandler db = new DatabaseHandler();
        private MainForm mainForm;

        public int user_id;
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
                    case 0: return "employee";
                    case 1: return "admin";
                    default: return string.Empty;
                }
            }
            set
            {
                switch (value)
                {
                    case "employee": cbRole.SelectedIndex = 0; break;
                    case "admin": cbRole.SelectedIndex = 1; break;
                    default: cbRole.SelectedIndex = -1; break;
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

        public frmUserAddModify(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //Form Loading
            //lblMenu.Text = "Sales";
            mainForm.PnlFormLoader.Controls.Clear();
            frmUsers FrmUsers_Vrb = new frmUsers(mainForm) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmUsers_Vrb.FormBorderStyle = FormBorderStyle.None;
            mainForm.PnlFormLoader.Controls.Add(FrmUsers_Vrb);
            FrmUsers_Vrb.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text)) { MessageBox.Show("Invalid Username"); return; }
            if (user_id != -1 && password_changed)
            {
                if (string.IsNullOrEmpty(txtPassword.Text)) { MessageBox.Show("Invalid Password"); return; }
                if (txtPassword.Text != txtConfirmPassword.Text) { MessageBox.Show("Passwords do not match"); return; }
            }
            if (cbRole.SelectedIndex == -1) { MessageBox.Show("You must select a user role"); return; }

            if (user_id == -1)
            {
                db.AddUser(
                    new User(
                        -1,
                        txtUsername.Text,
                        DatabaseHandler.EncryptPassword(txtPassword.Text),
                        txtName.Text,
                        cbRole.SelectedIndex == 1 ? "admin" : "employee",
                        DateTime.Now
                    )
                );
                MessageBox.Show("User added successfully.");
                var mainfrm = new frmUsers(mainForm)
                {
                    Dock = DockStyle.Fill,
                    TopLevel = false,
                    TopMost = true
                };
                // Go back to the Users form
                mainForm.PnlFormLoader.Controls.Add(mainfrm);
                mainfrm.Show();
                this.Dispose();
            }

            // Update user information
            if (password_changed)
            {
                db.UpdateUser(
                    new User(
                        user_id,
                        txtUsername.Text,
                        DatabaseHandler.EncryptPassword(txtPassword.Text),
                        txtName.Text,
                        cbRole.SelectedIndex == 1 ? "admin" : "employee",
                        DateTime.Now
                    )
                );
            }
            else
            {
                db.UpdateUser(
                    new User(
                        user_id,
                        txtUsername.Text,
                        db.GetUser(user_id).Password,
                        txtName.Text,
                        cbRole.SelectedIndex == 1 ? "admin" : "employee",
                        DateTime.Now
                    )
                );
            }

            MessageBox.Show("User updated successfully.");
            var mainfrm2 = new frmUsers(mainForm)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            // Go back to the Users form
            mainForm.PnlFormLoader.Controls.Add(mainfrm2);
            mainfrm2.Show();
            this.Dispose();
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
                MessageBox.Show("You are not modifying an existing user, so there's nothing to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.DeleteUser(user_id);
                MessageBox.Show("User deleted successfully.");
                btnBack.PerformClick();
            }
        }
    }
}
