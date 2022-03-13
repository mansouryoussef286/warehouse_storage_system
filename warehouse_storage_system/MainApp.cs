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
        ConfirmDialog confirmDlg;
        public MainApp()
        {
            InitializeComponent();
            confirmDlg = new ConfirmDialog("this store?");
        }

        private void deleteStoreBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = confirmDlg.ShowDialog();
            if(res == DialogResult.OK)
            {
                MessageBox.Show("deleted");
            }
            else
            {

            }
        }

    }
}
