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
    public partial class frmUserAddModify : Form
    {
        private MainForm mainForm;
        public frmUserAddModify(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void frmUserAddModify_Load(object sender, EventArgs e)
        {

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
    }
}
