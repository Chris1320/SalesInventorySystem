using System;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MySql.Data.MySqlClient;
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
            UpdateTransactionsList(null);
        }

        /// <summary>
        /// Updates the transactions list in the DataGridView.
        /// </summary>
        /// <param name="query">If not null, search for transactions with details containing this query.</param>
        private void UpdateTransactionsList(string query)
        {
            try
            {
                cbItem.Items.Clear();
                foreach (var item in ih.GetAllItems())
                    cbItem.Items.Add($"[{item.Id}] {item.Name}");

                var transactions =
                    query == null ? sh.GetAllTransactions() : sh.SearchTransactions(query);
                dgvSales.Rows.Clear();
                foreach (var transaction in transactions)
                    dgvSales.Rows.Add(
                        transaction.Id.ToString("yyyy-MM-dd HH:mm:ss"),
                        $"[{transaction.ItemId}] {ih.GetItem(transaction.ItemId).Name}",
                        transaction.Category,
                        transaction.Price,
                        transaction.Quantity,
                        transaction.Status,
                        transaction.Notes
                    );
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to update transactions list.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        /// <summary>
        /// Validates the values of the components in the form.
        /// </summary>
        /// <returns>If the values are valid.</returns>
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
            if (int.Parse(txtQuantity.Text) < 0)
            {
                MessageBox.Show(
                    "Quantity must be a positive number.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            if (double.Parse(txtPrice.Text) < 0)
            {
                MessageBox.Show(
                    "Price must be a positive number.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            return true;
        }

        /// <summary>
        /// Fills up the values of the form based on the selected item and quantity.
        /// </summary>
        private void FillUpValues()
        {
            // Get the item data
            if (cbItem.SelectedIndex != -1)
            {
                // Get the item ID from the combobox
                int item_id = int.Parse(cbItem.Text.Split(']')[0].Substring(1));
                Item item;
                try
                {
                    item = ih.GetItem(item_id);
                }
                catch (MySqlException exc)
                {
                    MessageBox.Show(
                        $"A database-related error occured: {exc.Message}\n\nFailed to acquire item information.",
                        "Database Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                // Fill up the category combobox
                cbCategory.SelectedIndex = item.Category == "general" ? 0 : 1;
                // Fill up the price textbox if the item and quantity are selected/entered.
                if (int.TryParse(txtQuantity.Text, out int _))
                    txtPrice.Text = (item.UnitPrice * int.Parse(txtQuantity.Text)).ToString();

                // Fill up the status with default value if it is empty.
                if (cbStatus.SelectedIndex == -1)
                    cbStatus.SelectedIndex = 0;

                return;
            }
            txtPrice.Text = string.Empty;
            cbStatus.SelectedIndex = -1;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            selected_transaction = DateTime.MinValue;
            txtTransactionID.Text = string.Empty;
            cbItem.SelectedIndex = -1;
            cbCategory.SelectedIndex = -1;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            cbStatus.SelectedIndex = -1;
            txtNotes.Text = string.Empty;
            UpdateTransactionsList(null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateValues())
                return;

            try
            {
                if (
                    int.Parse(txtQuantity.Text)
                    > ih.GetItem(int.Parse(cbItem.Text.Split(']')[0].Substring(1))).Stock
                )
                {
                    MessageBox.Show(
                        "The quantity exceeds the stock.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                sh.AddTransaction(
                    new Transaction
                    {
                        Id = DateTime.Now,
                        ItemId = int.Parse(cbItem.Text.Split(']')[0].Substring(1)),
                        Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                        Quantity = int.Parse(txtQuantity.Text),
                        Price = double.Parse(txtPrice.Text),
                        Status = cbStatus.SelectedIndex == 0 ? "Unpaid" : "Paid",
                        Notes = txtNotes.Text,
                    }
                );
                var old_item = ih.GetItem(int.Parse(cbItem.Text.Split(']')[0].Substring(1)));
                old_item.Stock -= int.Parse(txtQuantity.Text);
                ih.UpdateItem(old_item);

                MessageBox.Show(
                    "Transaction added successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                btnClear.PerformClick();
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to add the transaction.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            UpdateTransactionsList(null);
        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e) => FillUpValues();

        private void txtQuantity_TextChanged(object sender, EventArgs e) => FillUpValues();

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return; // do nothing if the header is clicked
            var trans_id = DateTime.Parse((string)dgvSales.Rows[e.RowIndex].Cells["id"].Value);
            Transaction transaction;
            try
            {
                transaction = sh.GetTransaction(trans_id);
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(
                    $"A database-related error occured: {exc.Message}\n\nFailed to retrieve the transaction.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            selected_transaction = trans_id;
            txtTransactionID.Text = trans_id.ToString();
            cbItem.SelectedIndex = cbItem.FindString($"[{transaction.ItemId}]");
            cbCategory.SelectedIndex = transaction.Category == "general" ? 0 : 1;
            txtPrice.Text = transaction.Price.ToString();
            txtQuantity.Text = transaction.Quantity.ToString();
            cbStatus.SelectedIndex = transaction.Status == "Unpaid" ? 0 : 1;
            txtNotes.Text = transaction.Notes;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selected_transaction == DateTime.MinValue)
            {
                MessageBox.Show(
                    "Please select a transaction to delete.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (
                MessageBox.Show(
                    "Are you sure you want to delete this transaction?",
                    "Delete Transaction",
                    MessageBoxButtons.YesNo
                ) == DialogResult.No
            )
                return;

            var old_item = ih.GetItem(sh.GetTransaction(selected_transaction).ItemId);
            old_item.Stock += sh.GetTransaction(selected_transaction).Quantity;
            ih.UpdateItem(old_item);

            sh.DeleteTransaction(selected_transaction);
            MessageBox.Show(
                "Transaction deleted successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            btnClear.PerformClick();
            UpdateTransactionsList(null);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            // ask the user where to store the PDF
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Export Transactions List";
            saveFileDialog.FileName =
                $"transactions-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            var transactions = sh.GetAllTransactions();

            // create the PDF
            using (var doc = new Document(new PdfDocument(new PdfWriter(saveFileDialog.FileName))))
            {
                // Create a table with 7 columns
                var table = new Table(new float[] { 1, 2, 1, 1, 1, 1, 3 }).UseAllAvailableWidth();
                // Add a header
                table.AddHeaderCell("Transaction Date");
                table.AddHeaderCell("Item");
                table.AddHeaderCell("Category");
                table.AddHeaderCell("Price");
                table.AddHeaderCell("Quantity");
                table.AddHeaderCell("Status");
                table.AddHeaderCell("Notes");

                // Add the transactions to the table
                foreach (var transaction in transactions)
                {
                    table.AddCell(transaction.Id.ToString());
                    table.AddCell(ih.GetItem(transaction.ItemId).Name);
                    table.AddCell(transaction.Category);
                    table.AddCell(transaction.Price.ToString());
                    table.AddCell(transaction.Quantity.ToString());
                    table.AddCell(transaction.Status);
                    table.AddCell(transaction.Notes);
                }

                // Add the table to the document
                doc.Add(table);

                // Show a message box to inform the user that the PDF has been created
                MessageBox.Show(
                    "Transactions list exported successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Open the PDF file
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (selected_transaction == DateTime.MinValue)
            {
                MessageBox.Show(
                    "Please select a transaction to update.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (!ValidateValues())
                return;

            var old_stock_value = sh.GetTransaction(selected_transaction).Quantity;
            if (
                int.Parse(txtQuantity.Text) - old_stock_value
                > ih.GetItem(int.Parse(cbItem.Text.Split(']')[0].Substring(1))).Stock
            )
            {
                MessageBox.Show(
                    "The quantity exceeds the stock.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            sh.UpdateTransaction(
                new Transaction
                {
                    Id = selected_transaction,
                    ItemId = int.Parse(cbItem.Text.Split(']')[0].Substring(1)),
                    Category = cbCategory.SelectedIndex == 0 ? "general" : "electronic",
                    Quantity = int.Parse(txtQuantity.Text),
                    Price = double.Parse(txtPrice.Text),
                    Status = cbStatus.SelectedIndex == 0 ? "Unpaid" : "Paid",
                    Notes = txtNotes.Text,
                }
            );
            var old_item = ih.GetItem(int.Parse(cbItem.Text.Split(']')[0].Substring(1)));
            old_item.Stock -= int.Parse(txtQuantity.Text) - old_stock_value;
            ih.UpdateItem(old_item);

            MessageBox.Show(
                "Transaction updated successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            btnClear.PerformClick();
            UpdateTransactionsList(null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchbar = new Searchbar();
            searchbar.searchbar_title = "Search Transactions";
            var query = searchbar.ShowDialog();
            if (query == DialogResult.OK)
                UpdateTransactionsList(searchbar.query);
        }
    }
}
