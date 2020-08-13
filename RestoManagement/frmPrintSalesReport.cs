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
    public partial class frmPrintSalesReport : Form
    {
        List<OrderPrinta> _list;
        string _betweenDates;
        // OrderHeader _orderHeader;
      //  OrderHeaderPrint _orderHeaderPrint;
        public frmPrintSalesReport(string betweenDates, List<OrderPrinta> list)
        {
            InitializeComponent();
            _list = list;
            _betweenDates = betweenDates;
           // _orderHeaderPrint = orderHeaderPrint;
        }

        private void FrmPrintSalesReport_Load(object sender, EventArgs e)
        {
            rptSalesReport.SetDataSource(_list);
            rptSalesReport.SetParameterValue("pBetweenDates", _betweenDates);
            crystalReportViewer.ReportSource = rptSalesReport;
            crystalReportViewer.Refresh();
        }
    }
}
