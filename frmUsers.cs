using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesInventorySystem_WAM1
{
    public partial class frmUsers : Form
    {
        private MainForm mainForm;
        public frmUsers(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Form Loading
            //lblMenu.Text = "Sales";
            mainForm.PnlFormLoader.Controls.Clear();
            frmUserAddModify frmUserAddModify_Vrb = new frmUserAddModify(mainForm) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmUserAddModify_Vrb.FormBorderStyle = FormBorderStyle.None;
            mainForm.PnlFormLoader.Controls.Add(frmUserAddModify_Vrb);
            frmUserAddModify_Vrb.Show();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            //Form Loading
            //lblMenu.Text = "Sales";
            mainForm.PnlFormLoader.Controls.Clear();
            frmUserAddModify frmUserAddModify_Vrb = new frmUserAddModify(mainForm) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmUserAddModify_Vrb.FormBorderStyle = FormBorderStyle.None;
            mainForm.PnlFormLoader.Controls.Add(frmUserAddModify_Vrb);
            frmUserAddModify_Vrb.Show();
        }
    }
}
