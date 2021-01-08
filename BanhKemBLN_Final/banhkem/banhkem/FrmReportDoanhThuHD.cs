using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
namespace banhkem
{
    public partial class FrmReportDoanhThuHD : Form
    {
        List<rptDoanhThuHoaDon> listRP_DTHD = new List<rptDoanhThuHoaDon>();
        List<rptChiPhiNguyenLieu> listRP_CPNL = new List<rptChiPhiNguyenLieu>();
        string NguoiLap;
        public FrmReportDoanhThuHD()
        {
            InitializeComponent();
        }

        public FrmReportDoanhThuHD(List <rptDoanhThuHoaDon> listDTHD, string NVLap )
        {
            InitializeComponent();
            listRP_DTHD = listDTHD;
            NguoiLap = NVLap;
        }

        public FrmReportDoanhThuHD(List<rptChiPhiNguyenLieu> listCPNL, string NVLap)
        {
            InitializeComponent();
            listRP_CPNL = listCPNL;
            NguoiLap = NVLap;
        }

        private void FrmReportDoanhThuHD_Load(object sender, EventArgs e)
        {
            ReportParameter[] param = new ReportParameter[2];
            param[0] = new ReportParameter("paraNgayGio", DateTime.Now.ToString("dd/MM/yyyy"));
            param[1] = new ReportParameter("paraTenNV", NguoiLap);

            if (listRP_CPNL.Count == 0)
            {
                this.reportViewer1.LocalReport.ReportPath = "rptDoanhThuHoaDon.rdlc";
                this.reportViewer1.LocalReport.SetParameters(param);
                var reportDataSource = new ReportDataSource("rptDoanhThuHDDataSet", listRP_DTHD);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                this.reportViewer1.RefreshReport();
                return;
            }    
            else
            
            {
                this.reportViewer1.LocalReport.ReportPath = "rptChiPhiNguyenLieu.rdlc";
                this.reportViewer1.LocalReport.SetParameters(param);
                var reportDataSource = new ReportDataSource("rptChiPhiNLDataSet", listRP_CPNL);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                this.reportViewer1.RefreshReport();
                
            }    

            
            
            
        }

        private void FrmReportDoanhThuHD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) this.Close();
        }
    }
}
