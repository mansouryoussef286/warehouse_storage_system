﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace warehouse_storage_system
{
    public partial class MainApp : Form
    {
        //application variables
        ConfirmDialog confirmDlg;   //delete confirmation box
        warehouseDBEntities entities;

        public MainApp()
        {
            InitializeComponent();
            confirmDlg = new ConfirmDialog("this store?");
            entities = new warehouseDBEntities();
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            fillStoresGridView();
            fillSuppliersGridView();
        }

        #region stores tab
        //fill the grid view of the stores
        public void fillStoresGridView()
        {
            storesGridView.Rows.Clear();
            foreach(var store in entities.stores)
            {
                storesGridView.Rows.Add(store.store_name, store.store_address, store.store_keeper);
            }
        }

        private void storesGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //fill the products grid
            //if only 1 store is selected 
            if (storesGridView.SelectedRows.Count == 1)
            {
                string storeName = storesGridView.SelectedRows[0].Cells[0].Value.ToString();
                //fill the store grid view
                productsGridView_s.Rows.Clear();
                var products = entities.product_stores.Where(p => p.store_name == storeName).Select(p =>new { p.product_ID  ,p.product.product_name }).Distinct();
                foreach (var product in products)
                {
                    productsGridView_s.Rows.Add(product.product_ID, product.product_name, "");
                }
            }
            else     //if more than 1 store
            {
                List<string> storeNames = new List<string>();
                //get the selected store names
                for (int i = 0; i < storesGridView.SelectedRows.Count; i++)
                {
                    storeNames.Add(storesGridView.SelectedRows[i].Cells[0].Value.ToString());
                }
                productsGridView_s.Rows.Clear();
                var products = entities.product_stores.Where(p => storeNames.Contains(p.store_name)).Select(p => new { p.product_ID, p.product.product_name, p.store_name }).Distinct();
                foreach (var product in products)
                {
                    productsGridView_s.Rows.Add(product.product_ID, product.product_name, product.store_name);
                }
            }
            //fill the textboxes
            storeNameTextBox.Text = storesGridView.SelectedRows[0].Cells[0].Value.ToString();
            storeAddressTextBox.Text = storesGridView.SelectedRows[0].Cells[1].Value.ToString();
            storeKeeperTextBox.Text = storesGridView.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void deleteStoreBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = confirmDlg.ShowDialog();
            if(res == DialogResult.OK)
            {
                string storeName = storesGridView.SelectedRows[0].Cells[0].Value.ToString();
                var store = (entities.stores.Where(st => st.store_name == storeName).Select(st => st)).First();
                entities.stores.Remove(store);
                entities.SaveChanges();
                fillStoresGridView();
                MessageBox.Show("deleted");

            }
        }


        private void updateStoreBtn_Click(object sender, EventArgs e)
        {
            if (storesGridView.SelectedRows.Count == 1)
            {
                string storeName = storesGridView.SelectedRows[0].Cells[0].Value.ToString();
                var store = (entities.stores.Where(st => st.store_name == storeName).Select(st => st)).First();
                //store.store_name = storeNameTextBox.Text;
                store.store_address = storeAddressTextBox.Text;
                store.store_keeper = storeKeeperTextBox.Text;
                entities.SaveChanges();
                fillStoresGridView();
            }
            else
            {
                MessageBox.Show("please select one store");
            }
        }

        private void insertStoreBtn_Click(object sender, EventArgs e)
        {
            store newStore = new store();
            newStore.store_name = storeNameTextBox.Text;
            newStore.store_address = storeAddressTextBox.Text;
            newStore.store_keeper = storeKeeperTextBox.Text;
            entities.stores.Add(newStore);
            entities.SaveChanges();
            fillStoresGridView();
        }
        #endregion
        //*******************************************************************//
        #region suppliers tab
        public void fillSuppliersGridView()
        {
            suppliersGridView.Rows.Clear();
            foreach (var supplier in entities.suppliers)
            {
                suppliersGridView.Rows.Add(supplier.supplier_ID, supplier.supplier_name, supplier.phone);
            }
        }

        private void suppliersGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int supplierID = int.Parse(suppliersGridView.SelectedRows[0].Cells[0].Value.ToString());
            var supplier = (entities.suppliers.Where(s => s.supplier_ID == supplierID).Select(s => s)).First();
            supNameTextBox.Text = supplier.supplier_name ;
            supMobTextBox.Text = supplier.mobile ;
            supFaxTextBox.Text = supplier.fax ;
            supEmailTextBox.Text = supplier.email;
            supIDTextBox.Text = supplier.supplier_ID.ToString() ;
            supPhoneTextBox.Text = supplier.phone;
            supWebsiteTextBox.Text = supplier.website;
        }

        private void supInsertBtn_Click(object sender, EventArgs e)
        {
            try
            {
                supplier sup = new supplier();
                sup.supplier_name = supNameTextBox.Text;
                sup.mobile = supMobTextBox.Text;
                sup.fax = supFaxTextBox.Text;
                sup.email = supEmailTextBox.Text;
                sup.supplier_ID = int.Parse(supIDTextBox.Text);
                sup.phone = supPhoneTextBox.Text;
                sup.website = supWebsiteTextBox.Text;
                entities.suppliers.Add(sup);
                entities.SaveChanges();
                fillSuppliersGridView();
            }
            catch
            {
                MessageBox.Show("check to enter the info correctly");
            }
        }

        private void supUpdateBtn_Click(object sender, EventArgs e)
        {
            int supplierID = int.Parse(suppliersGridView.SelectedRows[0].Cells[0].Value.ToString());
            var sup = (entities.suppliers.Where(s => s.supplier_ID == supplierID).Select(s => s)).First();
            sup.supplier_name = supNameTextBox.Text;
            sup.mobile = supMobTextBox.Text;
            sup.fax = supFaxTextBox.Text;
            sup.email = supEmailTextBox.Text;
            sup.supplier_ID = int.Parse(supIDTextBox.Text);
            sup.phone = supPhoneTextBox.Text;
            sup.website = supWebsiteTextBox.Text;
            entities.SaveChanges();
            fillSuppliersGridView();
        }

        private void supDeleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = confirmDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                int supplierID = int.Parse(suppliersGridView.SelectedRows[0].Cells[0].Value.ToString());
                var sup = (entities.suppliers.Where(s => s.supplier_ID == supplierID).Select(s => s)).First();
                entities.suppliers.Remove(sup);
                entities.SaveChanges();
                fillSuppliersGridView();
                MessageBox.Show("deleted");
            }
            
        }
        #endregion
        //*******************************************************************//
        #region client tab

        #region

    }
}
