using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace RestoManagement
{
    public partial class frmReOrder : Form
    {
        InventoryClass InvClass = new InventoryClass();
        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;
        public frmReOrder()
        {
            InitializeComponent();
        }

        private void FrmReOrder_Load(object sender, EventArgs e)
        {
            LoadReOrder();
        }

        private void LoadReOrder() // Load all Items that needs RE-Order
        {

            int n;
            dgvReOrder.Rows.Clear();
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                int onHand, reOrder;
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT * FROM inventory order by ItemNo asc", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        onHand = int.Parse(reader["QtyOnHand"].ToString());
                        reOrder = int.Parse(reader["ReorderLevel"].ToString());
                        if (onHand <= reOrder)
                        {
                            n = dgvReOrder.Rows.Add();
                            dgvReOrder.Rows[n].Cells[0].Value = reader["ItemNo"].ToString();
                            dgvReOrder.Rows[n].Cells[1].Value = reader["Description"].ToString();
                            dgvReOrder.Rows[n].Cells[2].Value = reader["QtyOnHand"].ToString();
                            dgvReOrder.Rows[n].Cells[3].Value = reader["ReorderLevel"].ToString();
                            dgvReOrder.Rows[n].Cells[4].Value = reader["UnitsOnOrder"].ToString();
                            dgvReOrder.Rows[n].Cells[5].Value = reader["LeadTime"].ToString();
                            // dgvReOrder.Rows[n].DefaultCellStyle.BackColor = 
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in LoadReOrder() : '{ex}' ");
                    throw;
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
