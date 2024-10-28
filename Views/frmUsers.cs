using SalesInventorySystem_WAM1.Handlers;
using System;
using System.Windows.Forms;

namespace SalesInventorySystem_WAM1
{
    public partial class frmUsers : Form
    {
        private DatabaseHandler db = new Handlers.DatabaseHandler();
        private MainForm mainForm;
        private int selected_user;

        public frmUsers(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            UpdateUsersList();
        }

        private void gotofrmUserAddModify()
        {
            //Form Loading
            //lblMenu.Text = "Sales";
            mainForm.PnlFormLoader.Controls.Clear();
            var frmUserAddModify_Vrb = new frmUserAddModify(mainForm)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmUserAddModify_Vrb.FormBorderStyle = FormBorderStyle.None;
            mainForm.PnlFormLoader.Controls.Add(frmUserAddModify_Vrb);
            frmUserAddModify_Vrb.Show();
        }
        public void UpdateUsersList()
        {

            var db = new Handlers.DatabaseHandler();
            var users = db.GetAllUsers();
            dgvUsers.Rows.Clear();
            foreach (var user in users)
            {
                dgvUsers.Rows.Add(
                    user.Id,
                    user.Username,
                    user.Password
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) => gotofrmUserAddModify();
        private void btnModify_Click(object sender, EventArgs e) => gotofrmUserAddModify();

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int user_id = (int)dgvUsers.Rows[e.RowIndex].Cells["id"].Value;
            var user = db.GetUser(user_id);

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

            db.DeleteUser(selected_user);
            UpdateUsersList();
            MessageBox.Show("User deleted successfully.");
        }
    }
}
