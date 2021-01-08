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
    public partial class Frm_xemthongtinNV : Form
    {
        Model1 BanhkemDB;// Khai báo sẵn database + người dùng
        NhanVien user;
        public Frm_xemthongtinNV()
        {
            InitializeComponent();
        }

        public Frm_xemthongtinNV(Model1 banhkem, NhanVien a)
        {
            InitializeComponent();
            BanhkemDB = banhkem;
            user = a;
        }
        private void Frm_xemthongtinNV_Load(object sender, EventArgs e)
        {
            lblTenNV.Text = user.TenNV;
            lblNgaySinh.Text = user.NgaySinh.Value.ToString("dd/MM/yyyy");
            lblRole.Text = user.ChucVu.TenCV;
            lblSDT.Text = user.SDT;
            lblEmail.Text = user.Email;

        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDoiMk_Click(object sender, EventArgs e)
        {
            Frm_Doi_Mat_Khau f = new Frm_Doi_Mat_Khau(BanhkemDB, user); // Truyền dữ liệu database + nhân viên qua Form Main 
                                                             // để đỡ tốn bộ nhớ khi bên đó gọi lại
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                    Cursor.Current = Cursors.Arrow;
        }
    }
}
