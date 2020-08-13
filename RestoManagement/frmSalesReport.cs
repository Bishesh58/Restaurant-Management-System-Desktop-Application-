using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace RestoManagement
{
    public partial class frmSalesReport : Form
    {
        CustomerSalesOrder cusSales = new CustomerSalesOrder();
        /*SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;*/
        string betweenDates;
        public frmSalesReport()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (dtFromDate.Value > dtToDate.Value)
            {
                MessageBox.Show("From Date must be proir date to To Date before loading.", "Loading", MessageBoxButtons.OK);
                dtFromDate.Focus();
            }
            populateDataGridView();
        }

        private void populateDataGridView()
        {
            dgvSalesReport.AutoGenerateColumns = false;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
               
                string query = "select h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                    "h.Deleted, h.UserId, h.CreationDatetime " +
                                "from aOrderHeader h inner join aOrderDetails d on h.OrderNo = d.OrderNo " +
                                 $" Where h.CreationDatetime between '{dtFromDate.Text}' and '{dtToDate.Text}' " +
                                "group by h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid,  " +
                                    "h.Deleted, h.UserId, h.CreationDatetime ";

                orderHeaderPrintaBindingSource.DataSource = db.Query<OrderHeaderPrinta>(query, commandType: CommandType.Text);

                query = "select h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                "h.Deleted, h.UserId, h.CreationDatetime, " +
                                    "d.Qty, d.Description, d.UnitPrice, d.Subtotal " +
                            "from aOrderHeader h inner join aOrderDetails d on h.OrderNo = d.OrderNo " +
                            $" Where h.CreationDatetime between '{dtFromDate.Text}' and '{dtToDate.Text}' " +
                            "group by h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                "h.Deleted, h.UserId, h.CreationDatetime, " +
                                "d.Qty, d.Description, d.UnitPrice, d.Subtotal; ";

                orderPrintaBindingSource.DataSource = db.Query<OrderPrinta>(query, commandType: CommandType.Text);
                betweenDates = $"From '{dtFromDate.Text}' to '{dtToDate.Text}'";
            }
        }

        private void FrmSalesReport_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtFromDate.Value = now;
            dtToDate.Value = now;

            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "yyyy/MM/dd";

            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "yyyy/MM/dd";
        }

        private void DtFromDate_ValueChanged(object sender, EventArgs e)
        {
        }

        private void DtToDate_ValueChanged(object sender, EventArgs e)
        {
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (betweenDates != null)
            {
            
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        string query = "select h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                             "h.Deleted, h.UserId, h.CreationDatetime, " +
                                             "d.Qty, d.Description, d.UnitPrice, d.Subtotal " +
                                        "from aOrderHeader h inner join aOrderDetails d on h.OrderNo = d.OrderNo " +
                                        $" Where h.CreationDatetime between '{dtFromDate.Text}' and '{dtToDate.Text}' " +
                                        "group by h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                              "h.Deleted, h.UserId, h.CreationDatetime, " +
                                              "d.Qty, d.Description, d.UnitPrice, d.Subtotal; ";

                        List<OrderPrinta> list = db.Query<OrderPrinta>(query, commandType: CommandType.Text).ToList();
                        using(frmPrintSalesReport frm = new frmPrintSalesReport(betweenDates, list))
                        {
                            frm.ShowDialog();
                        }

                }
            }
            else
            {
                MessageBox.Show("Please select dates then click Load first.");
            }
        }
    }
}
