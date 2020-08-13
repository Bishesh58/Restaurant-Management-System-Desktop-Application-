using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoManagement
{
    public partial class frmPrintReceipta : Form
    {
        List<OrderPrinta> _list;
        public frmPrintReceipta(List<OrderPrinta> list)
        {
            InitializeComponent();
            _list = list;
        }

        private void FrmPrintReceipta_Load(object sender, EventArgs e)
        {
            rptOrdera1.SetDataSource(_list);

            //   rptReceipt.SetParameterValue("pBetweenDates", _betweenDates);
            crystalReportViewer1.ReportSource = rptOrdera1;
            crystalReportViewer1.Refresh();
        }
    }
}
