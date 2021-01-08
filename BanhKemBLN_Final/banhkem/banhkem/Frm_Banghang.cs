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
    public partial class Frm_Banghang : Form
    {
        Model1 BanhkemDB;
        NhanVien user;
        List<SanPham> listSP;
        int order_index=0; // số lượng danh sách đặt hàng
        double tongtien = 0; 
        public Frm_Banghang()
        {
            InitializeComponent();
        }
        public Frm_Banghang(Model1 banhkem,NhanVien nguoidung)
        {
            InitializeComponent();
            BanhkemDB = banhkem;
            user = nguoidung;
        }

        void BinGrid(List<SanPham> listSP)
        {
            int index = 0;
            foreach (var sanpham in listSP)
            {
                // Columns đầu tiên là số thứ tự như bên Design, theo em thấy thì columns đầu tiên được mặc định là item chính.
                int stt = index + 1;
                lstProduct.Items.Add(stt.ToString());
                // Những Items sau dòng đầu tiên là phụ. Bắt đầu đưa lần lượt dữ liệu thứ [index] vào Listview

                // Trên hình thức thì STT hiển thị từ 1 đến n.
                // Thực tế thì các Items phụ bắt đầu từ 0. Cho nên ở đây mới là index, ở trên thì là index+1
                lstProduct.Items[index].SubItems.Add(sanpham.TenSP);
                lstProduct.Items[index].SubItems.Add(sanpham.Size.ToString());
                double GiaGoc = Convert.ToDouble(sanpham.GiaGoc);
                lstProduct.Items[index].SubItems.Add(GiaGoc.ToString());
                index++;
            }
        }
        private void Frm_Banghang_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized; // Mở giao diện form Bán hàng với độ phân giải tối đa
            listSP = BanhkemDB.SanPhams.ToList();
            tongtien = 0;
            BinGrid(listSP);
            timer1.Start();// Khởi động hiển thị ngày,giờ
            
            panel6.Visible = false; // Giao diện thanh toán luôn đóng. Trừ khi nhân viên chọn thanh toán
            label5.Text = user.TenNV;
            cbLoaiThucUong.Text = "All";
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNgayHienTai.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); // Hiển thị ngày,giờ đang truy cập
        }

        private void TinhTong()
        {
            

            for (int i = 0; i < lstProductCart.Items.Count; i++)
            {
                double tongtien = 0;
                tongtien = tongtien + Convert.ToDouble(lstProductCart.Items[i].SubItems[5].Text);
                txttotalPrice.Text = tongtien.ToString();

            }
        }

        private void lstProduct_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            int soluong = 1; // Số lượng nhận vào luôn > 0

            if (e.IsSelected) // Nếu sp đã được chọn thì thực thi lệnh
            {
                if (panel6.Visible == false)
                {
                    MessageBox.Show("Mời bạn tạo hóa đơn trước !"); return;
                }
                string Order_Name = lstProduct.Items[e.ItemIndex].SubItems[1].Text;//Biến giữ tạm tên sản phẩm đặt hàng
                string Order_Size = lstProduct.Items[e.ItemIndex].SubItems[2].Text;//Biến giữ tạm kích cỡ sản phẩm đặt hàng
                string Order_Price = lstProduct.Items[e.ItemIndex].SubItems[3].Text;//Biến giữ tạm giá sản phẩm đặt hàng



                // Kiểm tra từ đầu đến cuối danh sách đặt hàng, nếu trùng tên sản phẩm và kích thước thì chỉ
                // thay đổi [Số lượng] và [Giá tổng].
                for (int i = 0; i < lstProductCart.Items.Count; i++)
                    if (lstProductCart.Items[i].SubItems[1].Text == Order_Name && lstProductCart.Items[i].SubItems[4].Text == Order_Size)
                    {


                        // Chuyển đổi số lượng từ kiểu string sang int
                        int sl = Convert.ToInt32(lstProductCart.Items[i].SubItems[2].Text);

                        // Chuyển đổi giá sản phẩm từ kiểu string sang decimal.
                        // Lí do chuyển thành Decimal là cho hợp với kiểu dữ liệu của [Giá Gốc] trong database.
                        // [Giá gốc] trong database là money <=> với decimal.
                        double gia = Convert.ToDouble(lstProductCart.Items[i].SubItems[3].Text);

                        sl++; // Vì sl hiện giờ đang là 1. Cứ mỗi lần nhấp vào, thì sẽ chạy vòng lặp so sánh rồi tăng số lượng lên 
                        soluong = sl; // Ta dùng biến [soluong] cho ListView nên phải đổi để còn dùng ở dưới
                        lstProductCart.Items[i].SubItems[2].Text = soluong.ToString();
                        double tong = sl * gia; // Tổng tiền = số lượng * giá gốc
                        lstProductCart.Items[i].SubItems[5].Text = tong.ToString(); // Thay đổi tổng tiền 
                                                                                    //để tránh tốn bộ nhớ

                        tongtien = tongtien + Convert.ToDouble(Order_Price);
                        txttotalPrice.Text = tongtien.ToString();
                        txtTongTien.Text = txttotalPrice.Text;
                        return;
                    }

                order_index++;// số lượng đặt hàng luôn tăng 1 khi ta nhấp vào 
                lstProductCart.Items.Add(order_index.ToString()); // Thêm Item chính (là STT) bên bảng ds đặt hàng
                lstProductCart.Items[order_index - 1].SubItems.Add(Order_Name);
                lstProductCart.Items[order_index - 1].SubItems.Add(soluong.ToString());

                lstProductCart.Items[order_index - 1].SubItems.Add(Order_Price);
                lstProductCart.Items[order_index - 1].SubItems.Add(Order_Size);
                lstProductCart.Items[order_index - 1].SubItems.Add(Order_Price);
                tongtien = tongtien + Convert.ToDouble(Order_Price);
                txttotalPrice.Text = tongtien.ToString();
                txtTongTien.Text = txttotalPrice.Text;
            }

        }


        private void txtSTK_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                btnXuatHD_Click(sender, e);
                e.Handled = true;
                txtSTK.Text = "";
                return;
            }

            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                
            }    
                
        }

        private void txtSTK_TextChanged(object sender, EventArgs e)
        {

            double tongtien = string.IsNullOrEmpty(txtTongTien.Text) ? 0 : Convert.ToDouble(txtTongTien.Text);
            double thanhtien = tongtien;
            if (txtSTK.Text.Equals(""))
            {
                txtSTK.Text = "0";
            }

            if (!txtSTK.Text.Equals(""))
            {
               

                if (txtSTK.Text.Length <= 20)
                {
                    double stk = string.IsNullOrEmpty(txtSTK.Text) ? 0 : Convert.ToDouble(txtSTK.Text);
                    double kt = (stk - thanhtien);

                    txtTienThoi.Text = kt != 0 ? string.Format("{0:0,0}", kt) : "0";
                    txtThanhTien.Text = (thanhtien != 0) ? string.Format("{0:0,0}", thanhtien) : "0";
                }
                else
                {
                    txtSTK.Text = "0";
                    txtTienThoi.Text = "0"; MessageBox.Show("Vui lòng nhập số tiền nằm khoảng chừng số tiền khách hàng phải trả.");
                }
            }
            else
            {
                txtTienThoi.Text = "0";
            }
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            lstProductCart.Items.Clear();
            order_index = 0;
            txttotalPrice.Text = "0"; tongtien = 0;

        }

        private void lstProductCart_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            int soluong = 0;

            

            if (e.IsSelected)
            {
                
                soluong = Convert.ToInt32(e.Item.SubItems[2].Text);
                Frm_themSoluong_Mon f = new Frm_themSoluong_Mon(soluong);
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;
                if (f.huychonSP == 1)

                {
                    int sothutu = e.ItemIndex;

                    tongtien -= Convert.ToDouble(e.Item.SubItems[5].Text);

                    txttotalPrice.Text = tongtien.ToString();
                    txtTongTien.Text = txttotalPrice.Text;

                   




                    for (int i = sothutu + 1; i < lstProductCart.Items.Count; i++)
                        lstProductCart.Items[i].Text = i.ToString();
                    lstProductCart.Items[e.ItemIndex].Selected = false;
                    lstProductCart.Items.RemoveAt(e.ItemIndex);

                    
                    order_index--;

                    
                    
                }
                else
                {
                    e.Item.SubItems[2].Text = f.soluongSP_Text.ToString();

                    double gia_lucdau = Convert.ToDouble(e.Item.SubItems[5].Text) * soluong;
                    double gia_update = f.soluongSP_Text * Convert.ToDouble(e.Item.SubItems[3].Text);
                    e.Item.SubItems[5].Text = gia_update.ToString();

                    if (gia_lucdau > gia_update) tongtien += gia_lucdau - gia_update;
                    else tongtien += gia_update - gia_lucdau;

                    txttotalPrice.Text = tongtien.ToString();
                    txtTongTien.Text = txttotalPrice.Text;
                }

            }
           
        }

        private void HuyHD()
        {
            panel6.Visible = false;
            txtHD.Text = "";
            txtSTK.Text = "0";
        }
        private void btn_Huyhoadon_Click_1(object sender, EventArgs e)
        {
            HuyHD();
        }

        private void btnTaohoadon_Click_1(object sender, EventArgs e)
        {
            panel6.Visible = true;
            List<HoaDon> listHD;
            listHD = BanhkemDB.HoaDons.ToList();
            txtMaHD.Text = "HD" + (listHD.Count + 1).ToString();
            txtHD.Text = "HD" + (listHD.Count + 1).ToString();
            txtTongTien.Text = txttotalPrice.Text;
            label18.Text = lblNgayHienTai.Text;
        }

        private void ClearSP()
        {
            lstProductCart.Items.Clear();
            order_index = 0;
            txttotalPrice.Text = "0"; tongtien = 0;
            txtTongTien.Text = "0";
        }
        private void btn_LamMoi_Click(object sender, EventArgs e)
        {
            ClearSP();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            lstProduct.Items.Clear();

            int index = 0;
            if (txtTuKhoa.Text == "" && cbLoaiThucUong.Text == "")
            {
                foreach (SanPham sanpham in listSP)
                {
                        int stt = index + 1;
                        lstProduct.Items.Add(stt.ToString());
                        lstProduct.Items[index].SubItems.Add(sanpham.TenSP);
                        lstProduct.Items[index].SubItems.Add(sanpham.Size.ToString());
                        double GiaGoc = Convert.ToDouble(sanpham.GiaGoc);
                        lstProduct.Items[index].SubItems.Add(GiaGoc.ToString());
                        index++;
                }
            }
            
            else
            {
                string string_temp = txtTuKhoa.Text.ToLower();

                foreach (SanPham sanpham in listSP)
                {
   
                    string name_temp = sanpham.TenSP.ToLower();
                    // Neu tu khoa trong txt tim kiem co ton tai trong listview SP list && loai san pham trong combobox == sanpham trong list thi in ra ds
                    if (name_temp.Contains(string_temp) && sanpham.Size == cbLoaiThucUong.Text )
                    {
                        int stt = index + 1;
                        lstProduct.Items.Add(stt.ToString());
                        lstProduct.Items[index].SubItems.Add(sanpham.TenSP);
                        lstProduct.Items[index].SubItems.Add(sanpham.Size.ToString());
                        double GiaGoc = Convert.ToDouble(sanpham.GiaGoc);
                        lstProduct.Items[index].SubItems.Add(GiaGoc.ToString());
                        index++;

                    }
                    else
                    {
                        if (name_temp.Contains(string_temp) && cbLoaiThucUong.Text == "All")
                        {
                            int stt = index + 1;
                            lstProduct.Items.Add(stt.ToString());
                            lstProduct.Items[index].SubItems.Add(sanpham.TenSP);
                            lstProduct.Items[index].SubItems.Add(sanpham.Size.ToString());
                            double GiaGoc = Convert.ToDouble(sanpham.GiaGoc);
                            lstProduct.Items[index].SubItems.Add(GiaGoc.ToString());
                            index++;
                        }
                    }
                }
            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (lstProductCart.Items.Count < 1)
                {
                    MessageBox.Show("Không tạo hóa đơn trống");
                }
                else
                {
                    List<rptCTHD> listRPCTHD = new List<rptCTHD>();
                    string MaHD_temp = txtMaHD.Text.Remove(0, 2);
                    HoaDon hoadon_moi = new HoaDon()
                    {
                        MaNV = user.MaNV,
                        NgayLap = Convert.ToDateTime(lblNgayHienTai.Text),
                        TienKhachTra = Convert.ToDecimal(txtSTK.Text),
                        TienThoi = Convert.ToDecimal(txtTienThoi.Text),
                        Tongsotien = Convert.ToDecimal(txtTongTien.Text),
                    };
                    BanhkemDB.HoaDons.Add(hoadon_moi);
                    BanhkemDB.SaveChanges();

                    for (int i = 0; i < lstProductCart.Items.Count; i++)
                    {
                        foreach (var sanPham in listSP)
                        {
                            if (lstProductCart.Items[i].SubItems[1].Text == sanPham.TenSP && lstProductCart.Items[i].SubItems[4].Text == sanPham.Size)
                            {
                                int sl = Convert.ToInt32(lstProductCart.Items[i].SubItems[2].Text);
                                
                                ChiTietHoaDon chitiethoadon_moi = new ChiTietHoaDon()
                                {
                                    MaHD = Convert.ToInt32(MaHD_temp),
                                    MaSP = sanPham.MaSP,
                                    Gia = Convert.ToDecimal(lstProductCart.Items[i].SubItems[3].Text),
                                    SIZE = lstProductCart.Items[i].SubItems[4].Text,
                                    SoLuong = sl,
                                    ThanhTien = Convert.ToDecimal(lstProductCart.Items[i].SubItems[5].Text)
                                };
                                
                                BanhkemDB.ChiTietHoaDons.Add(chitiethoadon_moi);
                                BanhkemDB.SaveChanges();

                                rptCTHD a = new rptCTHD();
                                a.GiaGoc = Convert.ToDecimal(lstProductCart.Items[i].SubItems[3].Text);
                                a.SIZE = lstProductCart.Items[i].SubItems[4].Text;
                                a.SL = Convert.ToInt32(lstProductCart.Items[i].SubItems[2].Text);
                                a.TenSP = lstProductCart.Items[i].SubItems[1].Text;
                                a.ThanhTien = Convert.ToDecimal(lstProductCart.Items[i].SubItems[5].Text);

                                listRPCTHD.Add(a);
                            }

                        }
                    }

                    FrmReportCTHoaDon f = new FrmReportCTHoaDon(hoadon_moi,listRPCTHD);
                    f.ShowDialog();
                    this.Show();
                    Cursor.Current = Cursors.Arrow;

                    txtSTK.Text = "0";
                    MessageBox.Show("Đã thanh toán hóa đơn");
                    ClearSP(); HuyHD();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }
        }

        private void cbLoaiThucUong_SelectedValueChanged(object sender, EventArgs e)
        {
            txtTuKhoa_TextChanged(sender, e);
        }
    }
}