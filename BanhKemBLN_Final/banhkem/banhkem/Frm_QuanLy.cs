using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace banhkem
{
    public partial class Frm_QuanLy : Form
    {
        Model1 BanhkemDB;// Khai báo sẵn database + người dùng
        NhanVien user;
        List<NhanVien> listNV;// Khai báo danh sách NV,SP,NL,HD
        List<SanPham> listSP;
        List<NguyenLieu> listNL;
        List<HoaDon> listHD;
        List<NhaCungCap> listNCC;

        public Frm_QuanLy()
        {
            InitializeComponent();
        }

        public Frm_QuanLy(Model1 banhkem, NhanVien nguoidung)
        {
            InitializeComponent();
            BanhkemDB = banhkem; // Lấy dữ liệu database + người dùng từ form Main trước đó 
            user = nguoidung;
        }


        public void BindGrid_listNV()
        {
            int h = 0;
            listView1.Items.Clear();
            //Xuất dữ liệu nhân viên lên ListView1
            foreach (var nhanvien in listNV)
            {
                int indexNV = h + 1; // Số thứ tự trên thực tế bắt đầu từ 1 nên h+1
                listView1.Items.Add(indexNV.ToString());
                listView1.Items[h].SubItems.Add("NV" + nhanvien.MaNV.ToString()); // Dữ liệu hệ thống có thứ tự từ 0 nên ở đây 
                listView1.Items[h].SubItems.Add(nhanvien.TenNV);                  // mới là h
                listView1.Items[h].SubItems.Add(nhanvien.SDT);
                listView1.Items[h].SubItems.Add(nhanvien.NgaySinh.Value.ToString("d"));
                listView1.Items[h].SubItems.Add(nhanvien.Email);
                listView1.Items[h].SubItems.Add(nhanvien.GioiTinh);
                listView1.Items[h].SubItems.Add(nhanvien.CaLamViec.TenCaLamViec);

                // Lương = lương cơ bản (từ NV) * hệ số lương (từ Chức vụ)
                double luong = Convert.ToDouble(nhanvien.Luongcoban) * Convert.ToDouble(nhanvien.ChucVu.Hesoluong);
                listView1.Items[h].SubItems.Add(luong.ToString());
                listView1.Items[h].SubItems.Add(nhanvien.ChucVu.TenCV);
                listView1.Items[h].SubItems.Add(nhanvien.NgayGiaNhap.Value.Date.ToString());
                listView1.Items[h].SubItems.Add(nhanvien.Matkhau);
                listView1.Items[h].SubItems.Add(nhanvien.CMND);
                listView1.Items[h].SubItems.Add(nhanvien.Trangthai);

                h++; // tăng stt sau khi thêm xong nhũng dữ liệu ở trên
            }

        }

        public void BindGrid_listSP()
        {
            int i = 0;
            listView2.Items.Clear();
            //Xuất dữ liệu sản phẩm lên ListView2
            foreach (var sanpham in listSP)
            {
                int indexSP = i + 1;// Số thứ tự trên thực tế bắt đầu từ 1 nên i+1
                listView2.Items.Add(indexSP.ToString());
                listView2.Items[i].SubItems.Add("SP" + sanpham.MaSP.ToString()); // Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                listView2.Items[i].SubItems.Add(sanpham.TenSP);                   // mới là i
                listView2.Items[i].SubItems.Add(sanpham.Size.ToString());
                listView2.Items[i].SubItems.Add(Convert.ToDouble(sanpham.GiaGoc).ToString());
                listView2.Items[i].SubItems.Add(sanpham.Soluong.ToString());
                listView2.Items[i].SubItems.Add(sanpham.NguyenLieu.TenNguyenLieu);
                listView2.Items[i].SubItems.Add(sanpham.Trangthai);

                i++;
            }

        }

        public void BindGrid_listNL()
        {
            int j = 0;
            listView3.Items.Clear();

            //Xuất dữ liệu sản phẩm lên ListView3
            foreach (var nguyenlieu in listNL)
            {
                int indexNL = j + 1;// Số thứ tự trên thực tế bắt đầu từ 1 nên j+1
                listView3.Items.Add(indexNL.ToString());
                listView3.Items[j].SubItems.Add("NL" + nguyenlieu.MaNguyenLieu.ToString());// Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                listView3.Items[j].SubItems.Add(nguyenlieu.TenNguyenLieu);                 // mới là j
                listView3.Items[j].SubItems.Add(nguyenlieu.SoLuong.ToString());
                listView3.Items[j].SubItems.Add(Convert.ToDouble(nguyenlieu.Gia).ToString());
                listView3.Items[j].SubItems.Add(nguyenlieu.NgayNhap.Value.ToString("d"));
                listView3.Items[j].SubItems.Add(nguyenlieu.LoaiNguyenLieu.TenLoaiNguyenLieu);
                listView3.Items[j].SubItems.Add(nguyenlieu.NhaCungCap.TenNCC);
                listView3.Items[j].SubItems.Add(nguyenlieu.TrangThai);
                j++;

            }

        }


        public void BindGrid_listHD()
        {
            int k = 0;


            // Xóa bỏ danh sách hiện tại để sau này load lại sẽ tự cập nhật lại.

            listView4.Items.Clear();

            foreach (var hoadon in listHD)
            {
                // Ngay bat dau <= Ngay lap hoa don <= Ngay ket thuc
                if ((dtpStartDay.Value.Date < hoadon.NgayLap.Date) || (dtpStartDay.Value.Date == hoadon.NgayLap.Date)
                && ((dtpEndDay.Value > hoadon.NgayLap) || (dtpEndDay.Value.Date == hoadon.NgayLap.Date)))
                {
                    int indexHD = k + 1;
                    listView4.Items.Add(indexHD.ToString());
                    listView4.Items[k].SubItems.Add("HD" + hoadon.MaHD.ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.Tongsotien).ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.TienKhachTra).ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.TienThoi).ToString());
                    listView4.Items[k].SubItems.Add(hoadon.NgayLap.ToString());
                    listView4.Items[k].SubItems.Add(hoadon.NhanVien.TenNV);
                    k++;
                }
            }
        }

        public void BindGrid_listNCC()
        {
            int l = 0;
            listView5.Items.Clear();

            foreach (var ncc in listNCC)
            {
                // Ngay bat dau <= Ngay lap hoa don <= Ngay ket thuc
                // Số thứ tự trên thực tế bắt đầu từ 1 nên j+1
                l++;
                listView5.Items.Add(l.ToString());
                listView5.Items[l - 1].SubItems.Add(ncc.MaNCC);// Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                listView5.Items[l - 1].SubItems.Add(ncc.TenNCC);                 // mới là j
                listView5.Items[l - 1].SubItems.Add(ncc.SDT);
                listView5.Items[l - 1].SubItems.Add(ncc.Email);
                listView5.Items[l - 1].SubItems.Add(ncc.DiaChi);
                
            }
        }


        // Đem dữ liệu vào tất cả các Combobox
        private void FillComboBox()
        {
            cbxCaLamViec.DataSource = BanhkemDB.CaLamViecs.ToList(); // Nguồn dữ liệu CaLamViec lấy từ Database
            cbxCaLamViec.DisplayMember = "TenCaLamViec"; //Chỉ hiển thị tên của ca làm việc lên combobox
            cbxCaLamViec.ValueMember = "MaCaLamViec"; // Với mỗi dòng lựa chọn trên combobox đều tương ứng với Mã ca làm việc của chúng.

            cbxChucVu_NV.DataSource = BanhkemDB.ChucVus.ToList(); // Nguồn dữ liệu CaLamViec lấy từ Database
            cbxChucVu_NV.DisplayMember = "TenCV";//Chỉ hiển thị tên chức vụ lên combobox
            cbxChucVu_NV.ValueMember = "MaCV"; // Với mỗi dòng lựa chọn trên combobox đều tương ứng với Mã chức vụ của chúng.

            cbxNL_Main.DataSource = BanhkemDB.NguyenLieux.ToList();
            cbxNL_Main.DisplayMember = "TenNguyenLieu";
            cbxNL_Main.ValueMember = "MaNguyenLieu";

            cbxLoaiNL.DataSource = BanhkemDB.LoaiNguyenLieux.ToList();
            cbxLoaiNL.DisplayMember = "TenLoaiNguyenLieu";
            cbxLoaiNL.ValueMember = "MaLoaiNguyenLieu";

            cbxNhaCungCapNL.DataSource = BanhkemDB.NhaCungCaps.ToList();
            cbxNhaCungCapNL.DisplayMember = "TenNCC";
            cbxNhaCungCapNL.ValueMember = "MaNCC";

        }

        //Load lại danh sách các dữ liệu
        private void Load_ListView()
        {
            listNV = BanhkemDB.NhanViens.ToList();
            listHD = BanhkemDB.HoaDons.ToList();
            listSP = BanhkemDB.SanPhams.ToList();
            listNL = BanhkemDB.NguyenLieux.ToList();
            listNCC = BanhkemDB.NhaCungCaps.ToList();
        }

        private void Frm_QuanLy_Load(object sender, EventArgs e)
        {
            rbNu.Checked = true; //Mục nữ được chọn tự động
            Load_ListView();

            BindGrid_listHD(); // In dữ liệu lên tất cả ListView
            BindGrid_listNL();
            BindGrid_listNV();
            BindGrid_listSP();
            BindGrid_listNCC();


            FillComboBox(); // In dữ liệu lên tất cả Combobox

        }

        //DƯỚI ĐÂY LÀ CODE THẢO TÁC VỚI TAB PAGE NHÂN VIÊN

        // Xóa trắng các ô nhập dữ liệu
        private void ClearText_NV()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = ""; txtMatKhau_NV.Text = "";
            txtCMND_NV.Text = ""; txtSDT_NV.Text = "";
            txtLuongNV.Text = ""; txtEmail_NV.Text = "";
        }


        // Thêm nhân viên vào database
        private void btnAddNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNV.Text != "")
                {
                    // Vì txtMaNV bị khóa, và nó cũng tự động tăng nên ta không thể gõ dữ liệu vào đó được.
                    // Nếu thêm NV mới vào trong khi mã nhân viên đó lại là của tài khoản cũ thì sẽ mất thẩm mỹ.
                    // Nên người dùng phải làm mới mã nhân viên để mã nhân viên thành khoảng trắng ""
                    MessageBox.Show("Xin hãy làm mới danh sách sách !");
                    return;
                }


                if (txtTenNV.Text == "" || txtMatKhau_NV.Text == "" || txtCMND_NV.Text == "" || txtSDT_NV.Text == "" ||
                        txtLuongNV.Text == "" || txtEmail_NV.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể thêm nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }


                else
                {
                    // Khi nhập đủ rồi thì bắt đầu quá trình thêm NV vào database
                    NhanVien nv_them = new NhanVien()
                    {

                        TenNV = txtTenNV.Text,
                        Matkhau = txtMatKhau_NV.Text,
                        CMND = txtCMND_NV.Text,
                        SDT = txtSDT_NV.Text,
                        NgaySinh = dtpNgaySinhNV.Value,
                        Luongcoban = Convert.ToDecimal(txtLuongNV.Text),
                        NgayGiaNhap = dtpNgayGiaNhap_NV.Value,
                        Email = txtEmail_NV.Text,
                        Trangthai = cbxTrangThai_NV.Text,
                        MaCaLamViec = Convert.ToInt32(cbxCaLamViec.SelectedValue.ToString()), //Lấy dữ liệu của dòng combobox CLV đang chọn. Chi tiết ở dòng 125
                        MaCV = Convert.ToInt32(cbxChucVu_NV.SelectedValue.ToString()) //Lấy dữ liệu của dòng combobox CLV đang chọn. Chi tiết ở dòng 129
                    };

                    // Nếu đã chọn Nam thì nv_them lấy giá trị là Nam. Còn ngược lại thì chỉ có thể là Nữ
                    if (rbNam.Checked == true) nv_them.GioiTinh = rbNam.Text; else nv_them.GioiTinh = rbNu.Text;
                    //
                    string currentday= DateTime.Now.ToString("dd/MM/yyyy");
                    if (nv_them.Trangthai == "Đã nghỉ việc") nv_them.NgayNghi = Convert.ToDateTime(currentday);
                    else nv_them.NgayNghi = null;
                    // Thêm vào Database
                    BanhkemDB.NhanViens.Add(nv_them);
                    BanhkemDB.SaveChanges(); //Lưu Database

                    Load_ListView(); BindGrid_listNV(); // Load lại database 
                    MessageBox.Show("Thêm thành công !");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // Nếu nhân viên này được chọn thì thực thi các lệnh sau đây
            if (e.IsSelected)
            {
                // Dán dữ liệu được chọn bởi chuột lên textbox
                txtMaNV.Text = e.Item.SubItems[1].Text;
                txtTenNV.Text = e.Item.SubItems[2].Text; txtSDT_NV.Text = e.Item.SubItems[3].Text;
                dtpNgaySinhNV.Value = Convert.ToDateTime(e.Item.SubItems[4].Text);
                txtEmail_NV.Text = e.Item.SubItems[5].Text;

                //So sánh nếu là Nam thì radiobutton Nam phải dc chọn và Nữ thì ngược lại 
                if (Equals(e.Item.SubItems[6].Text, "Nam       "))
                {
                    // Do kiểu dữ liệu của GioiTinh là nchar(10) nên phải chừa khoảng cách đủ 10 như ở trên
                    rbNam.Checked = true;
                    rbNu.Checked = false;
                    ;
                }
                else
                {
                    rbNam.Checked = false;
                    rbNu.Checked = true;
                }
                cbxCaLamViec.Text = e.Item.SubItems[7].Text;
                cbxChucVu_NV.Text = e.Item.SubItems[9].Text;

                // Lương ở dưới ListView được lấy từ lương cơ bản * hệ số lương.
                // Cho nên muốn tìm ngược lại lương cơ bản thì ta phải tìm ngược lại hệ số lương.
                // Mà hệ số lương thì nằm ở table ChucVu.
                double luongcoban = 0;
                List<ChucVu> listCV;
                listCV = BanhkemDB.ChucVus.ToList();

                //Xét từng chức vụ trong danh sách ChucVu với ChucVu trong ListView. 
                // Nếu đúng thì chia ngược lại lấy lương cơ bản
                foreach (ChucVu a in listCV)
                {
                    if (Convert.ToInt32(cbxChucVu_NV.SelectedValue.ToString()) == a.MaCV)
                        luongcoban = Convert.ToDouble(e.Item.SubItems[8].Text) / a.Hesoluong;
                }
                txtLuongNV.Text = luongcoban.ToString();


                dtpNgayGiaNhap_NV.Value = Convert.ToDateTime(e.Item.SubItems[10].Text);
                txtMatKhau_NV.Text = e.Item.SubItems[11].Text;
                txtCMND_NV.Text = e.Item.SubItems[12].Text;
                cbxTrangThai_NV.Text = e.Item.SubItems[13].Text;

            }
        }

        private void btnClearNV_Click(object sender, EventArgs e)
        {
            ClearText_NV();
        }

        private void btnEditNV_Click(object sender, EventArgs e)
        {
            try
            {
                // Nếu không nhập đủ thì xuất thông báo.
                if (txtTenNV.Text == "" || txtMatKhau_NV.Text == "" || txtCMND_NV.Text == "" || txtSDT_NV.Text == "" ||
                        txtLuongNV.Text == "" || txtEmail_NV.Text == "")
                {
                    MessageBox.Show("Bạn không thể sửa nếu để trống bất kỳ trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    txtMaNV.Text = txtMaNV.Text.Remove(0, 2); // Xóa 2 ký tự đầu tiên của MaNV trong ListView (đó là 2 chữ "NV")

                    // Vì mã NV trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    int Ma_nv = Convert.ToInt32(txtMaNV.Text);
                    var nhanvien_them = BanhkemDB.NhanViens.Find(Ma_nv);

                    //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                    if (nhanvien_them != null)
                    {
                        nhanvien_them.TenNV = txtTenNV.Text;
                        nhanvien_them.Matkhau = txtMatKhau_NV.Text;
                        nhanvien_them.CMND = txtCMND_NV.Text;
                        nhanvien_them.SDT = txtSDT_NV.Text;
                        nhanvien_them.NgaySinh = dtpNgaySinhNV.Value;
                        nhanvien_them.Luongcoban = Convert.ToDecimal(txtLuongNV.Text);
                        nhanvien_them.NgayGiaNhap = dtpNgayGiaNhap_NV.Value;
                        nhanvien_them.Email = txtEmail_NV.Text;
                        nhanvien_them.Trangthai = cbxTrangThai_NV.Text;
                        nhanvien_them.MaCaLamViec = Convert.ToInt32(cbxCaLamViec.SelectedValue.ToString());
                        nhanvien_them.MaCV = Convert.ToInt32(cbxChucVu_NV.SelectedValue.ToString());
                        if (rbNam.Checked == true) nhanvien_them.GioiTinh = rbNam.Text; else nhanvien_them.GioiTinh = rbNu.Text;

                        string currentday = DateTime.Now.ToString("dd/MM/yyyy");
                        if (nhanvien_them.Trangthai == "Đã nghỉ việc") nhanvien_them.NgayNghi = Convert.ToDateTime(currentday);
                        else nhanvien_them.NgayNghi = null;
                        //Lưu vào database
                        BanhkemDB.Entry(nhanvien_them).State = EntityState.Modified;
                        BanhkemDB.SaveChanges();

                        // Thêm lại chữ "NV" để còn in vào ListView sau khi load lại,
                        txtMaNV.Text = "NV" + txtMaNV.Text;

                        Load_ListView(); BindGrid_listNV();
                        MessageBox.Show("Cập nhật thành công !");
                    }
                    else MessageBox.Show("Không tìm thấy nhân viên này !");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }

        }


        //WARNING : DƯỚI ĐÂY LÀ CODE THAO TÁC VỚI SẢN PHẨM

        private void btnClearSP_Click(object sender, EventArgs e)
        {
            txtMaSP.Text = ""; txtTenSP.Text = "";
            txtGiaSP.Text = ""; txtSoLuongSP.Text = "";
        }


        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // Nếu nhân viên này được chọn thì thực thi các lệnh sau đây
            if (e.IsSelected)
            {
                // Dán dữ liệu được chọn bởi chuột lên textbox
                txtMaSP.Text = e.Item.SubItems[1].Text;
                txtTenSP.Text = e.Item.SubItems[2].Text;
                if (e.Item.SubItems[3].Text == "S") cbxSizeSP.Text = "S";
                else
                {
                    if (e.Item.SubItems[3].Text == "L") cbxSizeSP.Text = "L";
                    else cbxSizeSP.Text = "M";
                }
                txtGiaSP.Text = e.Item.SubItems[4].Text;
                txtSoLuongSP.Text = e.Item.SubItems[5].Text;
                cbxNL_Main.Text = e.Item.SubItems[6].Text;
                if (e.Item.SubItems[7].Text == "Đang hoạt động")
                {
                    rbTrangThaiSP_Danghd.Checked = true;
                    rbTrangThaiSP_Ngungban.Checked = false;
                }
                else
                {
                    rbTrangThaiSP_Danghd.Checked = false;
                    rbTrangThaiSP_Ngungban.Checked = true;
                }
            }
        }

        private void btnAddSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text != "")
                {
                    // Vì txtMaNV bị khóa, và nó cũng tự động tăng nên ta không thể gõ dữ liệu vào đó được.
                    // Nếu thêm NV mới vào trong khi mã nhân viên đó lại là của tài khoản cũ thì sẽ mất thẩm mỹ.
                    // Nên người dùng phải làm mới mã nhân viên để mã nhân viên thành khoảng trắng ""
                    MessageBox.Show("Xin hãy làm mới danh sách sách !");
                    return;
                }


                if (txtTenSP.Text == "" || txtGiaSP.Text == "" || txtSoLuongSP.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể thêm nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    // Khi nhập đủ rồi thì bắt đầu quá trình thêm NV vào database
                    SanPham sp_them = new SanPham()
                    {

                        TenSP = txtTenSP.Text,
                        GiaGoc = Convert.ToDecimal(txtGiaSP.Text),
                        Size = cbxSizeSP.Text,
                        Soluong = Convert.ToInt32(txtSoLuongSP.Text),
                        MaNguyenLieu = Convert.ToInt32(cbxNL_Main.SelectedValue.ToString())

                    };

                    // Nếu đã chọn Nam thì nv_them lấy giá trị là Nam. Còn ngược lại thì chỉ có thể là Nữ
                    if (rbTrangThaiSP_Danghd.Checked == true) sp_them.Trangthai = rbTrangThaiSP_Danghd.Text;
                    else sp_them.Trangthai = rbTrangThaiSP_Ngungban.Text;

                    BanhkemDB.SanPhams.Add(sp_them);
                    BanhkemDB.SaveChanges();

                    Load_ListView(); BindGrid_listSP(); // Load lại database 
                    MessageBox.Show("Thêm thành công !");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }

        }

        private void btnDelSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text == "")
                {
                    // Vì txtMaSP bị khóa, và ta không thể chủ động nhập mã sản phẩm được.
                    // Nên người dùng phải chọn 1 sản phẩm ở dưới listView để xuất hiện mã SP
                    MessageBox.Show("Xin hãy chọn sản phẩm cần xóa !");
                    return;
                }
                else
                {
                    txtMaSP.Text = txtMaSP.Text.Remove(0, 2); // Xóa 2 ký tự đầu tiên của MaNV trong ListView (đó là 2 chữ "SP")

                    // Vì mã SP trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    int Ma_sp = Convert.ToInt32(txtMaSP.Text);
                    var sanpham_xoa = BanhkemDB.SanPhams.Find(Ma_sp);

                    //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                    if (sanpham_xoa != null)
                    {

                        BanhkemDB.SanPhams.Remove(sanpham_xoa);
                        BanhkemDB.SaveChanges();

                        // Thêm lại chữ "SP" để còn in vào ListView sau khi load lại,

                        Load_ListView(); BindGrid_listSP();
                        MessageBox.Show("Xóa thành công !");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnUpdateSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text == "")
                {
                    MessageBox.Show("Xin hãy chọn sản phẩm cần sửa !");
                    return;
                }


                if (txtTenSP.Text == "" || txtGiaSP.Text == "" || txtSoLuongSP.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể chỉnh sửa nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }

                else
                {
                    txtMaSP.Text = txtMaSP.Text.Remove(0, 2); // Xóa 2 ký tự đầu tiên của MaNV trong ListView (đó là 2 chữ "SP")

                    // Vì mã SP trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    int Ma_sp = Convert.ToInt32(txtMaSP.Text);
                    var sanpham_sua = BanhkemDB.SanPhams.Find(Ma_sp);

                    //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                    if (sanpham_sua != null)
                    {
                        sanpham_sua.TenSP = txtTenSP.Text;
                        sanpham_sua.GiaGoc = Convert.ToDecimal(txtGiaSP.Text);
                        sanpham_sua.Size = cbxSizeSP.Text;
                        sanpham_sua.Soluong = Convert.ToInt32(txtSoLuongSP.Text);
                        sanpham_sua.MaNguyenLieu = Convert.ToInt32(cbxNL_Main.SelectedValue.ToString());
                        if (rbTrangThaiSP_Danghd.Checked == true) sanpham_sua.Trangthai = rbTrangThaiSP_Danghd.Text;
                        else sanpham_sua.Trangthai = rbTrangThaiSP_Ngungban.Text;

                        BanhkemDB.Entry(sanpham_sua).State = EntityState.Modified;
                        BanhkemDB.SaveChanges();

                        // Thêm lại chữ "SP" để còn in vào ListView sau khi load lại,

                        txtMaSP.Text = "SP" + txtMaSP.Text;

                        Load_ListView(); BindGrid_listSP();
                        MessageBox.Show("Cập nhật thành công !");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }

        //WARNING : DƯỚI ĐÂY LÀ CODE THAO TÁC VỚI DỮ LIỆU NGUYÊN LIỆU

        private void btnClearNL_Click(object sender, EventArgs e)
        {
            txtMaNL.Text = ""; txtTenNL.Text = "";
            txtSoLuongNL.Text = ""; txtGiaNL.Text = "";
        }

        private void listView3_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                // Dán dữ liệu được chọn bởi chuột lên textbox
                txtMaNL.Text = e.Item.SubItems[1].Text;
                txtTenNL.Text = e.Item.SubItems[2].Text;
                txtSoLuongNL.Text = e.Item.SubItems[3].Text;
                txtGiaNL.Text = e.Item.SubItems[4].Text;
                dtpNgayNhapNL.Value = Convert.ToDateTime(e.Item.SubItems[5].Text);
                cbxLoaiNL.Text = e.Item.SubItems[6].Text;
                cbxNhaCungCapNL.Text = e.Item.SubItems[7].Text;
                if (e.Item.SubItems[8].Text == "Còn hàng")
                {
                    rbTrangThaiNL_ConHang.Checked = true;
                    rbTrangThaiNL_HetHang.Checked = false;
                }
                else
                {
                    rbTrangThaiNL_ConHang.Checked = false;
                    rbTrangThaiNL_HetHang.Checked = true;
                }
            }
        }

        private void btnThemNL_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNL.Text != "")
                {
                    // Vì txtMaNV bị khóa, và nó cũng tự động tăng nên ta không thể gõ dữ liệu vào đó được.
                    // Nếu thêm NV mới vào trong khi mã nhân viên đó lại là của tài khoản cũ thì sẽ mất thẩm mỹ.
                    // Nên người dùng phải làm mới mã nhân viên để mã nhân viên thành khoảng trắng ""
                    MessageBox.Show("Xin hãy làm mới danh sách sách !");
                    return;
                }


                if (txtTenNL.Text == "" || txtGiaNL.Text == "" || txtSoLuongNL.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể thêm nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    // Khi nhập đủ rồi thì bắt đầu quá trình thêm NV vào database
                    NguyenLieu nl_them = new NguyenLieu()
                    {

                        TenNguyenLieu = txtTenNL.Text,
                        Gia = Convert.ToDecimal(txtGiaNL.Text),
                        SoLuong = Convert.ToInt32(txtSoLuongNL.Text),
                        NgayNhap = Convert.ToDateTime(dtpNgayNhapNL.Value),
                        MaLoaiNguyenLieu = Convert.ToInt32(cbxLoaiNL.SelectedValue.ToString()),
                        MaNCC = cbxNhaCungCapNL.SelectedValue.ToString()

                    };

                    // Nếu đã chọn Nam thì nv_them lấy giá trị là Nam. Còn ngược lại thì chỉ có thể là Nữ
                    if (rbTrangThaiNL_ConHang.Checked == true) nl_them.TrangThai = rbTrangThaiNL_ConHang.Text;
                    else nl_them.TrangThai = rbTrangThaiNL_HetHang.Text;

                    BanhkemDB.NguyenLieux.Add(nl_them);

                    BanhkemDB.SaveChanges();

                    Load_ListView(); BindGrid_listNL(); // Load lại database 
                    MessageBox.Show("Thêm thành công !");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }
        }

        private void btnXoaNL_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNL.Text == "")
                {
                    // Vì txtMaSP bị khóa, và ta không thể chủ động nhập mã sản phẩm được.
                    // Nên người dùng phải chọn 1 sản phẩm ở dưới listView để xuất hiện mã SP
                    MessageBox.Show("Xin hãy chọn nguyên liệu cần xóa !");
                    return;
                }
                else
                {
                    txtMaNL.Text = txtMaNL.Text.Remove(0, 2); // Xóa 2 ký tự đầu tiên của MaNV trong ListView (đó là 2 chữ "NL")

                    // Vì mã SP trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    int Ma_nl = Convert.ToInt32(txtMaNL.Text);
                    var nguyenlieu_xoa = BanhkemDB.NguyenLieux.Find(Ma_nl);

                    //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                    if (nguyenlieu_xoa != null)
                    {

                        BanhkemDB.NguyenLieux.Remove(nguyenlieu_xoa);
                        BanhkemDB.SaveChanges();

                        // Thêm lại chữ "SP" để còn in vào ListView sau khi load lại,

                        Load_ListView(); BindGrid_listNL();
                        MessageBox.Show("Xóa thành công !");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnSuaNL_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNL.Text == "")
                {
                    MessageBox.Show("Xin hãy chọn nguyên liệu cần sửa !");
                    return;
                }


                if (txtTenNL.Text == "" || txtGiaNL.Text == "" || txtSoLuongNL.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể chỉnh sửa nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                }

                else
                {
                    txtMaNL.Text = txtMaNL.Text.Remove(0, 2); // Xóa 2 ký tự đầu tiên của MaNL trong ListView (đó là 2 chữ "NL")

                    // Vì mã NL trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    int Ma_nl = Convert.ToInt32(txtMaNL.Text);
                    var nguyenlieu_sua = BanhkemDB.NguyenLieux.Find(Ma_nl);

                    //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                    if (nguyenlieu_sua != null)
                    {
                        nguyenlieu_sua.TenNguyenLieu = txtTenNL.Text;
                        nguyenlieu_sua.Gia = Convert.ToDecimal(txtGiaNL.Text);
                        nguyenlieu_sua.SoLuong = Convert.ToInt32(txtSoLuongNL.Text);
                        nguyenlieu_sua.MaLoaiNguyenLieu = Convert.ToInt32(cbxLoaiNL.SelectedValue.ToString());
                        nguyenlieu_sua.MaNCC = cbxNhaCungCapNL.SelectedValue.ToString();
                        nguyenlieu_sua.NgayNhap = Convert.ToDateTime(dtpNgayNhapNL.Value);
                        if (rbTrangThaiNL_ConHang.Checked == true) nguyenlieu_sua.TrangThai = rbTrangThaiNL_ConHang.Text;
                        else nguyenlieu_sua.TrangThai = rbTrangThaiNL_HetHang.Text;

                        BanhkemDB.Entry(nguyenlieu_sua).State = EntityState.Modified;
                        BanhkemDB.SaveChanges();

                        // Thêm lại chữ "SP" để còn in vào ListView sau khi load lại,

                        txtMaNL.Text = "NL" + txtMaNL.Text;
                        Load_ListView(); BindGrid_listNL();
                        MessageBox.Show("Cập nhật thành công !");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }


        private void dtpEndDay_ValueChanged(object sender, EventArgs e)
        {
            if (dtpStartDay.Value > dtpEndDay.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc !");
                listView4.Items.Clear();

            }
            else BindGrid_listHD();
        }

        private void listView4_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            List<ChiTietHoaDon> listCTHD = BanhkemDB.ChiTietHoaDons.ToList();

            if (e.IsSelected)
            {
                string MaHD = e.Item.SubItems[1].Text;
                MaHD = MaHD.Remove(0, 2);

                HoaDon temp = new HoaDon();
                List<rptCTHD> listRPCTHD = new List<rptCTHD>();

                foreach (HoaDon hd in listHD)
                {
                    if (Convert.ToInt32(MaHD) == hd.MaHD)
                        temp = hd;
                }

                foreach (ChiTietHoaDon cthd in listCTHD)
                {
                    if (Convert.ToInt32(MaHD) == cthd.MaHD)
                    {
                        rptCTHD a = new rptCTHD();
                        a.GiaGoc = cthd.Gia.Value;
                        a.SIZE = cthd.SIZE;
                        a.SL = cthd.SoLuong.Value;
                        a.TenSP = cthd.SanPham.TenSP;
                        a.ThanhTien = cthd.ThanhTien.Value;
                        
                        listRPCTHD.Add(a);
                    }
                }

                FrmReportCTHoaDon f = new FrmReportCTHoaDon(temp, listRPCTHD);
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;

            }
        }

        private void btnClearNCC_Click(object sender, EventArgs e)
        {
            txtMaNCC.Text = ""; txtTenNCC.Text = "";
            txtDiaChi_Ncc.Text = ""; txtEmailNcc.Text = "";
            txtSDT_Ncc.Text = "";
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            try
            {
                
                foreach (NhaCungCap ncc in listNCC)
                {
                    if (txtMaNCC.Text == ncc.MaNCC) { MessageBox.Show("Đã bị trùng mã nhà cung cấp !"); return; }
                }
                
                if ( txtTenNCC.Text == "" || txtDiaChi_Ncc.Text == "" || txtEmailNcc.Text == "" || txtSDT_Ncc.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể thêm nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);return;
                }
                    
                    // Khi nhập đủ rồi thì bắt đầu quá trình thêm NV vào database
                    NhaCungCap ncc_them = new NhaCungCap()
                    {
                        MaNCC =txtMaNCC.Text,
                        TenNCC = txtTenNCC.Text,
                        DiaChi = txtDiaChi_Ncc.Text,
                        Email = txtEmailNcc.Text,
                        SDT = txtSDT_Ncc.Text,   
                    };

                    // Nếu đã chọn Nam thì nv_them lấy giá trị là Nam. Còn ngược lại thì chỉ có thể là Nữ
                   

                    BanhkemDB.NhaCungCaps.Add(ncc_them);
                    BanhkemDB.SaveChanges();

                    Load_ListView(); BindGrid_listNCC(); // Load lại database 
                    MessageBox.Show("Thêm thành công !");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }
        }

        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            try
            {
                    // Vì mã SP trong database là kiểu int nên ta phải đổi nó lại rồi đi tìm trong database
                    
                    var nhacc_xoa = BanhkemDB.NhaCungCaps.Find(txtMaNCC.Text);

                //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                if (nhacc_xoa != null)
                {

                    BanhkemDB.NhaCungCaps.Remove(nhacc_xoa);
                    BanhkemDB.SaveChanges();

                    // Thêm lại chữ "SP" để còn in vào ListView sau khi load lại,

                    Load_ListView(); BindGrid_listNCC();
                    MessageBox.Show("Xóa thành công !");
                }
                else MessageBox.Show("Không tồn tại nhà cung cấp trong danh sách !");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenNCC.Text == "" || txtDiaChi_Ncc.Text == "" || txtEmailNcc.Text == "" || txtSDT_Ncc.Text == "")
                {
                    // Nếu không nhập đủ thì xuất thông báo.
                    MessageBox.Show("Bạn không thể thêm nếu như để trống một trường dữ liệu nào.", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                
                var nhacc_sua = BanhkemDB.NhaCungCaps.Find(txtMaNCC.Text);

                //Khi tìm thấy thì thay đổi dữ liệu các thuộc tính của nó
                if (nhacc_sua != null)
                {
                    nhacc_sua.TenNCC = txtTenNCC.Text;
                    nhacc_sua.SDT = txtSDT_Ncc.Text;
                    nhacc_sua.DiaChi = txtDiaChi_Ncc.Text;
                    nhacc_sua.Email = txtEmailNcc.Text;



                    BanhkemDB.Entry(nhacc_sua).State = EntityState.Modified;
                    BanhkemDB.SaveChanges();
                    Load_ListView(); BindGrid_listNCC();
                    MessageBox.Show("Cập nhật thành công !");                                    }
                else MessageBox.Show("Không tồn tại nhà cung cấp trong danh sách !");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
                // Nếu bắt được lỗi logic thì in nó lên MessageBox.
            }
        }

        private void listView5_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                // Dán dữ liệu được chọn bởi chuột lên textbox
                txtMaNCC.Text = e.Item.SubItems[1].Text;
                txtTenNCC.Text = e.Item.SubItems[2].Text;
                txtDiaChi_Ncc.Text = e.Item.SubItems[3].Text;
                txtEmailNcc.Text = e.Item.SubItems[4].Text;
                txtSDT_Ncc.Text = e.Item.SubItems[5].Text;
            }
        }
        private void txtTra_cuu_HD_TextChanged(object sender, EventArgs e)
        {
            int k = 0;

            // Xóa bỏ danh sách hiện tại để sau này load lại sẽ tự cập nhật lại.

            listView4.Items.Clear();

            foreach (var hoadon in listHD)
            {
                // Ngay bat dau <= Ngay lap hoa don <= Ngay ket thuc
                if (hoadon.MaHD.ToString().ToLower().Contains(txtTra_cuu_HD.Text.ToLower())
                 || hoadon.NhanVien.TenNV.ToLower().Contains(txtTra_cuu_HD.Text.ToLower())
                    )     
                {
                    int indexHD = k + 1;
                    listView4.Items.Add(indexHD.ToString());
                    listView4.Items[k].SubItems.Add("HD" + hoadon.MaHD.ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.Tongsotien).ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.TienKhachTra).ToString());
                    listView4.Items[k].SubItems.Add(Convert.ToDouble(hoadon.TienThoi).ToString());
                    listView4.Items[k].SubItems.Add(hoadon.NgayLap.ToString());
                    listView4.Items[k].SubItems.Add(hoadon.NhanVien.TenNV);
                    k++;
                }
            }
        }

        private void txtTra_cuu_Nl_TextChanged(object sender, EventArgs e)
        {
            int j = 0;
            listView3.Items.Clear();

            //Xuất dữ liệu sản phẩm lên ListView3
            foreach (var nguyenlieu in listNL)
            {
                if ( nguyenlieu.TenNguyenLieu.ToLower().Contains(txtTra_cuu_Nl.Text.ToLower())
                   || nguyenlieu.MaNguyenLieu.ToString().ToLower().Contains(txtTra_cuu_Nl.Text.ToLower())
                   )
                {
                    int indexNL = j + 1;// Số thứ tự trên thực tế bắt đầu từ 1 nên j+1
                    listView3.Items.Add(indexNL.ToString());
                    listView3.Items[j].SubItems.Add("NL" + nguyenlieu.MaNguyenLieu.ToString());// Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                    listView3.Items[j].SubItems.Add(nguyenlieu.TenNguyenLieu);                 // mới là j
                    listView3.Items[j].SubItems.Add(nguyenlieu.SoLuong.ToString());
                    listView3.Items[j].SubItems.Add(Convert.ToDouble(nguyenlieu.Gia).ToString());
                    listView3.Items[j].SubItems.Add(nguyenlieu.NgayNhap.Value.ToString("d"));
                    listView3.Items[j].SubItems.Add(nguyenlieu.LoaiNguyenLieu.TenLoaiNguyenLieu);
                    listView3.Items[j].SubItems.Add(nguyenlieu.NhaCungCap.TenNCC);
                    listView3.Items[j].SubItems.Add(nguyenlieu.TrangThai);
                    j++;
                }    
                    

            }
        }

        private void txtTra_cuu_SP_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            listView2.Items.Clear();
            //Xuất dữ liệu sản phẩm lên ListView2
            foreach (var sanpham in listSP)
            {
                if (sanpham.TenSP.ToLower().Contains(txtTra_cuu_SP.Text.ToLower())
                   || sanpham.MaSP.ToString().ToLower().Contains(txtTra_cuu_SP.Text.ToLower())
                   )
                {
                    int indexSP = i + 1;// Số thứ tự trên thực tế bắt đầu từ 1 nên i+1
                    listView2.Items.Add(indexSP.ToString());
                    listView2.Items[i].SubItems.Add("SP" + sanpham.MaSP.ToString()); // Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                    listView2.Items[i].SubItems.Add(sanpham.TenSP);                   // mới là i
                    listView2.Items[i].SubItems.Add(sanpham.Size.ToString());
                    listView2.Items[i].SubItems.Add(Convert.ToDouble(sanpham.GiaGoc).ToString());
                    listView2.Items[i].SubItems.Add(sanpham.Soluong.ToString());
                    listView2.Items[i].SubItems.Add(sanpham.NguyenLieu.TenNguyenLieu);
                    listView2.Items[i].SubItems.Add(sanpham.Trangthai);

                    i++;
                }
            }
        }

        private void txtTra_Cua_NV_TextChanged(object sender, EventArgs e)
        {
            int h = 0;
            listView1.Items.Clear();
            //Xuất dữ liệu nhân viên lên ListView1
            foreach (var nhanvien in listNV)
            {
                if (nhanvien.TenNV.ToLower().Contains(txtTra_Cua_NV.Text.ToLower())
                   || nhanvien.MaNV.ToString().ToLower().Contains(txtTra_Cua_NV.Text.ToLower())
                   )
                {
                    int indexNV = h + 1; // Số thứ tự trên thực tế bắt đầu từ 1 nên h+1
                    listView1.Items.Add(indexNV.ToString());
                    listView1.Items[h].SubItems.Add("NV" + nhanvien.MaNV.ToString()); // Dữ liệu hệ thống có thứ tự từ 0 nên ở đây 
                    listView1.Items[h].SubItems.Add(nhanvien.TenNV);                  // mới là h
                    listView1.Items[h].SubItems.Add(nhanvien.SDT);
                    listView1.Items[h].SubItems.Add(nhanvien.NgaySinh.Value.ToString("d"));
                    listView1.Items[h].SubItems.Add(nhanvien.Email);
                    listView1.Items[h].SubItems.Add(nhanvien.GioiTinh);
                    listView1.Items[h].SubItems.Add(nhanvien.CaLamViec.TenCaLamViec);

                    // Lương = lương cơ bản (từ NV) * hệ số lương (từ Chức vụ)
                    double luong = Convert.ToDouble(nhanvien.Luongcoban) * Convert.ToDouble(nhanvien.ChucVu.Hesoluong);
                    listView1.Items[h].SubItems.Add(luong.ToString());
                    listView1.Items[h].SubItems.Add(nhanvien.ChucVu.TenCV);
                    listView1.Items[h].SubItems.Add(nhanvien.NgayGiaNhap.Value.Date.ToString());
                    listView1.Items[h].SubItems.Add(nhanvien.Matkhau);
                    listView1.Items[h].SubItems.Add(nhanvien.CMND);
                    listView1.Items[h].SubItems.Add(nhanvien.Trangthai);

                    h++; // tăng stt sau khi thêm xong nhũng dữ liệu ở trên
                }

            }
        }

        private void txtTra_cuu_NCC_TextChanged(object sender, EventArgs e)
        {
            int j = 0;
            listView5.Items.Clear();

            //Xuất dữ liệu sản phẩm lên ListView3
            foreach (var ncc in listNCC)
            {
                if (ncc.TenNCC.ToLower().Contains(txtTracua_NCC.Text.ToLower())
                   || ncc.MaNCC.ToString().ToLower().Contains(txtTracua_NCC.Text.ToLower())
                   )
                {
                    int indexNL = j + 1;// Số thứ tự trên thực tế bắt đầu từ 1 nên j+1
                    listView5.Items.Add(indexNL.ToString());
                    listView5.Items[j].SubItems.Add(ncc.MaNCC);// Dữ liệu hệ thống có thứ tự từ 0 nên ở đây
                    listView5.Items[j].SubItems.Add(ncc.TenNCC);                 // mới là j
                    listView5.Items[j].SubItems.Add(ncc.SDT);
                    listView5.Items[j].SubItems.Add(ncc.Email); 
                    listView5.Items[j].SubItems.Add(ncc.DiaChi);
                    j++;
                }


            }
        }

    }
}