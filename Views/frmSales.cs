using System;
using System.Windows.Forms;
using SalesInventorySystem_WAM1.Handlers;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1
{
    public partial class frmSales : Form
    {
        private ItemHandler ih = new ItemHandler();
        private SalesHandler sh = new SalesHandler();
        private DateTime selected_transaction = DateTime.MinValue;

        public frmSales()
        {
            InitializeComponent();
            UpdateItemsList();
        }

        private void UpdateItemsList()
        {
            cbItem.Items.Clear();
            foreach (var item in ih.GetAllItems())
                cbItem.Items.Add($"[{item.Id}] {item.Name}");

            foreach (var transaction in sh.GetAllTransactions())
                dgvSales.Rows.Add(
                    transaction.Id,
                    transaction.ItemId,
                    transaction.Category,
                    transaction.Price,
                    transaction.Quantity,
                    transaction.Status,
                    transaction.Notes
                );
        }

        private bool ValidateValues()
        {
            if (cbItem.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a category.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            if (txtQuantity.Text == string.Empty)
            {
                MessageBox.Show(
                    "Please enter a quantity.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            if (int.TryParse(txtQuantity.Text, out int _) == false)
            {
                MessageBox.Show(
                    "Quantity must be a number.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            if (txtPrice.Text == string.Empty)
            {
                MessageBox.Show(
                    "Please select an item and enter the quantity to get a price.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            return true;
        }

        private void FillUpValues()
        {
            // Get the item data
            if (cbItem.SelectedIndex != -1)
            {
                int item_id = int.Parse(cbItem.Text.Split(']')[0].Substring(1));
                var item = ih.GetItem(item_id);
                // Fill up the category combobox
                cbCategory.SelectedIndex = item.Category == "general" ? 0 : 1;
                // Fill up the price textbox if the item and quantity are selected/entered.
                if (int.TryParse(txtQuantity.Text, out int _))
                    txtPrice.Text = (item.UnitPrice * int.Parse(txtQuantity.Text)).ToString();

                txtStatus.Text = "Unpaid";
                return;
            }
            txtPrice.Text = string.Empty;
            txtStatus.Text = string.Empty;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            selected_transaction = DateTime.MinValue;
            txtTransactionID.Text = string.Empty;
            cbItem.SelectedIndex = -1;
            cbCategory.SelectedIndex = -1;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateValues())
                return;

            sh.AddTransaction(
                new Transaction
                {
                    Id = DateTime.Now,
                    ItemId = int.Parse(cbItem.Text.Split(']')[0].Substring(1)),
                    Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                    Quantity = int.Parse(txtQuantity.Text),
                    Price = double.Parse(txtPrice.Text),
                    Status = txtStatus.Text,
                    Notes = txtNotes.Text,
                }
            );

            MessageBox.Show(
                "Transaction added successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            btnClear.PerformClick();
            UpdateItemsList();
        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e) => FillUpValues();

        private void txtQuantity_TextChanged(object sender, EventArgs e) => FillUpValues();

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var trans_id = (DateTime)dgvSales.Rows[e.RowIndex].Cells["id"].Value;
            var transaction = sh.GetTransaction(trans_id);

            selected_transaction = trans_id;
            txtTransactionID.Text = trans_id.ToString();
            cbItem.SelectedIndex = cbItem.FindString($"[{transaction.ItemId}]");
            cbCategory.SelectedIndex = transaction.Category == "general" ? 0 : 1;
            txtPrice.Text = transaction.Price.ToString();
            txtQuantity.Text = transaction.Quantity.ToString();
            txtStatus.Text = transaction.Status;
            txtNotes.Text = transaction.Notes;
        }
    }
}
