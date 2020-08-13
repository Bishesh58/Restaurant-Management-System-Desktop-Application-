using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;


namespace RestoManagement
{
    public partial class frmPrintReceipt : Form
    {
        List<OrderPrinta> _list;
        public frmPrintReceipt(List<OrderPrinta> list)
        {
            InitializeComponent();
            _list = list;
        }
        private void FrmPrintReceipt_Load(object sender, EventArgs e)
        {
            rptReceipt11.SetDataSource(_list);

            //   rptReceipt.SetParameterValue("pBetweenDates", _betweenDates);
            crystalReportViewer.ReportSource = rptReceipt11;
            crystalReportViewer.Refresh();
        }

        private void FrmPrintReceipt_Load_1(object sender, EventArgs e)
        {
           /* rptReceipt.SetDataSource(_list);

            //   rptReceipt.SetParameterValue("pBetweenDates", _betweenDates);
            crystalReportViewer.ReportSource = rptReceipt;
            crystalReportViewer.Refresh();*/
        }
    }
}
