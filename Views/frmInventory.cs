using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BarcodeStandard;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;
using SkiaSharp;

namespace SalesInventorySystem_WAM1
{
    public partial class frmInventory : Form
    {
        private ItemHandler item_handler = new ItemHandler();
        private int selected_item = -1;

        public frmInventory()
        {
            InitializeComponent();
            UpdateItemsList(null);
        }

        /// <summary>
        /// Updates the items list in the DataGridView.
        /// </summary>
        /// <param name="query">If not null, search for items with details containing this query.</param>
        public void UpdateItemsList(string query)
        {
            var items =
                query == null ? item_handler.GetAllItems() : item_handler.SearchItems(query);
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

        /// <summary>
        /// Validates the values of the components in the form.
        /// </summary>
        /// <returns>If the values are valid.</returns>
        public bool ValidateComponentValues()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the item.");
                return false;
            }
            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category for the item.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Please enter a unit price for the item.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Please enter a stock quantity for the item.");
                return false;
            }

            if (!double.TryParse(txtUnitPrice.Text, out double _))
            {
                MessageBox.Show("Invalid unit price.");
                return false;
            }
            if (!int.TryParse(txtStock.Text, out int _))
            {
                MessageBox.Show("Invalid stock quantity.");
                return false;
            }
            if (double.Parse(txtUnitPrice.Text) < 0)
            {
                MessageBox.Show("Unit price cannot be negative.");
                return false;
            }
            if (int.Parse(txtStock.Text) < 0)
            {
                MessageBox.Show("Stock quantity cannot be negative.");
                return false;
            }

            return true;
        }

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return; // do nothing if the header is clicked
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

            var bcode = new Barcode();
            bcode.IncludeLabel = true;
            bcode.ForeColor = SKColors.Black;
            bcode.BackColor = SKColors.White;
            bcode.Encode(BarcodeStandard.Type.Code11, item.Id.ToString());
            MemoryStream ms = new MemoryStream(bcode.EncodedImageBytes);
            picBarcode.Image = Image.FromStream(ms);
            lblBcode.Text = string.Join("     ", item.Id.ToString().ToCharArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateComponentValues())
                return;

            txtItemID.Text = item_handler.GenerateItemId().ToString();
            dtpDate.Value = DateTime.Now;

            item_handler.AddItem(
                new Item
                {
                    Id = int.Parse(txtItemID.Text),
                    Name = txtName.Text,
                    Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                    UnitPrice = double.Parse(txtUnitPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    DateAdded = dtpDate.Value,
                }
            );

            UpdateItemsList(null);
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
            picBarcode.Image = null;
            lblBcode.Text = string.Empty;
            UpdateItemsList(null);
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
                UpdateItemsList(null);
                MessageBox.Show("Item deleted successfully.");
                btnClear_Click(sender, e);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!ValidateComponentValues())
                return;

            dtpDate.Value = DateTime.Now;

            item_handler.UpdateItem(
                new Item
                {
                    Id = selected_item,
                    Name = txtName.Text,
                    Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                    UnitPrice = double.Parse(txtUnitPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    DateAdded = dtpDate.Value,
                }
            );

            UpdateItemsList(null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchbar = new Searchbar();
            searchbar.searchbar_title = "Search Items";
            var query = searchbar.ShowDialog();
            if (query == DialogResult.OK)
                UpdateItemsList(searchbar.query);
        }
    }
}
