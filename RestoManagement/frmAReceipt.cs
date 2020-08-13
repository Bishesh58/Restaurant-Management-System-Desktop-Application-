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
    public partial class frmAReceipt : Form
    {

        List<OrderPrinta> _list;
        public frmAReceipt(List<OrderPrinta> list)
        {
            InitializeComponent();
            _list = list;
        }

        private void FrmAReceipt_Load(object sender, EventArgs e)
        {
            rptReceipt1.SetDataSource(_list);
            crystalReportViewer1.ReportSource = rptReceipt1;
            crystalReportViewer1.Refresh();
        }
    }
}
