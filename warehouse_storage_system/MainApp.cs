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
        warehouseDBEntities1 entities;

        public MainApp()
        {
            InitializeComponent();
            entities = new warehouseDBEntities1();
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            fillStoresGridView();
            fillSuppliersGridView();
            fillClientsGridView();
            fillProductsGridView();
            fillFromToStoresGridView();
            fillInRequestsGridView();
            fillOutRequestsGridView();
            //reportViewer1.RefreshReport();
            //reportViewer1.ServerReport.Refresh();
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
            confirmDlg = new ConfirmDialog("Are you sure to delete\nthis store?");
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
            fillInRequestsGridView();
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
                fillInRequestsGridView();
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
            fillInRequestsGridView();
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
            confirmDlg = new ConfirmDialog("Are you sure to delete\nthis supplier?");
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
            confirmDlg = new ConfirmDialog("Are you sure to delete\nthis client?");
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

        #endregion

        //*******************************************************************//
        #region incoming requests tab
        public void fillInRequestsGridView()
        {
            inRequestsGridView.Rows.Clear();
            foreach (var InReq in entities.supplier_requests)
            {
                inRequestsGridView.Rows.Add(InReq.inRequest_ID, InReq.supplier.supplier_name, InReq.date);
            }
            inReqStoreName.Items.Clear();
            foreach (var store in entities.stores)
            {
                inReqStoreName.Items.Add(store.store_name);
            }
        }

        private void inRequestsGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID= int.Parse(inRequestsGridView.SelectedRows[0].Cells[0].Value.ToString());
            var request = entities.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID)
                .Select(r=>r).FirstOrDefault();
            var inreqProducts = entities.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID)
                .Select(r => r);
            //fill the products in that request
            inReqProdGridView.Rows.Clear();
            foreach (var prod in inreqProducts)
            {
                inReqProdGridView.Rows.Add(prod.product_ID, prod.product.product_name,prod.production_date , prod.input_quantity);
            }
            inReqID.Text = request.supplierRequest_ID.ToString();
            inReqSupName.Text =request.supplier_requests.supplier.supplier_name;
            inReqStoreName.Text = request.store_name;
            inReqDate.Text =request.supplier_requests.date.ToString().Split(' ')[0];
        }

        private void inReqProdGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(inRequestsGridView.SelectedRows[0].Cells[0].Value.ToString());
            int prodID = int.Parse(inReqProdGridView.SelectedRows[0].Cells[0].Value.ToString());
            DateTime proDate = DateTime.Parse(inReqProdGridView.SelectedRows[0].Cells[2].Value.ToString());
            var inProduct = entities.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID && r.product_ID==prodID && r.production_date==proDate).Select(r => r).FirstOrDefault();
            
            inReqProdID.Text = prodID.ToString();
            inReqProdDate.Text = inProduct.production_date.ToString().Split(' ')[0];
            inReqExpPeriod.Text = inProduct.product.expire_period.ToString();
            inReqQuantity.Text = inProduct.input_quantity.ToString();
        }

        private void inReqInsertBtn_Click(object sender, EventArgs e)
        {
            //get the request and product details
            int reqID = int.Parse(inReqID.Text);
            string storeName = inReqStoreName.SelectedItem.ToString();
            DateTime date = DateTime.Parse(inReqDate.Text);

            int prodID = int.Parse(inReqProdID.Text);
            DateTime prodDate = DateTime.Parse(inReqProdDate.Text);
            int quantity = int.Parse(inReqQuantity.Text);
            
            //check if the supplier exist in the warehouse system
            var supID = entities.suppliers.Where(s => s.supplier_name == inReqSupName.Text)
                .Select(s => s.supplier_ID).FirstOrDefault();
            if (supID != 0)
            {
                //check if the product exist in the warehouse system
                var product = entities.products.Where(p => p.product_ID == prodID)
                    .Select(p => p).FirstOrDefault();
                if(product != null)
                {
                    //check if the supplier request id is new
                    var supplyrequest = entities.supplier_requests.Where(sr => sr.inRequest_ID == reqID)
                        .Select(sr => sr).FirstOrDefault();
                    if(supplyrequest == null)
                    {
                        //insert in supplier requests table
                        supplier_requests inRequest = new supplier_requests();
                        inRequest.inRequest_ID = reqID;
                        inRequest.supplier_ID = supID;
                        inRequest.date = date;
                        entities.supplier_requests.Add(inRequest);
                        entities.SaveChanges();
                    }
                    //else then the request exists
                    //so just add the products in the request details table
                  
                    //insert in supplier-requests details table
                    supplierRequest_details inRequestDetails = new supplierRequest_details();
                    inRequestDetails.supplierRequest_ID = reqID;
                    inRequestDetails.product_ID = prodID;
                    inRequestDetails.store_name = storeName;
                    inRequestDetails.input_quantity = quantity;
                    inRequestDetails.production_date = prodDate;
                    entities.supplierRequest_details.Add(inRequestDetails);

                    //update the product stores table with the incoming quantity
                    var productStore = entities.product_stores.Where(ps => ps.product_ID == prodID && ps.store_name == storeName && ps.production_date == prodDate)
                        .Select(ps => ps).FirstOrDefault();
                    //check if the product exists in that store
                    if (productStore == null)
                    {
                        product_stores ps = new product_stores();
                        ps.product_ID = prodID;
                        ps.store_name = storeName;
                        ps.production_date = prodDate;
                        ps.quantity = 0;
                        entities.product_stores.Add(ps);
                        productStore = ps;
                    }
                    productStore.quantity += quantity;
                    entities.SaveChanges();
                    MainApp_Load(null, null);
                }
                else
                {
                    MessageBox.Show("this product doesn't exist");
                    //adjust the if to == null
                    //then add the product in the products table
                    //and continue with the same code
                }
            }
            else
            {
                MessageBox.Show("this supplier doesn't exist");
            }
        }

        private void inReqUpdateBtn_Click(object sender, EventArgs e)
        {
            confirmDlg = new ConfirmDialog("Are you sure to Update\nthis request?");
            DialogResult res = confirmDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (inReqID.Text != null && inReqStoreName.SelectedItem.ToString() != null && inReqProdID.Text != null && inReqProdDate.Text != null && inReqQuantity.Text != null)
                {
                    int reqID = int.Parse(inReqID.Text);
                    string storeName = inReqStoreName.SelectedItem.ToString();
                    DateTime date = DateTime.Parse(inReqDate.Text);

                    int prodID = int.Parse(inReqProdID.Text);
                    DateTime prodDate = DateTime.Parse(inReqProdDate.Text);
                    int quantity = int.Parse(inReqQuantity.Text);

                    //get the old quantity
                    var oldQuantity = entities.supplierRequest_details
                        .Where(sr => sr.supplierRequest_ID == reqID && sr.product_ID == prodID && sr.store_name == storeName && sr.production_date == prodDate)
                            .Select(ps => ps.input_quantity).FirstOrDefault();
                    //get the supplier id from his name
                    var supID = entities.suppliers.Where(s => s.supplier_name == inReqSupName.Text)
                    .Select(s => s.supplier_ID).FirstOrDefault();
                    //get the supply request
                    var supplyrequest = entities.supplier_requests.Where(sr => sr.inRequest_ID == reqID)
                            .Select(sr => sr).FirstOrDefault();
                    //get the supply request details to edit the quantity
                    var supplyrequestDetails = entities.supplierRequest_details
                        .Where(sr => sr.supplierRequest_ID == reqID && sr.product_ID == prodID && sr.store_name == storeName && sr.production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();

                    if (supID != 0)
                    {
                        //update the supply request table
                        supplyrequest.supplier_ID = supID;
                        supplyrequest.date = date;
                        //update the supply request details table
                        supplyrequestDetails.input_quantity = quantity;

                        //update the product stores table with the updated quantity
                        var productStore = entities.product_stores.Where(ps => ps.product_ID == prodID && ps.store_name == storeName && ps.production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();
                        //check if the product exists in that store
                        if (productStore != null)
                        {
                            productStore.quantity = productStore.quantity + quantity - oldQuantity;
                            entities.SaveChanges();
                        }
                        MainApp_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("this supplier doesn't exist");
                    }
                }
            }
        }
        
        #endregion

        //*******************************************************************//
        #region incoming requests tab

        public void fillOutRequestsGridView()
        {

            outRequestsGridView.Rows.Clear();
            foreach (var outReq in entities.client_requests)
            {
                outRequestsGridView.Rows.Add(outReq.outRequest_ID, outReq.client.client_name, outReq.date);
            }
            outReqStoreName.Items.Clear();
            foreach (var store in entities.stores)
            {
                outReqStoreName.Items.Add(store.store_name);
            }
        }

        private void outRequestsGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(outRequestsGridView.SelectedRows[0].Cells[0].Value.ToString());
            var request = entities.clientRequest_details.Where(r => r.clientRequets_ID == reqID)
                .Select(r => r).FirstOrDefault();
            var outReqProducts = entities.clientRequest_details.Where(r => r.clientRequets_ID == reqID)
                .Select(r => r);
            //fill the products in that request
            outReqProdGridView.Rows.Clear();
            foreach (var prod in outReqProducts)
            {
                outReqProdGridView.Rows.Add(prod.product_ID, prod.product.product_name, prod.production_date, prod.output_quantity);
            }
            outReqID.Text = request.clientRequets_ID.ToString();
            outReqClientName.Text = request.client_requests.client.client_name;
            outReqStoreName.Text = request.store_name;
            outReqDate.Text = request.client_requests.date.ToString().Split(' ')[0];
        }

        private void outReqProdGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(outRequestsGridView.SelectedRows[0].Cells[0].Value.ToString());
            int prodID = int.Parse(outReqProdGridView.SelectedRows[0].Cells[0].Value.ToString());
            DateTime proDate = DateTime.Parse(outReqProdGridView.SelectedRows[0].Cells[2].Value.ToString());
            var outProduct = entities.clientRequest_details.Where(r => r.clientRequets_ID == reqID && r.product_ID == prodID && r.production_date == proDate).Select(r => r).FirstOrDefault();

            outReqProdID.Text = prodID.ToString();
            outReqProdDate.Text = outProduct.production_date.ToString().Split(' ')[0];
            outReqExpPeriod.Text = outProduct.product.expire_period.ToString();
            outReqQuantity.Text = outProduct.output_quantity.ToString();
        }

        private void outReqInsertBtn_Click(object sender, EventArgs e)
        {
            //get the request and product details
            int reqID = int.Parse(outReqID.Text);
            string storeName = outReqStoreName.SelectedItem.ToString();
            DateTime date = DateTime.Parse(outReqDate.Text);

            int prodID = int.Parse(outReqProdID.Text);
            DateTime prodDate = DateTime.Parse(outReqProdDate.Text);
            int quantity = int.Parse(outReqQuantity.Text);

            //check if the client exist in the warehouse system
            var clientID = entities.clients.Where(c => c.client_name == outReqClientName.Text)
                .Select(c => c.client_ID).FirstOrDefault();

            if (clientID != 0)
            {
                //check if the product exist in the warehouse system
                var product = entities.products.Where(p => p.product_ID == prodID)
                    .Select(p => p).FirstOrDefault();
                if (product != null)
                {
                    //check if the product exists in that store
                    var productStore = entities.product_stores.Where(ps => ps.product_ID == prodID && ps.store_name == storeName && ps.production_date == prodDate)
                       .Select(ps => ps).FirstOrDefault();
                    if (productStore != null)
                    {
                        //check if the available quantity is more than the required quantity
                        if (productStore.quantity >= quantity)
                        {
                            //check if the client request id is new
                            var purchaseRequest = entities.client_requests.Where(cr => cr.outRequest_ID == reqID)
                                .Select(sr => sr).FirstOrDefault();
                            if (purchaseRequest == null)
                            {
                                //insert in client requests table
                                client_requests outRequest = new client_requests();
                                outRequest.outRequest_ID = reqID;
                                outRequest.client_ID = clientID;
                                outRequest.date = date;
                                entities.client_requests.Add(outRequest);
                                entities.SaveChanges();
                            }
                            //else then the request exists
                            //so just add the products in the request details table

                            //insert in clients-requests details table
                            clientRequest_details outRequestDetails = new clientRequest_details();
                            outRequestDetails.clientRequets_ID = reqID;
                            outRequestDetails.product_ID = prodID;
                            outRequestDetails.store_name = storeName;
                            outRequestDetails.output_quantity = quantity;
                            outRequestDetails.production_date = prodDate;
                            entities.clientRequest_details.Add(outRequestDetails);

                            //update the product stores table with the incoming quantity
                            productStore.quantity -= quantity;
                            entities.SaveChanges();
                            MainApp_Load(null, null);
                        }
                        else
                        {
                            MessageBox.Show($"the rquired quantity isn't available!\n only {productStore.quantity} left in store");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"the rquired product isn't available!\n in {productStore.store_name} store");
                    }
                }
                else
                {
                    MessageBox.Show("this product doesn't exist");
                    //adjust the if to == null
                    //then add the product in the products table
                    //and continue with the same code
                }
            }
            else
            {
                MessageBox.Show("this supplier doesn't exist");
            }
        }

        private void outReqUpdateBtn_Click(object sender, EventArgs e)
        {
            confirmDlg = new ConfirmDialog("Are you sure to Update\nthis request?");
            DialogResult res = confirmDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (outReqID.Text != null && outReqStoreName.SelectedItem.ToString() != null && outReqProdID.Text != null && outReqProdDate.Text != null && outReqQuantity.Text != null)
                {
                    int reqID = int.Parse(outReqID.Text);
                    string storeName = outReqStoreName.SelectedItem.ToString();
                    DateTime date = DateTime.Parse(outReqDate.Text);

                    int prodID = int.Parse(outReqProdID.Text);
                    DateTime prodDate = DateTime.Parse(outReqProdDate.Text);
                    int quantity = int.Parse(outReqQuantity.Text);

                    //get the old quantity
                    var oldQuantity = entities.clientRequest_details
                        .Where(cr => cr.clientRequets_ID == reqID && cr.product_ID == prodID && cr.store_name == storeName && cr.production_date == prodDate)
                            .Select(ps => ps.output_quantity).FirstOrDefault();
                    //get the client id from his name
                    var clientID = entities.clients.Where(c => c.client_name == outReqClientName.Text)
                    .Select(c => c.client_ID).FirstOrDefault();
                    //get the client request
                    var clientRequest = entities.client_requests.Where(cr => cr.outRequest_ID == reqID)
                            .Select(cr => cr).FirstOrDefault();
                    //get the supply request details to edit the quantity
                    var clientRequestDetails = entities.clientRequest_details
                        .Where(cr => cr.clientRequets_ID == reqID && cr.product_ID == prodID && cr.store_name == storeName && cr.production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();

                    if (clientID != 0)
                    {
                        //update the client request table
                        clientRequest.client_ID = clientID;
                        clientRequest.date = date;
                        //update the client request details table
                        clientRequestDetails.output_quantity = quantity;

                        //update the product stores table with the updated quantity
                        var productStore = entities.product_stores.Where(ps => ps.product_ID == prodID && ps.store_name == storeName && ps.production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();
                        //check if the product exists in that store
                        if (productStore != null)
                        {
                            productStore.quantity = productStore.quantity - quantity + oldQuantity;
                            entities.SaveChanges();
                        }
                        MainApp_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("this client doesn't exist");
                    }
                }
            }
        }

        #endregion

    }

}
