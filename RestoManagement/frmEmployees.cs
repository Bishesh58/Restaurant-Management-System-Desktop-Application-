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
    public partial class frmEmployees : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;
        int empId;
        public frmEmployees()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void populateDataGridView()   // Refresh datagrid from database
        {
            dgvEmp.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM employee", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgvEmp.DataSource = dtbl;
                sqlCon.Close();
            }
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            populateDataGridView();
            Clear();
         //   populateCBORoles();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (txtGivenName.Text.Trim() == "" || txtGivenName.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Given name is required.", "Save", MessageBoxButtons.OK);
                txtGivenName.Text = "------ Required ------";
                return;
            }
            if (txtLastName.Text.Trim() == "" || txtLastName.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Last name is required.", "Save", MessageBoxButtons.OK);
                txtLastName.Text = "------ Required ------";
                return;
            }
            if (txtContact.Text.Trim() == "" || txtContact.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Contact is required.", "Save", MessageBoxButtons.OK);
                txtContact.Text = "------ Required ------";
                return;
            }
            if (txtEmail.Text.Trim() == "" || txtEmail.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("Email is required.", "Save", MessageBoxButtons.OK);
                txtEmail.Text = "------ Required ------";
                return;
            }

            if (isGivenNameExist(txtGivenName.Text.Trim()) && btnSave.Text == "Save")
            {
                MessageBox.Show("Given name must be unique.", "Save", MessageBoxButtons.OK);
                return;
            }
            if (btnSave.Text == "Save")  // Invoke when Save button is enabled. 
            {
                SaveData();
            }
            else                         // Invoke when Update button is enabled. 
            {
                UpdateData();
            }
            populateDataGridView();
            Clear();

        }

        private bool isGivenNameExist(string givenName)
        {
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                string Name;
                bool found = false;
                Name = txtGivenName.Text.Trim();
                if (Name != "")
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM employee Where givenNames = '" + Name + "'", sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            found = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in BtnSerch_Click : '{ex}' ");
                        // throw;
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                }
                else
                {
                    found = false;
                }
                return found;
            }
        }
        private void SaveData()
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try  // Saving new employee to the Database
            {
                cmd = new SqlCommand("INSERT INTO employee( givenNames, familyName, phNo, emailId, active, enrolledDate ) " +   // roleId,
                    "Values (@roleId, @givenNames, @familyName, @phNo, @emailId, @active, @enrolledDate)", conn);

                cmd.Parameters.AddWithValue("@givenNames", txtGivenName.Text.Trim());
                cmd.Parameters.AddWithValue("@familyName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@phNo", txtContact.Text.Trim());
                cmd.Parameters.AddWithValue("@emailId", txtEmail.Text.Trim());
                if (chkActive.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@active",1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@active", 0);
                }
                cmd.Parameters.AddWithValue("@enrolledDate", now);
               
                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Inserted successfully!", "Employees");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveData : '{ex}' ");
                MessageBox.Show(ex.Message, "Insert Error");
                //throw;
            }
            finally
            {
                conn.Close();
                cmd = null;
            }
        }

        private void UpdateData()
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try
            {
                cmd = new SqlCommand("UPDATE employee SET givenNames = @givenNames, familyName = @familyName , phNo = @phNo, " +
                         "emailId = @emailId, active = @active, enrolledDate = @enrolledDate WHERE empId = " + empId, conn);

                //   cmd.Parameters.AddWithValue("@roleId", cboRoleName.Text.Trim());   roleId = @roleId , 
                cmd.Parameters.AddWithValue("@givenNames", txtGivenName.Text.Trim());
                cmd.Parameters.AddWithValue("@familyName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@phNo", txtContact.Text.Trim());
                cmd.Parameters.AddWithValue("@emailId", txtEmail.Text.Trim());
                if (chkActive.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@active", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@active", 0);
                }
                cmd.Parameters.AddWithValue("@enrolledDate", now);
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Updated successfully!", "Employees");
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

        void Clear()
        {
            txtEmpId.Text = txtGivenName.Text = txtLastName.Text = txtContact.Text = txtEmail.Text = "";
            chkActive.Checked = false;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            btnClear.Text = "Clear";
        //    cboRoleName.SelectedIndex = -1;
            empId = 0;

        }

        /*private void populateCBORoles()   // Refresh Roles combo box from database
        {

            cboRoleName.Items.Clear();
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT roleName FROM roles order by roleId", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cboRoleName.Items.Add(reader["roleName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in populateCBORoles() : '{ex}' ");
                    //throw;
                }
                finally
                {
                    sqlCon.Close();
                }

            }
        }*/
        private void DgvEmp_DoubleClick(object sender, EventArgs e)
        {
            // When Item is selected from the datagrid, reflect data to textbox for Update mode or to delete the Item.
            empId = 0;
            if (dgvEmp.CurrentRow.Index != -1)
            {
                empId = Convert.ToInt32(dgvEmp.CurrentRow.Cells["empid1"].Value);
                using (SqlConnection sqlCon = new SqlConnection(conStrInv))
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM employee Where empid =" + empId, sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            txtEmpId.Text = reader["empId"].ToString();
                            txtGivenName.Text = reader["givenNames"].ToString();
                            txtLastName.Text = reader["familyName"].ToString();
                            txtContact.Text = reader["phNo"].ToString();
                            txtEmail.Text = reader["emailId"].ToString();
                        //    cboRoleName.Text = reader["roleId"].ToString();
                            if (reader["Active"].ToString() == "True")
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                        }
                        btnSave.Text = "Update";
                        btnClear.Text = "Cancel";
                        btnDelete.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in DgvEmp_DoubleClick : '{ex}' ");
                        //throw;
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Employees", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }
            conn = new SqlConnection(conStrInv);
            try
            {
                cmd = new SqlCommand("DELETE employee WHERE empId = " + empId, conn);
                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {   // Confirm from the user before deleting.
                    MessageBox.Show("Data Deleted successfully!", "Employees");
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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnSerch_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                string Name;
                bool found=false;
                Name = txtGivenName.Text.Trim();
                if (Name != "")
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM employee Where givenNames = '" + Name +"'", sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            txtEmpId.Text = reader["empId"].ToString();
                            txtGivenName.Text = reader["givenNames"].ToString();
                            txtLastName.Text = reader["familyName"].ToString();
                            txtContact.Text = reader["phNo"].ToString();
                            txtEmail.Text = reader["emailId"].ToString();
                        //    cboRoleName.Text = reader["roleId"].ToString();
                            if (reader["Active"].ToString() == "True")
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                            found = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in BtnSerch_Click : '{ex}' ");
                        // throw;
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                    if (found)
                    {
                        btnSave.Text = "Update";
                        btnClear.Text = "Cancel";
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Not found.", "Search by Name");
                    }
                }
                else
                {
                    MessageBox.Show("Please search by given name.", "Search by Name");
                }
            }
        }

        private void TxtGivenName_MouseClick(object sender, MouseEventArgs e)
        {
/*            txtGivenName.Text = "";
            txtLastName.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";*/
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DgvEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TxtGivenName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

