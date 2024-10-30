using System;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;

namespace SalesInventorySystem_WAM1
{
    public partial class frmUsers : Form
    {
        private UserHandler user_handler = new UserHandler();
        private MainForm mainForm;
        private int selected_user = -1;

        public frmUsers(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            UpdateUsersList();
        }

        private void gotofrmUserAddModify(int user_id)
        {
            //Form Loading
            //lblMenu.Text = "Sales";
            mainForm.PnlFormLoader.Controls.Clear();
            var frmUserAddModify_Vrb = new frmUserAddModify(mainForm)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true,
            };
            if (user_id != -1)
            {
                var u = user_handler.GetUser(user_id);
                frmUserAddModify_Vrb.user_id = user_id;
                frmUserAddModify_Vrb.pub_txtName = u.Name;
                frmUserAddModify_Vrb.pub_cbRole = u.Role;
                frmUserAddModify_Vrb.pub_txtUsername = u.Username;
                frmUserAddModify_Vrb.pub_btnRegister = "Modify User";
            }
            else frmUserAddModify_Vrb.user_id = -1;

            frmUserAddModify_Vrb.FormBorderStyle = FormBorderStyle.None;
            mainForm.PnlFormLoader.Controls.Add(frmUserAddModify_Vrb);
            frmUserAddModify_Vrb.Show();
        }

        public void UpdateUsersList()
        {
            var db = new UserHandler();
            var users = db.GetAllUsers();
            dgvUsers.Rows.Clear();
            foreach (var user in users)
            {
                dgvUsers.Rows.Add(
                    user.Id,
                    user.Username,
                    string.IsNullOrEmpty(user.Name) ? "N/A" : user.Name,
                    user.Role,
                    user.LastLogin == null
                        ? "N/A"
                        : user.LastLogin.Date.ToString("yyyy-MM-dd HH:mm:ss")
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) => gotofrmUserAddModify(-1);

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (selected_user == -1)
            {
                MessageBox.Show("Select a user first");
                return;
            }
            gotofrmUserAddModify(selected_user);
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int user_id = (int)dgvUsers.Rows[e.RowIndex].Cells["id"].Value;
            var user = user_handler.GetUser(user_id);

            selected_user = user_id;
            txtUsername.Text = user.Username;
            txtName.Text = user.Name;
            switch (user.Role)
            {
                case "admin": cbRole.SelectedIndex = 1; break;
                case "employee": cbRole.SelectedIndex = 0; break;
                default:
                    MessageBox.Show("User has invalid role.");
                    cbRole.SelectedIndex = -1;
                    break;
            }
            dtpDate.Value = user.LastLogin;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            selected_user = -1;
            txtName.Text = string.Empty;
            txtUsername.Text = string.Empty;
            cbRole.SelectedIndex = -1;
            dtpDate.Value = DateTime.Now;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selected_user == -1)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            if (
                MessageBox.Show(
                    "Are you sure you want to delete this user?",
                    "Delete User",
                    MessageBoxButtons.YesNo
                ) == DialogResult.No
            )
                return;

            user_handler.DeleteUser(selected_user);
            UpdateUsersList();
            MessageBox.Show("User deleted successfully.");
        }

        private void btnSearch_Click(object sender, EventArgs e) { }
    }
}
