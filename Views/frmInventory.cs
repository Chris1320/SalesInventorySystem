using SalesInventorySystem_WAM1.Handlers;
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
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
            UpdateItemsList();
        }
        public void UpdateItemsList()
        {

            var db = new ItemHandler();
            var items = db.GetAllItems();
            dgvInventory.Rows.Clear();
            foreach (var item in items)
            {
                dgvInventory.Rows.Add(
                    item.Id,
                    item.Name,
                    item.Category,
                    item.UnitPrice,
                    item.Stock,
                    item.DateAdded == null ? "N/A" : item.DateAdded.Date.ToString("yyyy-MM-dd HH:mm:ss")
                );
            }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }
    }
}
