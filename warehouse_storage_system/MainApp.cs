using System;
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
            fillClientsGridView();
            fillProductsGridView();
            fillFromToStoresGridView();
        }

        #region stores tab
        //fill the grid view of the stores
        public void fillStoresGridView()
        {
            storesGridView.Rows.Clear();
            foreach (var store in entities.stores)
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
                var products = entities.product_stores.Where(p => p.store_name == storeName)
                    .Select(p => new { p.product_ID, p.product.product_name }).Distinct();
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
                var products = entities.product_stores.Where(p => storeNames.Contains(p.store_name))
                    .Select(p => new { p.product_ID, p.product.product_name, p.store_name }).Distinct();
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
            if (res == DialogResult.OK)
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
            try
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
                MessageBox.Show("updated successfully!");
            }
            catch
            {
                MessageBox.Show("update Failed!");
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
            supNameTextBox.Text = supplier.supplier_name;
            supMobTextBox.Text = supplier.mobile;
            supFaxTextBox.Text = supplier.fax;
            supEmailTextBox.Text = supplier.email;
            supIDTextBox.Text = supplier.supplier_ID.ToString();
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
                //sup.supplier_ID = int.Parse(supIDTextBox.Text);
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
            try
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
                MessageBox.Show("updated successfully!");
            }
            catch
            {
                MessageBox.Show("update Failed!");
            }
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
        public void fillClientsGridView()
        {
            clientsGridView.Rows.Clear();
            foreach (var client in entities.clients)
            {
                clientsGridView.Rows.Add(client.client_ID, client.client_name, client.phone);
            }
        }

        private void clientsGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int clientID = int.Parse(clientsGridView.SelectedRows[0].Cells[0].Value.ToString());
            var client = (entities.clients.Where(c => c.client_ID == clientID).Select(c => c)).First();
            clientNameTextBox.Text = client.client_name;
            clientMobTextBox.Text = client.mobile;
            clientFaxTextBox.Text = client.fax;
            clientEmailTextBox.Text = client.email;
            clientIDTextBox.Text = client.client_ID.ToString();
            clientPhoneTextBox.Text = client.phone;
            clientWebsiteTextBox.Text = client.website;
        }

        private void clientInsertBtn_Click(object sender, EventArgs e)
        {
            try
            {
                client client = new client();
                //client.client_ID = int.Parse(clientIDTextBox.Text);
                client.client_name = clientNameTextBox.Text;
                client.mobile = clientMobTextBox.Text;
                client.fax = clientFaxTextBox.Text;
                client.email = clientEmailTextBox.Text;
                client.phone = clientPhoneTextBox.Text;
                client.website = clientWebsiteTextBox.Text;
                entities.clients.Add(client);
                entities.SaveChanges();
                fillClientsGridView();
            }
            catch
            {
                MessageBox.Show("check to enter the info correctly");
            }
        }

        private void clientUpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int clientID = int.Parse(clientsGridView.SelectedRows[0].Cells[0].Value.ToString());
                var client = (entities.clients.Where(c => c.client_ID == clientID).Select(c => c)).First();
                client.client_name = clientNameTextBox.Text;
                client.mobile = clientMobTextBox.Text;
                client.fax = clientFaxTextBox.Text;
                client.email = clientEmailTextBox.Text;
                client.client_ID = int.Parse(clientIDTextBox.Text);
                client.phone = clientPhoneTextBox.Text;
                client.website = clientWebsiteTextBox.Text;
                entities.SaveChanges();
                fillClientsGridView();
                MessageBox.Show("updated successfully!");
            }
            catch
            {
                MessageBox.Show("update Failed!");
            }
        }

        private void clientDeleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = confirmDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                int clientID = int.Parse(clientsGridView.SelectedRows[0].Cells[0].Value.ToString());
                var client = (entities.clients.Where(s => s.client_ID == clientID).Select(s => s)).First();
                entities.clients.Remove(client);
                entities.SaveChanges();
                fillClientsGridView();
                MessageBox.Show("deleted");
            }
        }
        #endregion

        //*******************************************************************//
        #region products tab update product
        public void fillProductsGridView()
        {
            productsGridView_p.Rows.Clear();
            foreach (var product in entities.products)
            {
                productsGridView_p.Rows.Add(product.product_ID, product.product_name, product.expire_period);
            }
        }

        private void productsGridView_p_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (productsGridView_p.SelectedRows.Count == 1)
            {
                int productID = int.Parse(productsGridView_p.SelectedRows[0].Cells[0].Value.ToString());
                var product = (entities.products.Where(p => p.product_ID == productID).Select(p => p)).First();
                prodIDTextBox.Text = product.product_ID.ToString();
                prodNameTextBox.Text = product.product_name;
                prodExpPerTextBox.Text = product.expire_period.ToString();

                //get the stores where this product is stored in
                var stores = entities.product_stores.Where(s => s.product_ID == productID)
                    .Select(s => s.store_name).Distinct();
                //clear the stores, production dates boxes and the quantity
                prodStoreComboBox.Items.Clear();
                prodStoreComboBox.Text = "";
                prodProductionDateComboBox.Items.Clear();
                prodProductionDateComboBox.Text = "";
                prodQuantityTextBox.Text = "NULL";
                //refill the stores
                foreach (var store in stores)
                {
                    prodStoreComboBox.Items.Add(store);
                }
                //select the first store
                if (prodStoreComboBox.Items.Count > 0)
                {
                    prodStoreComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("please select one product");
            }

        }

        private void prodStoreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the production dates available in the specified store of that product
            string storeName = prodStoreComboBox.SelectedItem.ToString();
            int productID = int.Parse(productsGridView_p.SelectedRows[0].Cells[0].Value.ToString());
            var prodDates = entities.product_stores.Where(p => p.store_name == storeName && p.product_ID == productID)
                .Select(p => p.production_date);

            prodProductionDateComboBox.Items.Clear();
            prodQuantityTextBox.Text = "NULL";

            foreach (var prodDate in prodDates)
            {
                prodProductionDateComboBox.Items.Add(prodDate.ToString().Split(' ')[0]);
            }
            //select the first production date
            if (prodProductionDateComboBox.Items.Count > 0)
            {
                prodProductionDateComboBox.SelectedIndex = 0;
            }
        }

        private void prodProductionDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string storeName = prodStoreComboBox.SelectedItem.ToString();
                int productID = int.Parse(productsGridView_p.SelectedRows[0].Cells[0].Value.ToString());
                DateTime prodDate = DateTime.Parse(prodProductionDateComboBox.SelectedItem.ToString());
                int quantity = entities.product_stores.Where(p => p.store_name == storeName && p.product_ID == productID && p.production_date == prodDate)
                    .Select(p => p.quantity).First();
                prodQuantityTextBox.Text = quantity.ToString();
            }
            catch
            {
                prodQuantityTextBox.Text = "NULL";
                MessageBox.Show("no products");
            }

        }

        private void deleteProductBtn_Click(object sender, EventArgs e)
        {

        }

        private void insertProductBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int productID = int.Parse(prodIDTextBox.Text);
                string prodName = prodNameTextBox.Text;
                int expPeriod = int.Parse(prodExpPerTextBox.Text);
                var newProd = new product();
                newProd.product_ID = productID;
                newProd.product_name = prodName;
                newProd.expire_period = expPeriod;
                entities.products.Add(newProd);
                entities.SaveChanges();
                fillProductsGridView();
            }
            catch
            {
                MessageBox.Show("error happened product isnot inserted!");
            }
        }

        private void updateProductBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int productID = int.Parse(prodIDTextBox.Text);
                string prodName = prodNameTextBox.Text;
                int expPeriod = int.Parse(prodExpPerTextBox.Text);
                var product = entities.products.Find(productID);
                //product.product_ID = int.Parse(prodIDTextBox.Text);//cannot update a pk in EF
                product.product_name = prodName;
                product.expire_period = expPeriod;
                entities.SaveChanges();
                fillProductsGridView();
            }
            catch
            {
                MessageBox.Show("error happened product isnot updated!");
            }
        }
        #endregion

        //*******************************************************************//
        #region products tab move product
        public void fillFromToStoresGridView()
        {
            fromStoreGridView.Rows.Clear();
            foreach (var store in entities.stores)
            {
                fromStoreGridView.Rows.Add(store.store_name);
            }
            toStoreGridView.Rows.Clear();
            foreach (var store in entities.stores)
            {
                toStoreGridView.Rows.Add(store.store_name);
            }
        }

        #endregion

        private void fromStoreGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string storeName = fromStoreGridView.SelectedRows[0].Cells[0].Value.ToString();
            var products = entities.product_stores.Where(p => p.store_name == storeName)
                .Select(p => new { p.product_ID, p.product.product_name }).Distinct();
            //fill products ids and names
            prodIDComboBox.Items.Clear();
            prodIDComboBox.Text = "";
            foreach (var product in products)
            {
                prodIDComboBox.Items.Add(product.product_ID);
            }
            prodNameComboBox.Items.Clear();
            prodNameComboBox.Text = "";
            foreach (var product in products)
            {
                prodNameComboBox.Items.Add(product.product_name);
            }
            //clear the form
            prodExpPeriodTextBox.Text = "";
            prodProdDateComboBox.Text = "";
            reqQuantityTextBox.Text = "";
            availableQuantityTextBox.Text = "";
            //select first product if existed 
            if (prodIDComboBox.Items.Count > 0)
            {
                prodIDComboBox.SelectedIndex = 0;
                prodNameComboBox.SelectedIndex = 0;
            }
        }


        private void prodIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            prodNameComboBox.SelectedIndex = prodIDComboBox.SelectedIndex;
            int prodID = int.Parse(prodIDComboBox.SelectedItem.ToString());
            string storeName = fromStoreGridView.SelectedRows[0].Cells[0].Value.ToString();
            var prodStores = entities.product_stores.Where(p => p.product_ID == prodID && p.store_name == storeName)
                .Select(p => p);

            prodProdDateComboBox.Items.Clear();
            foreach (var product in prodStores)
            {
                prodProdDateComboBox.Items.Add(product.production_date.ToString().Split(' ')[0]);
            }
            prodExpPeriodTextBox.Text = entities.products.Where(p => p.product_ID == prodID)
                .Select(p => p.expire_period).First().ToString();
        }

        private void prodNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            prodIDComboBox.SelectedIndex = prodNameComboBox.SelectedIndex;
        }

        private void prodProdDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int prodID = int.Parse(prodIDComboBox.SelectedItem.ToString());
            string storeName = fromStoreGridView.SelectedRows[0].Cells[0].Value.ToString();
            DateTime prodDate = DateTime.Parse(prodProdDateComboBox.SelectedItem.ToString());
            int quantity = entities.product_stores.Where(p => p.product_ID == prodID && p.store_name == storeName && p.production_date == prodDate)
                .Select(p => p.quantity).First();
            availableQuantityTextBox.Text = quantity.ToString();
        }

        private void mvoeProductBtn_Click(object sender, EventArgs e)
        {
            //check on the required quantity
            int check;
            int.TryParse(reqQuantityTextBox.Text, out check);
            if (reqQuantityTextBox.Text != "" && check != 0)
            {
                int reqQuantity = int.Parse(reqQuantityTextBox.Text);
                int avaQuantity = int.Parse(availableQuantityTextBox.Text);
                string fromstoreName = fromStoreGridView.SelectedRows[0].Cells[0].Value.ToString();
                string toStoreName = toStoreGridView.SelectedRows[0].Cells[0].Value.ToString();

                int prodID = int.Parse(prodIDComboBox.SelectedItem.ToString());
                DateTime prodDate = DateTime.Parse(prodProdDateComboBox.SelectedItem.ToString());


                if (reqQuantity <= avaQuantity)
                {
                    //subtract the moved quantity from store 1
                    var prodStoresFrom = entities.product_stores.Where(p => p.product_ID == prodID && p.store_name == fromstoreName && p.production_date == prodDate)
                        .Select(p => p).First();
                    prodStoresFrom.quantity -= reqQuantity;
                    //add the moved quantity to store 2 if the product exists or add the product with the moved quantity
                    var prodStoresTo = entities.product_stores.Where(p => p.product_ID == prodID && p.store_name == toStoreName && p.production_date == prodDate)
                        .Select(p => p).FirstOrDefault();
                    //move the product
                    if(prodStoresTo != null) //the product exists in the ingoing store
                    {
                        prodStoresTo.quantity += reqQuantity;
                    }
                    else        //the product doesnot exist in the outgoing store
                    {
                        prodStoresTo = new product_stores();
                        prodStoresTo.product_ID = prodID;
                        prodStoresTo.store_name = toStoreName;
                        prodStoresTo.production_date = prodDate;
                        prodStoresTo.quantity = reqQuantity;
                        entities.product_stores.Add(prodStoresTo);
                    }
                    //add this move in the log table products movement
                    var prodMove = new products_movement();
                    prodMove.store_from = fromstoreName;
                    prodMove.store_to = toStoreName;
                    prodMove.product_ID = prodID;
                    prodMove.production_date = prodDate;
                    prodMove.move_date = DateTime.Today;
                    prodMove.quantity = reqQuantity;
                    entities.products_movement.Add(prodMove);
                    entities.SaveChanges();
                    //update form
                    fillFromToStoresGridView();
                }
                else
                {
                    MessageBox.Show("required quantity isn't available");
                }
            }
            else
            {
                MessageBox.Show("enter a numbered quantity");
            }
        }
    }

}
