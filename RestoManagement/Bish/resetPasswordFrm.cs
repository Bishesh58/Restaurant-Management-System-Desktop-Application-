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
    public partial class resetPasswordForm : Form
    {
       
        SqlCommand cmd;
        string conStrInv = ConfigurationManager.ConnectionStrings["POSConnectionString"].ConnectionString;

        SqlConnection sqlCon;
        SqlConnection sqlCon1;
        SqlConnection sqlCon2;
        public resetPasswordForm()
        {
            InitializeComponent();
        }
        private void RbtnLogin_Click(object sender, EventArgs e)
        {

            try
            {
                SqlCommand cmd1;
                SqlCommand cmd2;
                sqlCon = new SqlConnection(conStrInv);
                sqlCon.Open();
                cmd = new SqlCommand("select * from UserLogin where userName = '" + RtxtUserName.Text.Trim() + "'", sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (RtxtPassword.Text == RtxtConfirm.Text) // checking if password is entered correctly
                    {
                        if (reader["password"].ToString().Trim() == RtxtOldPass.Text.Trim())
                        {
                            // authenticating empId and username 
                            if (reader["password"].ToString().Trim().Equals(RtxtOldPass.Text) && reader["username"].ToString().Trim().Equals(RtxtUserName.Text))
                            {
                                sqlCon2 = new SqlConnection(conStrInv);
                                sqlCon2.Open();
                                cmd2 = new SqlCommand("update UserLogin set password=@password where username='"+ RtxtUserName.Text+"'", sqlCon2);
                                cmd2.Parameters.AddWithValue("@password", RtxtConfirm.Text);
                                cmd2.ExecuteNonQuery();
                                sqlCon2.Close();
                                MessageBox.Show("Password changed successfully!!");
                                //  fieldClear();
                                //  return;
                                this.Close();
                                loginFrm Lf = new loginFrm();
                                Lf.Show();
                            }
                            
                        }
                        else if (reader["reset"].ToString() == "0")
                        {
                            MessageBox.Show("You should ask the Manager to reset your password first.");
                            return;
                        }
                        else if (reader["reset"].ToString() == "1")
                        {
                            if (reader["username"].ToString().Trim() + "123" == RtxtOldPass.Text.Trim())
                            {
                                sqlCon1 = new SqlConnection(conStrInv);
                                sqlCon1.Open();
                                cmd1 = new SqlCommand("update UserLogin set password=@password, reset=@reset where username='" + RtxtUserName.Text + "'", sqlCon1);
                                cmd1.Parameters.AddWithValue("@password", RtxtConfirm.Text);
                                cmd1.Parameters.AddWithValue("@reset", "0");
                                cmd1.ExecuteNonQuery();
                                sqlCon1.Close();
                                MessageBox.Show("Password changed successfully!!");
                                //  fieldClear();
                                // return;
                                this.Close();
                                loginFrm Lf = new loginFrm();
                                Lf.Show();
                            }
                            else
                            {
                                MessageBox.Show("Please enter default reset password.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your old password should match.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Confirm password doesn't match to new password");
                    }

                }
                else
                {
                    MessageBox.Show("Username does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // throw;
            }
            finally
            {
                sqlCon.Close();
                
            }
        }
        
        public void fieldClear()
        {
            RtxtUserName.Text = "";
            RtxtOldPass.Text = "";
            RtxtPassword.Text = "";
            RtxtConfirm.Text = "";
        }
        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            this.Close();
            loginFrm Lf = new loginFrm();
            Lf.Show();
        }

       

        private void RtxtUserName_TextChanged(object sender, EventArgs e)
        {
            if (RtxtUserName.Text =="")
            {
                errUserName.Visible = true;
            }
            else
            {
                errUserName.Visible = false;
            }
        }

        private void RtxtEmpId_TextChanged(object sender, EventArgs e)
        {
            if (RtxtOldPass.Text == "")
            {
                errEmpId.Visible = true;
            }
            else
            {
                errEmpId.Visible = false;
            }
        }

        private void RtxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (RtxtPassword.Text == "")
            {
                errNewPassword.Visible = true;
            }
            else
            {
                errNewPassword.Visible = false;
            }
        }

        private void RtxtConfirm_TextChanged(object sender, EventArgs e)
        {
            if (RtxtConfirm.Text == "")
            {
                errConfirmPassword.Visible = true;
            }
            else
            {
                errConfirmPassword.Visible = false;
            }
        }
        private void TxtUserName_Click(object sender, EventArgs e)
        {
            RtxtUserName.Clear();
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            RtxtPassword.Clear();
            RtxtPassword.PasswordChar = '*';
        }

        private void RtxtConfirm_Click(object sender, EventArgs e)
        {
            RtxtConfirm.Clear();
            RtxtConfirm.PasswordChar = '*';
        }

        private void RtxtEmpId_Click(object sender, EventArgs e)
        {
            RtxtOldPass.Clear();
            RtxtOldPass.PasswordChar = '*';
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            loginFrm lf = new loginFrm();
            lf.Show();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
