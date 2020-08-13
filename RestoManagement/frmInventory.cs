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
using System.IO;



namespace RestoManagement
{
    public partial class frmInventory : Form
    {
        InventoryClass InvClass = new InventoryClass();
        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;
        int inventoryId = 0;
        public frmInventory()
        {
            InitializeComponent();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtItemNo.Text = txtDescription.Text = txtSellingPrice.Text = txtAquiredPrice.Text = txtOnhand.Text = txtUserId.Text = "";
            txtLeadTime.Text = txtReorderLevel.Text = txtUnitsOnOrder.Text = "";
            chkActive.Checked = false;
            btnSave.Text = "Save";
            txtItemNo.ReadOnly = false;
            btnDelete.Enabled = false;
            btnClear.Text = "Clear";
            picBox.Image = null;
            cboCategory.SelectedIndex = -1;
            inventoryId = 0;
        }

        private void FrmInventory_Load(object sender, EventArgs e)
        {
            Clear();
            populateDataGridView();
            populateDataGridViewCategory();
            populateCBOCategoryGrid();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int msg;
            decimal msg1;
            // TextBox Validation ===========>>>>>>>>>>>>>>
            if (txtItemNo.Text.Trim() == "" || txtItemNo.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Product ID is required.", "Save", MessageBoxButtons.OK);
                txtItemNo.Text = "------ Required ------";
                return;
            }
            
           
            if (txtDescription.Text.Trim() == "" || txtDescription.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Product Name is required.", "Save", MessageBoxButtons.OK);
                txtDescription.Text = "------ Required ------";
                return;
            }
            if (!decimal.TryParse(txtSellingPrice.Text.Trim(), out msg1))
            {
                MessageBox.Show("Selling Price is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtSellingPrice.Text = "------ Required Numeric ------";
                return;
            }
            if (!decimal.TryParse(txtAquiredPrice.Text.Trim(), out msg1))
            {
                MessageBox.Show("Aquired Price is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtAquiredPrice.Text = "------ Required Numeric ------";
                return;
            }
            if (!int.TryParse(txtOnhand.Text.Trim(), out msg))
            {
                MessageBox.Show("Units on Hand is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtOnhand.Text = "------ Required Numeric ------";
                return;
            }
            if (cboCategory.Text.Trim() == "" || cboCategory.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Category is required.", "Save", MessageBoxButtons.OK);
                cboCategory.Text = "------ Required ------";
                return;
            }
            if (!int.TryParse(txtLeadTime.Text.Trim(), out msg))
            {
                MessageBox.Show("Lead Time is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtLeadTime.Text = "------ Required Numeric ------";
                return;
            }
            if (!int.TryParse(txtReorderLevel.Text.Trim(), out msg))
            {
                MessageBox.Show("Re-Order Level is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtReorderLevel.Text = "------ Required Numeric ------";
                return;
            }
            if (!int.TryParse(txtUnitsOnOrder.Text.Trim(), out msg))
            {
                MessageBox.Show("Units on Order is required and should be numeric.", "Save", MessageBoxButtons.OK);
                txtUnitsOnOrder.Text = "------ Required Numeric ------";
                return;
            }

            // Enter data in the Inventory Class 
            InvClass.setItemNo(txtItemNo.Text.Trim());
            InvClass.setDescription(txtDescription.Text.Trim());
            if (txtSellingPrice.Text.Trim() != "")
                InvClass.setSellingPrice(decimal.Parse(txtSellingPrice.Text.Trim()));
            if (txtAquiredPrice.Text.Trim()!="")
                InvClass.setAquiredPrice(decimal.Parse(txtAquiredPrice.Text.Trim()));
            if (txtOnhand.Text.Trim() != "")
                InvClass.setQtyOnHand(int.Parse(txtOnhand.Text.Trim()));
            InvClass.setCategory(cboCategory.Text.Trim());
            InvClass.setLeadTime(txtLeadTime.Text.Trim());
            InvClass.setReorderLevel(txtReorderLevel.Text.Trim());
            InvClass.setUnitsOnReoder(txtUnitsOnOrder.Text.Trim());
            InvClass.setUserId(Global.GlobalUserID);
            if (chkActive.Checked == true)
            {
                InvClass.setActive(1);
            }
            else
            {
                InvClass.setActive(0);
            }
            if (btnSave.Text == "Save")  // Invoke when Save button is enabled. 
            {
                if (isProductNoExist(txtItemNo.Text.Trim()))
                {
                    MessageBox.Show("Product ID should be unique.", "Save", MessageBoxButtons.OK);
                    txtItemNo.Text = "------ Required ------";
                    return;
                }
                SaveData(InvClass);
            }
            else                         // Invoke when Update button is enabled. 
            {
                UpdateData(InvClass);
            }
            populateDataGridView();      // Populate datagridview
            Clear();                     // reset all textbox, combo box, image, checkbox to blank.
        }
        private bool isProductNoExist(string itemNO)
        {
            bool productNoExist = false;
  
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT * FROM inventory Where ItemNo ='" + itemNO +"'", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        txtItemNo.Text = reader["ItemNo"].ToString();
                        productNoExist = true;

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
                    //throw;
                }
                finally
                {
                    sqlCon.Close();
                }

            }
            return productNoExist;
        }
        private void SaveData(InventoryClass invClass)
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try  // Saving new Inventory Item to the Database
            {
                cmd = new SqlCommand("INSERT INTO inventory(ItemNo, Description, SellingPrice, AquiredPrice, QtyOnHand, " +
                    "Category, LeadTime, ReorderLevel, UnitsOnOrder, Image, Active, UserId, CreatedDateTime) " +
                    "Values (@ItemNo, @Description, @SellingPrice, @AquiredPrice, @QtyOnHand, " +
                    "@Category, @LeadTime, @ReorderLevel, @UnitsOnOrder, @Image, @Active, @UserId, @CreatedDateTime)", conn);

                cmd.Parameters.AddWithValue("@ItemNo", invClass.getItemNo());
                cmd.Parameters.AddWithValue("@Description", invClass.getDescription());
                cmd.Parameters.AddWithValue("@SellingPrice", invClass.getSellingPrice());
                cmd.Parameters.AddWithValue("@AquiredPrice", invClass.getAquiredPrice());
                cmd.Parameters.AddWithValue("@QtyOnHand", invClass.getQtyOnHand());
                cmd.Parameters.AddWithValue("@Category", invClass.getCategory());
                cmd.Parameters.AddWithValue("@LeadTime", invClass.getLeadTime());
                cmd.Parameters.AddWithValue("@ReorderLevel", invClass.getReorderLevel());
                cmd.Parameters.AddWithValue("@UnitsOnOrder", invClass.getUnitsOnReoder());
                if (dlgOpenImage.FileName != "")
                    cmd.Parameters.AddWithValue("@Image", File.ReadAllBytes(dlgOpenImage.FileName));
                else
                    cmd.Parameters.AddWithValue("@Image", SqlDbType.VarBinary);
                cmd.Parameters.AddWithValue("@Active", invClass.getActive());
                cmd.Parameters.AddWithValue("@UserId", invClass.getUserId());
                cmd.Parameters.AddWithValue("@CreatedDateTime", now);

                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Inserted successfully!", "Inventory");
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
                MessageBox.Show(ex.Message, "Insert Error");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void UpdateData(InventoryClass invClass)
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try
            {
                cmd = new SqlCommand("UPDATE inventory SET ItemNo = @ItemNo , Description = @Description, SellingPrice = @SellingPrice, " +
                                " AquiredPrice = @AquiredPrice, QtyOnHand = @QtyOnHand, Category = @Category, LeadTime = @LeadTime, " +
                                " ReorderLevel =  @ReorderLevel, UnitsOnOrder =  @UnitsOnOrder, Image = @Image, Active = @Active, " +
                                "UserId =  @UserId, CreatedDateTime = @CreatedDateTime WHERE Id = " + inventoryId, conn);

                cmd.Parameters.AddWithValue("@ItemNo", invClass.getItemNo());
                cmd.Parameters.AddWithValue("@Description", invClass.getDescription());
                cmd.Parameters.AddWithValue("@SellingPrice", invClass.getSellingPrice());
                cmd.Parameters.AddWithValue("@AquiredPrice", invClass.getAquiredPrice());
                cmd.Parameters.AddWithValue("@QtyOnHand", invClass.getQtyOnHand());
                cmd.Parameters.AddWithValue("@Category", invClass.getCategory());
                cmd.Parameters.AddWithValue("@LeadTime", invClass.getLeadTime());
                cmd.Parameters.AddWithValue("@ReorderLevel", invClass.getReorderLevel());
                cmd.Parameters.AddWithValue("@UnitsOnOrder", invClass.getUnitsOnReoder());
                if (dlgOpenImage.FileName != "")
                    cmd.Parameters.AddWithValue("@Image", File.ReadAllBytes(dlgOpenImage.FileName));
                else
                    cmd.Parameters.AddWithValue("@Image", SqlDbType.VarBinary);
                cmd.Parameters.AddWithValue("@Active", invClass.getActive());
                cmd.Parameters.AddWithValue("@UserId", invClass.getUserId());
                cmd.Parameters.AddWithValue("@CreatedDateTime", now);
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Updated successfully!", "Inventory");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateData : '{ex}' ");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void populateDataGridView()   // Refresh Inventory datagrid from database
        {
            dgv.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM inventory", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv.DataSource = dtbl;
                sqlCon.Close();
            }
            LoadReOrder();
        }

        private void populateDataGridViewCategory() // Refresh Category datagrid from database
        {
            dgvCategory.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM category ORDER BY category", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgvCategory.DataSource = dtbl;
               // sqlCon.Close();
            }
            populateCBOCategoryGrid();
        }

        private void populateCBOCategoryGrid()   // Refresh Category combo box from database
        {

            cboCategoryGrid.Items.Clear();
            cboCategory.Items.Clear();
            cboCategoryGrid.Items.Add("-- ALL --");
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT category FROM category order by category", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cboCategoryGrid.Items.Add(reader["Category"].ToString());
                        cboCategory.Items.Add(reader["Category"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in populateCBOCategoryGrid() : '{ex}' ");
                    //throw;
                }
                finally
                {
                    sqlCon.Close();
                }

            }
        }

        private void Dgv_DoubleClick(object sender, EventArgs e)  
        {
            // When Item is selected from the datagrid, reflect data to textbox for Update mode or to delete the Item.
            inventoryId = 0;
            if (dgv.CurrentRow.Index != -1)
            {
                decimal dec;
                inventoryId = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);
                using (SqlConnection sqlCon = new SqlConnection(conStrInv))
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM inventory Where Id =" + inventoryId, sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            txtItemNo.Text = reader["ItemNo"].ToString();
                            txtItemNo.ReadOnly = true;
                            txtDescription.Text = reader["Description"].ToString();
                            dec = decimal.Parse(reader["SellingPrice"].ToString());
                            txtSellingPrice.Text = dec.ToString("N");
                            dec = decimal.Parse(reader["AquiredPrice"].ToString());
                            txtAquiredPrice.Text = dec.ToString("N");
                            txtOnhand.Text = reader["QtyOnHand"].ToString();
                            cboCategory.Text = reader["Category"].ToString();
                            txtLeadTime.Text = reader["LeadTime"].ToString();
                            txtReorderLevel.Text = reader["ReorderLevel"].ToString();
                            txtUnitsOnOrder.Text = reader["ReorderLevel"].ToString();
                            if (reader["Active"].ToString() == "True")
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                           // txtUserId.Text = Global.GlobalUserID.ToString();
                            txtUserId.Text = reader["UserId"].ToString();
                            btnSave.Text = "Update";
                            btnClear.Text = "Cancel";
                            btnDelete.Enabled = true;

                            picBox.Image = null;
                            byte[] img = (byte[])reader["Image"];
                            if (img.Length == 0)
                                picBox.Image = null;
                            else
                            {
                                MemoryStream ms = new MemoryStream(img);
                                picBox.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
                        //throw;
                    }
                    finally
                    {
                        sqlCon.Close();
                    }

                }
            }

        }

