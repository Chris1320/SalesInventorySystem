using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

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
            UpdateUsersList(null);
        }

        /// <summary>
        /// Go to the User Add/Modify form.
        /// </summary>
        /// <param name="user_id">The ID of the user to modify. -1 if adding a new user.</param>
        private void gotofrmUserAddModify(int user_id)
        {
            try
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
                else
                    frmUserAddModify_Vrb.user_id = -1;

                frmUserAddModify_Vrb.FormBorderStyle = FormBorderStyle.None;
                mainForm.PnlFormLoader.Controls.Add(frmUserAddModify_Vrb);
                frmUserAddModify_Vrb.Show();
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to acquire user information.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        /// <summary>
        /// Updates the users list in the DataGridView.
        /// </summary>
        /// <param name="query">If not null, search for users with details containing this query.</param>
        public void UpdateUsersList(string query)
        {
            try
            {
                var users =
                    query == null ? user_handler.GetAllUsers() : user_handler.SearchUsers(query);
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
                            : user.LastLogin.ToString("yyyy-MM-dd HH:mm:ss")
                    );
                }
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to get user list.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
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
            if (e.RowIndex < 0)
                return;
            int user_id = (int)dgvUsers.Rows[e.RowIndex].Cells["id"].Value;
            User user;
            try
            {
                user = user_handler.GetUser(user_id);
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

            selected_user = user_id;
            txtUsername.Text = user.Username;
            txtName.Text = user.Name;
            switch (user.Role)
            {
                case "admin":
                    cbRole.SelectedIndex = 1;
                    break;
                case "employee":
                    cbRole.SelectedIndex = 0;
                    break;
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
            UpdateUsersList(null);
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

            try
            {
                user_handler.DeleteUser(selected_user);
                UpdateUsersList(null);
                btnClear.PerformClick();
                MessageBox.Show("User deleted successfully.");
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to delete user.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchbar = new Searchbar();
            searchbar.searchbar_title = "Search Users";
            var query = searchbar.ShowDialog();
            if (query == DialogResult.OK)
                UpdateUsersList(searchbar.query);
        }
    }
}
