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
    public partial class Frm_Main : Form
    {
        Model1 BanhkemDB; /* Khai báo sẵn database*/
        NhanVien user = new NhanVien(); //Khai báo sẵn nhân viên
        
       
        public Frm_Main(Model1 banhkem,NhanVien a) 
        {
            InitializeComponent();
            BanhkemDB = banhkem; // Nhận dữ liệu database từ Form Đăng nhập
            user = a; // Nhận dữ liệu nhân viên đăng nhập
        }

        //Nhân viên bán hàng thì chỉ có công việc là bán hàng, không có quyền truy cập vào chức năng quản lý
        //Nhân viên quản lý thì chỉ có công việc là quản lý, không được quyền tác động đến việc buôn bán
        //Admin được quyền truy cập vào bất kỳ chức năng nào. Tuy nhiên vì yêu cầu bảo mật, chỉ nên có 1 Admin duy nhất trong trường hợp khẩn cấp


        private void button3_Click(object sender, EventArgs e)
        {
            
                Frm_Banghang f = new Frm_Banghang(BanhkemDB,user);
                this.Hide();
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow; // con trỏ chuột hiện tại là hình mũi tên
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                Frm_QuanLy f = new Frm_QuanLy(BanhkemDB, user);
                this.Hide();
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
                Frm_ThongKe f = new Frm_ThongKe(BanhkemDB, user);
                this.Hide();
                f.ShowDialog();
                this.Show();
                Cursor.Current = Cursors.Arrow;
                
            
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {

            textBox1.Text = user.TenNV;//hiển thị tên nhân viên đăng nhập  

              if (user.ChucVu.MaCV == 3) // Ban hang
              {
                button1.Visible = false;
                button2.Visible = false;
                button3.Location = button1.Location;
            }
            if (user.ChucVu.MaCV == 2) // Quan ly
            {
                button1.Visible = false;
                button3.Visible = false;
                button2.Location=button1.Location;
            }

            if (user.ChucVu.MaCV == 4) // Ke toan
            {
                button2.Visible = false;
                button3.Visible = false;
               
            }


        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThongtinNV_Click(object sender, EventArgs e)
        {
            Frm_xemthongtinNV f = new Frm_xemthongtinNV(BanhkemDB,user);
            this.Hide();
            f.ShowDialog();
            this.Show();
            Cursor.Current = Cursors.Arrow;
        }
    }
}
