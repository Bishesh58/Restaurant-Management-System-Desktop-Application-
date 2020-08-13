using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace RestoManagement
{
    public partial class ordersDisplayfrm : Form
    {
        SqlCommand cmd;
    //    conClass cn = new conClass();
        protected DataGridView dGridView;
        public ordersDisplayfrm()
        {
            InitializeComponent();
           
        }

        System.Timers.Timer t;
        int m, s;

        private void OrdersDisplay_Load(object sender, EventArgs e)
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OnTimeEvent;
            t.Start();

           

        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    s += 1;
                    if (s == 60)
                    {
                        s = 0;
                        m += 1;
                    }
                    lblTick.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
                }));

                if (s >= 30)
                {
                    lblTick.ForeColor = Color.Red;
                }
                if (s >= 30)
                {
                    dataGridView2.ForeColor = Color.Black;
                    dataGridView2.BackgroundColor = Color.Yellow;
                    dataGridView2.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                    dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGridView2.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.Yellow;
                    dataGridView2.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow;
                    dataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
                    dataGridView2.DefaultCellStyle.BackColor = Color.Yellow;
                    dataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Yellow;

                }
                if (m >= 1)
                {
                    dataGridView2.ForeColor = Color.White;
                    dataGridView2.BackgroundColor = Color.Maroon;
                    dataGridView2.DefaultCellStyle.SelectionBackColor = Color.Maroon;
                    dataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
                    dataGridView2.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.Maroon;
                    dataGridView2.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Maroon;
                    dataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
                    dataGridView2.DefaultCellStyle.BackColor = Color.Maroon;
                    dataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Maroon;

                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {

            //SqlConnection con = new SqlConnection();
            //cmd = new SqlCommand("UPDATE  [dbo].[CustomerOrder] SET status = 'True' ", cn.openCon());
            //cmd.ExecuteNonQuery();


            //if (cmd.ExecuteNonQuery() == 1)
            //{
            //    MessageBox.Show("sucessfully updated");
            //}


            // cn.closeCon();
            t.Stop();
            this.Close();
           

        }

        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void OrdersDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Stop();
            Application.DoEvents();
        }

       

       
    }
}
