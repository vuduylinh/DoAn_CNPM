using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banhkem
{
    public partial class Frm_ThongKe : Form
    {
        Model1 BanhkemDB;
        NhanVien user;

        List<NhanVien> listNV;// Khai báo danh sách NV,SP,NL,HD
        List<SanPham> listSP;
        List<NguyenLieu> listNL;
        List<HoaDon> listHD;

        List<rptDoanhThuHoaDon> listRP_DTHD = new List<rptDoanhThuHoaDon>();
        List<rptChiPhiNguyenLieu> listRP_CPNL = new List<rptChiPhiNguyenLieu>();
        public Frm_ThongKe()
        {
            InitializeComponent();
        }

        public Frm_ThongKe(Model1 banhkem, NhanVien nguoidung)
        {
            InitializeComponent();
            BanhkemDB = banhkem;
            user = nguoidung;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        public int kiemtraThangcoHD(int thang,int nam)
        {
            foreach (HoaDon hd in listHD)
            {
                if (hd.NgayLap.Month == thang && hd.NgayLap.Year == nam)
                {
                    return 1;
                }
            }
            return 0;
        }
        public void BindGrid()
        {
            listView1.Items.Clear();
            int stt = 0;
            int thang,nam;


            double  tongdoanhthuHD , tongchiphiNL;
            
                for (nam = dtpBegin.Value.Year; nam <= dtpEnd.Value.Year; nam++)
                    if (nam == dtpEnd.Value.Year)
                    {

                        for (thang = 1; thang <= dtpEnd.Value.Month; thang++)
                        {

                            tongchiphiNL = 0; tongdoanhthuHD = 0;
                            stt++;
                            listView1.Items.Add(stt.ToString());
                            listView1.Items[stt - 1].SubItems.Add(thang.ToString());
                            listView1.Items[stt - 1].SubItems.Add(nam.ToString());

                            foreach (HoaDon hd in listHD)
                            {
                                if (hd.NgayLap.Month == thang && hd.NgayLap.Year == nam)
                                    tongdoanhthuHD += Convert.ToDouble(hd.Tongsotien);

                            }

                            listView1.Items[stt - 1].SubItems.Add(tongdoanhthuHD.ToString());

                            foreach (NguyenLieu nl in listNL)
                            {
                                if ((nl.NgayNhap.Value.Month < dtpEnd.Value.Month || nl.NgayNhap.Value.Month == dtpEnd.Value.Month)

                                    && (nl.NgayNhap.Value.Year < dtpEnd.Value.Year || nl.NgayNhap.Value.Year == dtpEnd.Value.Year))
                                    tongchiphiNL += Convert.ToDouble(nl.Gia) * nl.SoLuong;
                            }
                            listView1.Items[stt - 1].SubItems.Add(tongchiphiNL.ToString());

                            double tienloi = tongdoanhthuHD - tongchiphiNL;
                            listView1.Items[stt - 1].SubItems.Add(tienloi.ToString());
                        }
                    }
                    else
                    {
                        for (thang = dtpBegin.Value.Month; thang < 13; thang++)
                        {

                            tongchiphiNL = 0; tongdoanhthuHD = 0;
                            stt++;
                            listView1.Items.Add(stt.ToString());
                            listView1.Items[stt - 1].SubItems.Add(thang.ToString());
                            listView1.Items[stt - 1].SubItems.Add(nam.ToString());

                            foreach (HoaDon hd in listHD)
                            {
                                if (hd.NgayLap.Month == thang && hd.NgayLap.Year == nam)
                                    tongdoanhthuHD += Convert.ToDouble(hd.Tongsotien);
                            }
                            listView1.Items[stt - 1].SubItems.Add(tongdoanhthuHD.ToString());

                            foreach (NguyenLieu nl in listNL)
                            {
                                if ((nl.NgayNhap.Value.Month < thang || nl.NgayNhap.Value.Month == thang)

                                    && (nl.NgayNhap.Value.Year < nam || nl.NgayNhap.Value.Year == nam))
                                    tongchiphiNL += Convert.ToDouble(nl.Gia) * nl.SoLuong;
                            }
                            listView1.Items[stt - 1].SubItems.Add(tongchiphiNL.ToString());

                            double tienloi = tongdoanhthuHD - tongchiphiNL;
                            listView1.Items[stt - 1].SubItems.Add(tienloi.ToString());
                        }
                    }
                
        }

        private void Frm_ThongKe_Load(object sender, EventArgs e)
        {
            listNV = BanhkemDB.NhanViens.ToList();
            listHD = BanhkemDB.HoaDons.ToList();
            listSP = BanhkemDB.SanPhams.ToList();
            listNL = BanhkemDB.NguyenLieux.ToList();

            BindGrid();
        }

        private void dtpBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBegin.Value > dtpEnd.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc !");
                return;
            }

            BindGrid();
        }

        private void btnReportDT_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (HoaDon hd in listHD)
                {
                    if (hd.NgayLap.Month == Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text) &&
                        hd.NgayLap.Year == Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text))
                    {
                        rptDoanhThuHoaDon rpdthd = new rptDoanhThuHoaDon();
                        rpdthd.MaHD = "HD" + hd.MaHD.ToString();
                        rpdthd.NgayLap = hd.NgayLap.ToString("d");
                        rpdthd.NhanVienLap = hd.NhanVien.TenNV;
                        rpdthd.TienKhachTra = hd.TienKhachTra;
                        rpdthd.ThanhTien = hd.Tongsotien;

                        listRP_DTHD.Add(rpdthd);
                    }
                }


                FrmReportDoanhThuHD f = new FrmReportDoanhThuHD(listRP_DTHD, user.TenNV);
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;

                listRP_DTHD.Clear();
            }
            else MessageBox.Show("Mời bạn nhấp vào dữ liệu trước khi chọn !");
        }

        private void btnReportNL_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (NguyenLieu nl in listNL)
                {
                    if ((nl.NgayNhap.Value.Month < Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text) || nl.NgayNhap.Value.Month == Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text))

                        && (nl.NgayNhap.Value.Year < Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text) || nl.NgayNhap.Value.Year == Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text))
                        )
                    {
                        rptChiPhiNguyenLieu rpcpnl = new rptChiPhiNguyenLieu();
                        rpcpnl.TenNL = "NL" + nl.TenNguyenLieu.ToString();
                        rpcpnl.NgayNhap = nl.NgayNhap.Value.ToString("d");
                        rpcpnl.SoLuong = nl.SoLuong;
                        rpcpnl.Gia = nl.Gia;
                        rpcpnl.ThanhTien = nl.Gia * nl.SoLuong;

                        listRP_CPNL.Add(rpcpnl);
                    }
                }

                FrmReportDoanhThuHD f = new FrmReportDoanhThuHD(listRP_CPNL, user.TenNV);
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;

                listRP_CPNL.Clear();
            }
            else MessageBox.Show("Mời bạn nhấp vào dữ liệu trước khi chọn !");
            
        }
    }
}
