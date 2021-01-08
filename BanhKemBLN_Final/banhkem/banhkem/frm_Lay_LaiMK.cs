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
    public partial class frm_Lay_LaiMK : Form
    {
        List<NhanVien> listNV;
        public frm_Lay_LaiMK( List <NhanVien> listNV)
        {
            InitializeComponent();
            this.listNV = listNV;
        }

        private void btnLayLaiMK_Click(object sender, EventArgs e)
        {
            foreach (NhanVien nv in listNV)
            {
                if (txtCMND.Text == nv.CMND && txtEmail.Text == nv.Email && txtSDT.Text == nv.SDT)
                {
                    MessageBox.Show("Mật khẩu của bạn là:" + nv.Matkhau);return;
                }

            }
            MessageBox.Show("Nhập sai thông tin tài khoản !");
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
