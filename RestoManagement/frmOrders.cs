using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Dapper;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


namespace RestoManagement
{
    public partial class frmOrders : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;
     //   int inventoryId = 0;
        decimal Total = new decimal(0.0);
        int n = 0;


        public frmOrders()
        {
            InitializeComponent();
        }

        private void FrmOrders_Load(object sender, EventArgs e)
        {
            populateDataGridView();
            cboPayType.SelectedIndex = 1;
        }

        private void populateDataGridView()
        {
            dgv.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT *  FROM inventory where active=1  ", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv.DataSource = dtbl;
                sqlCon.Close();
            }
        }

        private void populateDataGridView(string category)
        {
            dgv.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();                                                    
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT *  FROM inventory where category = '" + category + "' and active=1 ", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv.DataSource = dtbl;
                sqlCon.Close();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Dgv_DoubleClick(object sender, EventArgs e)
        {
            if (dgv.CurrentRow.Index != -1)
            {
                decimal unitPrice, subtotal;
                int qty=1;
                string desc = "";
                bool addd = false;

                desc = dgv.CurrentRow.Cells["Description"].Value.ToString().Trim();
                unitPrice = decimal.Parse(dgv.CurrentRow.Cells["SellingPrice"].Value.ToString());

                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (desc == dgvOrders.Rows[i].Cells[2].Value.ToString())
                    {
                        dgvOrders.Rows[i].Cells[0].Value = qty = int.Parse(dgvOrders.Rows[i].Cells[0].Value.ToString()) + 1;
                        subtotal = qty * unitPrice;
                        dgvOrders.Rows[i].Cells[3].Value = string.Format("{0:#,##0.00}", subtotal);
                        dgvOrders.Rows[i].Cells[4].Value = dgv.CurrentRow.Cells["ItemNo"].Value.ToString().Trim();
                        Total += unitPrice;
                        addd = true;
                        break;

                    }
                  
                }
                if (!addd)
                {
                    n = dgvOrders.Rows.Add();
                    dgvOrders.Rows[n].Cells[0].Value = "1";
                    dgvOrders.Rows[n].Cells[1].Value = unitPrice;
                    dgvOrders.Rows[n].Cells[2].Value = desc;
                    subtotal = qty * unitPrice;
                    dgvOrders.Rows[n].Cells[3].Value = string.Format("{0:#,##0.00}", subtotal);
                    dgvOrders.Rows[n].Cells[4].Value = dgv.CurrentRow.Cells["ItemNo"].Value.ToString().Trim();
                    Total += unitPrice;
                }
                txtTotal.Text = Total.ToString("N");
                ComputeAmtDue();
            }
        }

        private void TxtQty_Enter(object sender, EventArgs e)
        {
        }

        private void TxtQty_TextChanged(object sender, EventArgs e)
        {
        }

        private void BtnCleartxt_Click(object sender, EventArgs e)
        {
        //    ClearItems();
        }

        private void BtnClearOrder_Click(object sender, EventArgs e)
        {
            dgvOrders.Rows.Clear();
            Total = 0;
            txtTotal.Text = "0.00";
            txtAmtDue.Text = "0.00";
        }

        private void ClearAll()
        {
            dgvOrders.Rows.Clear();
            txtCustName.Text = "";
            txtTableNo.Text = "";
            Total = 0;
            txtDiscount.Text = "0";
            txtTotal.Text = "0.00";
            txtAmtDue.Text = "0.00";
         //   ClearItems();
        }

        private void ComputeAmtDue()
        {
            decimal disc = new decimal(0.0);
            decimal discTotal = new decimal(0.0);
            if (txtDiscount.Text.Trim() != "" && txtDiscount.Text.Trim() != "0")
            {
                disc = decimal.Parse(txtDiscount.Text) / 100;
                discTotal = Total * disc;
            }
            txtAmtDue.Text = (Total - discTotal).ToString("N");
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtCustName_MouseClick(object sender, MouseEventArgs e)
        {
            txtCustName.Text = "";
        }

        private void TxtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return || (char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                ComputeAmtDue();
            }
        }
        private void BtnX_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        frmInventory frmInv;
        private void BtnInventory_Click(object sender, EventArgs e)
        {
            frmInv = new frmInventory();
            frmInv.ShowDialog();
        }

        private void DgvOrders_DoubleClick(object sender, EventArgs e)
        {
        }

        private void DgvOrders_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
         //   decimal subtotal;
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                Total += decimal.Parse(dgvOrders.Rows[i].Cells[3].Value.ToString());
            }
            txtTotal.Text = Total.ToString();
            ComputeAmtDue();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            populateDataGridView();
        }

        private void BtnBeverages_Click(object sender, EventArgs e)
        {
            populateDataGridView("Breverage");
        }

        private void BtnSandwiches_Click(object sender, EventArgs e)
        {
            populateDataGridView("Sandwich");
        }

        private void BtnMeals_Click(object sender, EventArgs e)
        {
            populateDataGridView("Meals");
        }

        private void BtnPaid_Click(object sender, EventArgs e)
        {
            if (txtAmtDue.Text.Trim() != "0.00")
            {
                SaveOrderHeader();
                PrintReceipt();
                DeductToInv();
                SendToKitchen();
                CheckNeedsReOrder();
                ClearAll();
                
            }
        }

        frmReOrder frmRO;
        private void CheckNeedsReOrder()
        {
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if(IsBelowOrderLevel(row.Cells[4].Value.ToString().Trim()))
                    {
                        if (frmRO == null || frmRO.IsDisposed)
                        {
                            frmRO = new frmReOrder();
                            frmRO.Show();
                        }
                        break;
                    }

                }
                
            }
        }


        bool ReOrderLevel;
        private bool IsBelowOrderLevel(string ItemNo)
        {
            bool orderLevel = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                int onHand, reOrder;
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT * FROM inventory Where ItemNo ='" + ItemNo + "'", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        onHand = int.Parse(reader["QtyOnHand"].ToString());
                        reOrder = int.Parse(reader["ReorderLevel"].ToString());
                        if (onHand <= reOrder)
                        {
                            orderLevel = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in LoadReOrder() : '{ex}' ");
                   // throw;
                }
                finally
                {
                    sqlCon.Close();
                }
            }
            return orderLevel;
        }

        kitchenDisplayFrm kdf;
        ordersDisplayfrm orderD;
        private void SendToKitchen()
        {
            if (kdf == null || kdf.IsDisposed)
            {
                kdf = new kitchenDisplayFrm();
                kdf.Show();

            }

            orderD = new ordersDisplayfrm();
            orderD.MdiParent = kdf;
            //-->
            kdf.tableLayoutPanel1.Controls.Add(orderD);
            orderD.Dock = DockStyle.Fill;
            //<--
            orderD.Show();

            int n;
            foreach (DataGridViewRow dgRow in dgvOrders.Rows)
            {
                if (dgRow.Cells[0].Value != null)
                {
                    n = orderD.dataGridView2.Rows.Add();
                    orderD.dataGridView2.Rows[n].Cells[0].Value = dgRow.Cells[0].Value.ToString();
                    orderD.dataGridView2.Rows[n].Cells[1].Value = dgRow.Cells[2].Value.ToString();
                }
            }
            orderD.llbOrderNo.Text = "Order Number # " + orderNo;
        }

        private void DeductToInv()
        {
            int  qty = 0, onHand;
            string productid;

            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.Cells[0].Value != null)
                        {  
                            qty = int.Parse(row.Cells[0].Value.ToString());
                            productid =  row.Cells[4].Value.ToString().Trim();
                            onHand = getQtyOnhand(productid);
                            cmd = new SqlCommand("UPDATE inventory SET QtyOnHand = @QtyOnHand " +
                                                    "WHERE ItemNo = '" + productid +"'", sqlCon);
                            cmd.Parameters.AddWithValue("@QtyOnHand", (onHand - qty));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DeductToInv: '{ex}' ");
                    // throw;
                }
                finally
                {
                    sqlCon.Close();
                    cmd = null;
                }
            }
        }

        private int getQtyOnhand(string productid)
        {

            int QtyOnHand=0;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT QtyOnHand FROM inventory Where ItemNo = '" + productid + "'", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QtyOnHand = int.Parse(reader["QtyOnHand"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in getQtyOnhand : '{ex}' ");
                    // throw;
                }
                finally
                {
                    sqlCon.Close();
                    cmd = null;
                }
            }
            return QtyOnHand;

        }

        private void SaveOrderHeader()
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try
            {
                cmd = new SqlCommand("INSERT INTO aOrderHeader(Discount, TotalAmt, AmtDue, PayType, " +
                    "Paid, Deleted, UserId, CreationDatetime) " +
                    "Values (@Discount, @TotalAmt, @AmtDue, " +
                    "@PayType, @Paid, @Deleted, @UserId, @CreationDatetime)", conn);
                if (txtDiscount.Text.Trim() == "") txtDiscount.Text = "0";
                if (txtDiscount.Text.Trim() == "0")
                {
                    cmd.Parameters.AddWithValue("@Discount", decimal.Parse("0"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Discount", decimal.Parse(txtDiscount.Text.Trim()) / 100);
                }
                if (txtTotal.Text.Trim() != "")
                    cmd.Parameters.AddWithValue("@TotalAmt", decimal.Parse(txtTotal.Text.Trim()));
                if (txtAmtDue.Text.Trim() != "")
                    cmd.Parameters.AddWithValue("@AmtDue", decimal.Parse(txtAmtDue.Text.Trim()));
                cmd.Parameters.AddWithValue("@PayType", cboPayType.GetItemText(cboPayType.SelectedItem));
                cmd.Parameters.AddWithValue("@Paid", 1);
                cmd.Parameters.AddWithValue("@Deleted", 0);
                cmd.Parameters.AddWithValue("@UserId", Global.GlobalUserID);  
                cmd.Parameters.AddWithValue("@CreationDatetime", now);
                
                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    SaveOrderDetail();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
               // MessageBox.Show(ex.Message, "Insert Error");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }

        }

        int orderNo;
        private int getOrderNo()
        {
            orderNo = 0;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT IDENT_CURRENT('aOrderHeader') AS Current_Identity", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orderNo = int.Parse(reader["Current_Identity"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
                    // throw;
                }
                finally
                {
                    sqlCon.Close();
                    cmd = null;
                }
            }
            return orderNo;
        }

        private void SaveOrderDetail()
        {
            orderNo = getOrderNo();
            conn = new SqlConnection(conStrInv);
            try
            {
                conn.Open();
                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        cmd = new SqlCommand("INSERT INTO aOrderDetails(id, OrderNo, Qty, Description, UnitPrice, Subtotal)" +
                            "Values (@id, @OrderNo, @Qty, @Description, @UnitPrice, @Subtotal )", conn);

                        cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                        cmd.Parameters.AddWithValue("@Qty", int.Parse(row.Cells[0].Value.ToString()));
                        cmd.Parameters.AddWithValue("@UnitPrice", decimal.Parse(row.Cells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@Description", row.Cells[2].Value.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Subtotal", decimal.Parse(row.Cells[3].Value.ToString()));
                        cmd.Parameters.AddWithValue("@id", row.Cells[4].Value.ToString().Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveOrderDetail() : '{ex}' ");
            }
            finally
            {
                conn.Close();
                cmd = null;
            }

        }
        private void PrintReceipt()
        {
               using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString))
               {
                   if (db.State == ConnectionState.Closed)
                       db.Open();
                   string query = "select h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                        "h.Deleted, h.UserId, h.CreationDatetime, " +
                                        "d.Qty, d.Description, d.UnitPrice, d.Subtotal " +
                                    "from aOrderHeader h inner join aOrderDetails d on h.OrderNo = d.OrderNo " +
                                    " Where h.OrderNo = " + orderNo +
                                    " group by h.OrderNo, h.Discount, h.TotalAmt, h.AmtDue, h.PayType, h.Paid, " +
                                        "h.Deleted, h.UserId, h.CreationDatetime, " +
                                        "d.Qty, d.Description, d.UnitPrice, d.Subtotal; ";

                    List<OrderPrinta> list = db.Query<OrderPrinta>(query, commandType: CommandType.Text).ToList();
                    using (frmAReceipt frm = new frmAReceipt(list))
                    {
                        frm.ShowDialog();
                    }
                    db.Close();
               }

        }

        private void DgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void TxtDiscount_TextChanged(object sender, EventArgs e)
        {
        }

        private void TxtDiscount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return || (char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                if (txtDiscount.Text.Trim() != "" || txtDiscount.Text.Trim() != "0")
                {
                    ComputeAmtDue();
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtDiscount_MouseClick(object sender, MouseEventArgs e)
        {
            txtDiscount.Text = "";
        }
    }
}
