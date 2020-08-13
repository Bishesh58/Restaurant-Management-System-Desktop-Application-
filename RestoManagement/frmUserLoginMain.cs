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
    public partial class frmUserLoginMain : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;
        int userId;

        public frmUserLoginMain()
        {
            InitializeComponent();
        }

        private void FrmUserLoginMain_Load(object sender, EventArgs e)
        {
            populateCBORoles();
            populateCBOEmpNames();
            populateDataGridView();
            Clear();

        }
        private void Label2_Click(object sender, EventArgs e)
        {
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                string Name;
                bool found = false;
                userId = 0;
                Name = txtUserName.Text.Trim();
                if (Name != "")
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM UserLogin Where userName = '" + Name + "'", sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            userId = int.Parse(reader["userId"].ToString());
                            txtUserID.Text = reader["userId"].ToString();
                            cboRoleName.Text = reader["roleId"].ToString();
                          //  txtEmpID.Text = reader["empId"].ToString();
                            cboEmpName.Text = reader["empName"].ToString();
                            txtUserName.Text = reader["userName"].ToString();
                            txtPassword.Text = reader["password"].ToString();
                            if (reader["active"].ToString() == "True")
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                            if (reader["reset"].ToString() == "True")
                            {
                                chkReset.Checked = true;
                            }
                            else
                            {
                                chkReset.Checked = false;
                            }
                            found = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in BtnSearch_Click : '{ex}' ");
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
                    MessageBox.Show("Please search by Username.", "Search by Username");
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (cboRoleName.Text.Trim() == "" )
            {
                MessageBox.Show("Role name is required.", "Save", MessageBoxButtons.OK);
                cboRoleName.Focus();
                return;
            }
            if (cboEmpName.Text.Trim() == "")
            {
                MessageBox.Show("Employee name is required.", "Save", MessageBoxButtons.OK);
                cboEmpName.Focus();
                return;
            }
            
            if (txtUserName.Text.Trim() == "" || txtUserName.Text.Trim() == "------ Required ------")
            {
                MessageBox.Show("User name is required.", "Save", MessageBoxButtons.OK);
                txtUserName.Text = "------ Required ------";
                return;
            }
            if (txtUserName.Text.Trim().Length < 6 )
            {
                MessageBox.Show("Username should atleast have 6 characters long.", "Save", MessageBoxButtons.OK);
                txtUserName.Focus();
                return;
            }

            if (btnSave.Text == "Save")
            {
                if (txtPassword.Text.Trim() == "" || txtPassword.Text.Trim() == "------ Required ------")
                {
                    MessageBox.Show("Password is required.", "Save", MessageBoxButtons.OK);
                    txtPassword.Text = "------ Required ------";
                    return;
                }
                if (txtPassword.Text.Trim().Length < 4)
                {
                    MessageBox.Show("Password should atleast have 4 characters long.", "Save", MessageBoxButtons.OK);
                    txtPassword.Focus();
                    return;
                }
                if (txtPassword.Text.Trim() != txtConfirmPass.Text.Trim())
                {
                    MessageBox.Show("Password and Confirm password should match.", "Save", MessageBoxButtons.OK);
                    txtPassword.Focus();
                    return;
                }
            }

            if (cboRoleName.Text.Trim() == "")
            {
                MessageBox.Show("Role is required.", "Save", MessageBoxButtons.OK);
                cboRoleName.Focus();
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
        private void SaveData()
        {
            conn = new SqlConnection(conStrInv);
            DateTime now = DateTime.Now;
            try  // Saving to the Database
            {
                cmd = new SqlCommand("INSERT INTO UserLogin(empName, roleId, userName, password, reset, active, CreationDate ) " +
                    "Values (@empName, @roleId, @userName, @password, @reset, @active, @CreationDate)", conn);

                cmd.Parameters.AddWithValue("@empName", cboEmpName.Text.Trim());
                cmd.Parameters.AddWithValue("@roleId", cboRoleName.Text.Trim());
                cmd.Parameters.AddWithValue("@userName", txtUserName.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                if (chkReset.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@reset", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@reset", 0);
                }
                if (chkActive.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@active", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@active", 0);
                }
                cmd.Parameters.AddWithValue("@CreationDate", now);

                conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Inserted successfully!", "User Login Maintenance");
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
                cmd = new SqlCommand("UPDATE UserLogin SET roleId = @roleId , empName = @empName, userName = @userName, " +   //, password = @password, " +
                         "reset = @reset, active = @active, CreationDate = @CreationDate WHERE userId = " + userId, conn);

                cmd.Parameters.AddWithValue("@roleId", cboRoleName.Text.Trim());
                cmd.Parameters.AddWithValue("@empName", cboEmpName.Text.Trim());
                cmd.Parameters.AddWithValue("@userName", txtUserName.Text.Trim());
              //  cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                if (chkActive.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@active", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@active", 0);
                }
                if (chkReset.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@reset", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@reset", 0);
                }
                cmd.Parameters.AddWithValue("@CreationDate", now);
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data Updated successfully!", "User Maintenance");
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

        private void populateDataGridView()
        {
            dgvUser.AutoGenerateColumns = false;
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM UserLogin", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgvUser.DataSource = dtbl;
                sqlCon.Close();
            }
        }

        private void Clear()
        {
            txtUserID.Text = txtUserName.Text = txtPassword.Text = txtConfirmPass.Text = "";
            chkActive.Checked = false;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            btnClear.Text = "Clear";
            txtPassword.BackColor = Color.White;
            txtConfirmPass.BackColor = Color.White;
            txtPassword.ReadOnly = false;
            txtConfirmPass.ReadOnly = false;
            cboRoleName.SelectedIndex = -1;
            cboEmpName.SelectedIndex = -1;
            userId = 0;
        }

        private void DgvUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void populateCBORoles()   // Refresh Roles combo box from database
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
                    cmd = null;
                }

            }
        }

        private void populateCBOEmpNames()   // Refresh Roles combo box from database
        {

            cboEmpName.Items.Clear();
            using (SqlConnection sqlCon = new SqlConnection(conStrInv))
            {
                try
                {
                    sqlCon.Open();
                    cmd = new SqlCommand("SELECT givenNames FROM Employee order by empId", sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cboEmpName.Items.Add(reader["givenNames"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in populateCBOEmpNames() : '{ex}' ");
                    //throw;
                }
                finally
                {
                    sqlCon.Close();
                    cmd = null;
                }

            }
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void DgvUser_DoubleClick(object sender, EventArgs e)
        {
            userId = 0;
            if (dgvUser.CurrentRow.Index != -1)
            {
                userId = Convert.ToInt32(dgvUser.CurrentRow.Cells["userId1"].Value);
                using (SqlConnection sqlCon = new SqlConnection(conStrInv))
                {
                    try
                    {
                        sqlCon.Open();
                        cmd = new SqlCommand("SELECT * FROM UserLogin Where userId =" + userId, sqlCon);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            txtUserID.Text = reader["userId"].ToString();
                            txtUserName.Text = reader["userName"].ToString();
                            cboRoleName.Text = reader["roleId"].ToString();
                            cboEmpName.Text = reader["empName"].ToString();
                        //    txtPassword.Text = reader["password"].ToString();
                        //    if (reader["active"].ToString() == "True")
                            if (Convert.ToInt32(dgvUser.CurrentRow.Cells["Active"].Value)==1)
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                            if (reader["reset"].ToString() == "True")
                            {
                                chkReset.Checked = true;
                            }
                            else
                            {
                                chkReset.Checked = false;
                            }
                        }
                        btnSave.Text = "Update";
                        btnClear.Text = "Cancel";
                        btnDelete.Enabled = true;
                        txtPassword.BackColor = Color.Gray;
                        txtConfirmPass.BackColor = Color.Gray;
                        txtPassword.ReadOnly = true;
                        txtConfirmPass.ReadOnly = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in DgvUser_MouseDoubleClick : '{ex}' ");
                        //throw;
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void DgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ChkActive_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