        private void TxtSellingPrice_KeyPress(object sender, KeyPressEventArgs e)
        {   // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void TxtAquiredPrice_KeyPress(object sender, KeyPressEventArgs e)
        {   // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void TxtOnhand_KeyPress(object sender, KeyPressEventArgs e)
        {   // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Inventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }
            conn = new SqlConnection(conStrInv);
            try
            {
                cmd = new SqlCommand("DELETE inventory WHERE id = " + inventoryId, conn);
                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {   // Confirm from the user before deleting.
                    MessageBox.Show("Data Deleted successfully!", "Inventory");
                }
                populateDataGridView();
                Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnDelete_Click : '{ex}' ");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void TxtLeadTime_TextChanged(object sender, EventArgs e)
        {
        }

        private void TxtLeadTime_KeyPress(object sender, KeyPressEventArgs e)
        {      // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void TxtReorderLevel_KeyPress(object sender, KeyPressEventArgs e)
        {   // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void TxtUnitsOnOrder_KeyPress(object sender, KeyPressEventArgs e)
        {   // Restrict user from entering letter and special characters and should only accept numbers
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void BtnUploadImage_Click(object sender, EventArgs e)
        {
            if (dlgOpenImage.ShowDialog() == DialogResult.OK)
                picBox.Load(dlgOpenImage.FileName);
        }

        private void PicBox_DoubleClick(object sender, EventArgs e)
        {
            if (inventoryId == 0) return;
            if (picBox.Image == null) return;
            Form frm = new Form();
            frm.BackgroundImage = picBox.Image;
            frm.Size = picBox.Image.Size;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void BtnDeleteImage_Click(object sender, EventArgs e)
        {   // Confirm before deleting the Image 
            DialogResult result = MessageBox.Show("Are you sure you want to delete this Image?", "Inventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            conn = new SqlConnection(conStrInv);
            try
            {
                cmd = new SqlCommand("UPDATE inventory SET Image = @Image WHERE id = " + inventoryId, conn);
                cmd.Parameters.AddWithValue("@Image", SqlDbType.VarBinary);
                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Image Deleted successfully!", "Inventory");
                }
                populateDataGridView();
                Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnDeleteImage_Click : '{ex}' ");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void PicBox_Click(object sender, EventArgs e)
        {
        }
        private void Label7_Click(object sender, EventArgs e)
        {
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void TabPage1_Click(object sender, EventArgs e)
        {
        }
        private void Label5_Click(object sender, EventArgs e)
        {
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void cboCategoryGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDataGrid();
        }

        private void loadDataGrid()
        {   // this is for datagrid maintenance
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                string comboT;
                comboT = cboCategoryGrid.GetItemText(cboCategoryGrid.SelectedItem); 
                SqlDataAdapter sqlDa;
                sqlCon.Open();
                if (comboT.Trim().Equals("-- ALL --"))
                {
                    sqlDa = new SqlDataAdapter("SELECT * FROM inventory order by ItemNo asc", sqlCon);
                }
                else
                {
                    sqlDa = new SqlDataAdapter("SELECT * FROM inventory WHERE Category = '" + comboT + "'", sqlCon);
                }
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv.DataSource = dtbl;
                sqlCon.Close();
            }
            Clear();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                decimal dec;
                string itemNo;
                string productName;
                itemNo = txtItemNo.Text.Trim();
                productName = txtDescription.Text.Trim();
                try
                {          
                    sqlCon.Open();
                    if (productName.Equals("") && itemNo.Equals(""))
                    {
                        return;
                    }   // Product Name is the priority from the search over Product ID if both textbox has input
                    if (!productName.Equals(""))
                    {
                        cmd = new SqlCommand("SELECT * FROM inventory WHERE description = '" + productName + "'", sqlCon);
                    }else if (!itemNo.Equals(""))
                    {
                        cmd = new SqlCommand("SELECT * FROM inventory WHERE ItemNo = '" + itemNo + "'", sqlCon);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT * FROM inventory order by ItemNo asc", sqlCon);
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        inventoryId = Convert.ToInt32(reader["Id"]);
                        txtItemNo.Text = reader["ItemNo"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                        dec = decimal.Parse(reader["SellingPrice"].ToString());
                        txtSellingPrice.Text = dec.ToString("N");
                        dec = decimal.Parse(reader["AquiredPrice"].ToString());
                        txtAquiredPrice.Text = dec.ToString("N");
                        txtOnhand.Text = reader["QtyOnHand"].ToString();
                        cboCategory.Text = reader["Category"].ToString();
                        txtLeadTime.Text = reader["LeadTime"].ToString();
                        txtReorderLevel.Text = reader["ReorderLevel"].ToString();
                        txtUnitsOnOrder.Text = reader["UnitsOnOrder"].ToString();
                        if (reader["Active"].ToString() == "True")
                        {
                            chkActive.Checked = true;
                        }
                        else
                        {
                            chkActive.Checked = false;
                        }
                        txtUserId.Text = reader["UserId"].ToString();
                        picBox.Image = null;
                        byte[] img = (byte[])reader["Image"];
                        if (img.Length == 0)
                            picBox.Image = null;
                        else
                        {
                            MemoryStream ms = new MemoryStream(img);
                            picBox.Image = System.Drawing.Image.FromStream(ms);
                        }
                        btnSave.Text = "Update";
                        btnClear.Text = "Cancel";
                        btnDelete.Enabled = true;
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
                }
            }
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
                        if(onHand <= reOrder)
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


        private void DgvCategory_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            conn = new SqlConnection(conStrInv);
            conn.Open();
            try
            {
                DataGridViewRow dgvCatRow = dgvCategory.CurrentRow;
                if (dgvCategory.CurrentRow != null)
                {
                    
                    SqlCommand cmd = new SqlCommand("UPDATE Category SET category = @category WHERE id = @Id", conn);
                    if (dgvCatRow.Cells["nId"].Value != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@Id", dgvCatRow.Cells["nId"].Value);
                        cmd.Parameters.AddWithValue("@category", dgvCatRow.Cells["nCategory"].Value);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Category (category) Values (@category)", conn);
                        cmd.Parameters.AddWithValue("@category", dgvCatRow.Cells["nCategory"].Value == DBNull.Value ? "" : dgvCatRow.Cells["nCategory"].Value);

                    }
                    cmd.ExecuteNonQuery();
                }
                
               //populateDataGridViewCategory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DgvCategory_CellValueChanged : '{ex}' ");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void DgvCategory_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dgvCategory.CurrentRow.Cells["nId"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Are you sure to delete this record?", "Category", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conn = new SqlConnection(conStrInv);
                    conn.Open();

                    cmd = new SqlCommand("Delete from Category where Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(dgvCategory.CurrentRow.Cells["nId"].Value));
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void DgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DgvCategory_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            populateCBOCategoryGrid();
        }

        private void CboCategoryGrid_Enter(object sender, EventArgs e)
        {
        }

        private void CboCategoryGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void Label9_Click(object sender, EventArgs e)
        {
        }

        private void TxtItemNo_TextChanged(object sender, EventArgs e)
        {
        }

        private void TxtItemNo_MouseClick(object sender, MouseEventArgs e)
        {
            txtItemNo.Text = "";
        }

        private void TxtOnhand_MouseClick(object sender, MouseEventArgs e)
        {
            txtOnhand.Text = "";
        }

        private void TxtSellingPrice_TextChanged(object sender, EventArgs e)
        {
        }

        private void TxtDescription_MouseClick(object sender, MouseEventArgs e)
        {
            txtDescription.Text = "";
        }

        private void TxtSellingPrice_MouseClick(object sender, MouseEventArgs e)
        {
            txtSellingPrice.Text = "";
        }

        private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void TxtAquiredPrice_TextChanged(object sender, EventArgs e)
        {
        }

        private void TabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            LoadReOrder();
        }

        private void TxtAquiredPrice_MouseClick(object sender, MouseEventArgs e)
        {
            txtAquiredPrice.Text = "";
        }
    }
    
}