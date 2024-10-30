using System;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

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

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the item.");
                return;
            }
            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category for the item.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Please enter a unit price for the item.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Please enter a stock quantity for the item.");
                return;
            }

            if (!double.TryParse(txtUnitPrice.Text, out double unit_price))
            {
                MessageBox.Show("Invalid unit price.");
                return;
            }
            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Invalid stock quantity.");
                return;
            }

            txtItemID.Text = item_handler.GenerateItemId().ToString();
            dtpDate.Value = DateTime.Now;

            item_handler.AddItem(
                new Item
                {
                    Id = int.Parse(txtItemID.Text),
                    Name = txtName.Text,
                    Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                    UnitPrice = unit_price,
                    Stock = stock,
                    DateAdded = dtpDate.Value,
                }
            );

            UpdateItemsList();
            selected_item = Convert.ToInt32(txtItemID.Text);
        }

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selected_item == -1)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
            if (
                MessageBox.Show(
                    "Are you sure you want to delete this item?",
                    "Delete Item",
                    MessageBoxButtons.YesNo
                ) == DialogResult.Yes
            )
            {
                item_handler.DeleteItem(selected_item);
                UpdateItemsList();
                MessageBox.Show("Item deleted successfully.");
                btnClear_Click(sender, e);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (selected_item == -1)
            {
                MessageBox.Show("Please select an item to modify.");
                return;
            }
        }
    }
}
