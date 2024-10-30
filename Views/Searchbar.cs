using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SalesInventorySystem_WAM1
{
    public partial class Searchbar : Form
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

        public string searchbar_title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public string query = "";

        public Searchbar()
        {
            InitializeComponent();
            //Border
            Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRGN(0, 0, Width, Height, 25, 25)
            );
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Dispose();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            query = txtQuery.Text;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
    }
}
