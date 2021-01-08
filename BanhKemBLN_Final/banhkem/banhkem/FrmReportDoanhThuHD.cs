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
              //Tạo tham số để sau này truyền vào Report
            ReportParameter[] param = new ReportParameter[2];
            param[0] = new ReportParameter("paraNgayGio", DateTime.Now.ToString("dd/MM/yyyy"));
            param[1] = new ReportParameter("paraTenNV", NguoiLap);
             // Nếu danh sách chi phí nguyên liệu ko có gì => Điều đó có nghĩa là người dùng đã click vào "Xem chi tiết hóa đơn"
            // Vì nhờ sự kiện click "Xem chi phí hóa đơn" ở form thống kê nên đã gửi dữ liệu listRP_DTHD
            // =>>> listRP_CPNL không có gì cả nên count nó == 0
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
               // Tương tự ở trên nhưng lần này là click vào "Xem chi phí nguyên liệu"
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
