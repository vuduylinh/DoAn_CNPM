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
    public partial class FrmReportCTHoaDon : Form
    {
        HoaDon hd;
        List<rptCTHD> listRP_CTHD = new List<rptCTHD>();

        public FrmReportCTHoaDon()
        {
            InitializeComponent();
        }

        public FrmReportCTHoaDon(HoaDon hoadon, List<rptCTHD> listRPCT_HD)
        {

            InitializeComponent();
            // Lay thong tin tu HD vua moi thanh toan
            hd = hoadon;
            // Lay thong tin chi tiet hoa don tu HD moi thanh toan
            listRP_CTHD = listRPCT_HD;
        }



        private void FrmReportCTHoaDon_Load(object sender, EventArgs e)
        {
            ReportParameter[] param = new ReportParameter[7];
            param[0] = new ReportParameter("paraNgayGio", hd.NgayLap.ToString());
            param[1] = new ReportParameter("paraSoHD", "HD" + hd.MaHD.ToString());
            param[2] = new ReportParameter("paraTenNV", hd.NhanVien.TenNV);
            param[3] = new ReportParameter("paraTongTien", hd.Tongsotien.ToString());
            param[4] = new ReportParameter("paraTienThoi", hd.TienThoi.ToString());
            param[5] = new ReportParameter("paraTienKhachTra", hd.TienKhachTra.ToString());
            param[6] = new ReportParameter("paraThanhTien", hd.Tongsotien.ToString());

            this.reportViewer1.LocalReport.ReportPath = "rptHoaDon.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("DataSetCTHD", listRP_CTHD);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.LocalReport.DisplayName = "CỬA HÀNG BÁNH KEM OnePiece SHOP";

            this.reportViewer1.RefreshReport();
        }

        private void FrmReportCTHoaDon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) this.Close();

        }
    }
}
