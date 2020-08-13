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
    public partial class loginFrm : Form
    {

        SqlConnection conn;
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;


        public loginFrm()
        {
            InitializeComponent();
        }
       
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(conStrInv);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SELECT UserLogin.userid, UserLogin.empName,UserLogin.userName, UserLogin.password, UserLogin.roleId, UserLogin.active from UserLogin WHERE username= '" + txtUserName.Text + "' and password='" + txtPassword.Text + "'",sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            
            if (reader.Read())
            {
               // MessageBox.Show(reader["active"].ToString());
                if (reader["active"].ToString() == "0") // admin have access to all all function
                {
                    MessageBox.Show("This Username is Deactived");
                    return;
                }
                if (reader["userName"].ToString().Trim().Equals(txtUserName.Text) && reader["password"].ToString().Trim().Equals(txtPassword.Text))
                {
                    Global.GlobalUserID = int.Parse(reader["userid"].ToString());
                    Global.GlobalUserName = reader["userName"].ToString();
                    if (reader["roleId"].ToString() == "Admin") // admin have access to all all function
                    {
                        this.Hide();
                        dashBoardFrm df = new dashBoardFrm();
                        df.Show();
                        //MessageBox.Show(reader["empName"].ToString(), reader["roleId"].ToString());
                    }

                    if (reader["roleId"].ToString() == "Manager") // manager can not access admin function
                    {
                        this.Hide();
                        dashBoardFrm df = new dashBoardFrm();
                        df.Show();
                        //MessageBox.Show(reader["empName"].ToString(), reader["roleId"].ToString());

                    }

                    if (reader["roleId"].ToString() == "Cashier") // cashier only have access to POS 
                    {
                        this.Hide();
                        dashBoardFrm df = new dashBoardFrm();
                        df.btnAdmin.Enabled = false;
                        df.btnSalesReport.Enabled = false;
                        df.btnEmployees.Enabled = false;
                        df.Show();
                        //MessageBox.Show(reader["empName"].ToString(), reader["roleId"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Invalid UserName or Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUserName.Focus();
                    txtPassword.Clear();
                }
            }

            else
            {
                MessageBox.Show("Invalid UserName or Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                txtPassword.Clear();
            }
            sqlCon.Close();

        } 

        private void TxtUserName_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            txtPassword.PasswordChar = '*';
        }
       
        

        private void TxtUserName_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                errorLblUserName.Visible = true;
            }
            else
            {
                errorLblUserName.Visible = false;
            }

        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                errorLblPassword.Visible = true;
            }
            else
            {
                errorLblPassword.Visible = false;
            }
        }

        private void BtnForgetPassword_Click(object sender, EventArgs e)
        {
            this.Hide();
            resetPasswordForm rsf = new resetPasswordForm();
            rsf.Show();

        }

        private void BtnMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

