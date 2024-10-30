using System;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;

namespace SalesInventorySystem_WAM1
{
    public partial class frmInventory : Form
    {
        private ItemHandler item_handler = new ItemHandler();
        private int selected_item = -1;

        public frmInventory()
        {
            InitializeComponent();
            UpdateItemsList();
        }

        public void UpdateItemsList()
        {
            var items = item_handler.GetAllItems();
            dgvInventory.Rows.Clear();
            foreach (var item in items)
            {
                dgvInventory.Rows.Add(
                    item.Id,
                    item.Name,
                    item.Category,
                    item.UnitPrice,
                    item.Stock,
                    item.DateAdded == null
                        ? "N/A"
                        : item.DateAdded.Date.ToString("yyyy-MM-dd HH:mm:ss")
                );
            }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int item_id = (int)dgvInventory.Rows[e.RowIndex].Cells["id"].Value;
            selected_item = item_id;
            var item = item_handler.GetItem(item_id);

            txtItemID.Text = item.Id.ToString();
            txtName.Text = item.Name;
            switch (item.Category)
            {
                case "general":
                    cbCategory.SelectedIndex = 0;
                    break;
                case "electronic":
                    cbCategory.SelectedIndex = 1;
                    break;
                default:
                    MessageBox.Show("Item has invalid category.");
                    cbCategory.SelectedIndex = -1;
                    break;
            }
            txtUnitPrice.Text = item.UnitPrice.ToString();
            txtStock.Text = item.Stock.ToString();
            dtpDate.Value = item.DateAdded;
        }

        private void btnAdd_Click(object sender, EventArgs e) { }

        private void btnClear_Click(object sender, EventArgs e)
        {
            selected_item = -1;
            txtItemID.Text = string.Empty;
            txtName.Text = string.Empty;
            cbCategory.SelectedIndex = -1;
            txtUnitPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            dtpDate.Value = DateTime.Now;
        }
    }
}
