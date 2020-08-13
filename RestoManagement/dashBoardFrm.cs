using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoManagement
{
    public partial class dashBoardFrm : Form
    {
        public dashBoardFrm()
        {
            InitializeComponent();
        }

        frmInventory frmInv;
        frmSalesReport frmSales;
        frmOrders frmOrders;
        frmEmployees frmEmp;
        frmUserLoginMain frmUM;
        private void BtnPos_Click(object sender, EventArgs e)
        {
            if (frmOrders == null || frmOrders.IsDisposed)
            {
                frmOrders = new frmOrders();
                frmOrders.Show();
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            if (frmUM == null || frmUM.IsDisposed)
            {
                frmUM = new frmUserLoginMain();
                frmUM.Show();
            }
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            if (frmInv == null || frmInv.IsDisposed)
            {
                frmInv = new frmInventory();
                frmInv.Show();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
         //   loginFrm lgn = new loginFrm();
         //   lgn.Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            loginFrm lf = new loginFrm();
            lf.Show();
        }
       
        private void BtnMenus_Click(object sender, EventArgs e)
        {
         
        }

        private void BtnSalesReport_Click(object sender, EventArgs e)
        {
            if (frmSales == null || frmSales.IsDisposed)
            {
                frmSales = new frmSalesReport();
                frmSales.Show();
            }
        }
        
        private void BtnEmp_Click(object sender, EventArgs e)
        {
            if (frmEmp == null || frmEmp.IsDisposed)
            {
                frmEmp = new frmEmployees();
                frmEmp.Show();
            }
        }

        private void LblCafe_Click(object sender, EventArgs e)
        {

        }
    }
}
