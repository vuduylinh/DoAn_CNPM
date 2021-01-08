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
    public partial class Frm_themSoluong_Mon : Form
    {
        int soluongSP;
        int huymon = 0;
        public Frm_themSoluong_Mon()
        {
            InitializeComponent();
        }

        public Frm_themSoluong_Mon(int soluong)
        {
            // Nhận số lượng của sản phẩm từ Form bán hàng
            InitializeComponent();
            soluongSP = soluong;
        }

        // Sau khi chọn số lượng sản phẩm ở sự kiện button1_Click ở dưới thì sẽ lưu biến số lượng sản phẩm ở dưới.
        public int soluongSP_Text
        {
            get { return soluongSP; }
            set { soluongSP = Convert.ToInt32(value); }
        }

        // Sau khi hủy chọn số lượng sản phẩm ở sự kiện button1_Click ở dưới thì sẽ lưu giá trị ở dưới.
        // Ở form banghang sẽ xem xét dựa vào giá trị này để thực hiện thao tác. 
        public int huychonSP
        {
            get { return huymon; }
            set { huymon = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Con số trong numbueric_up_down có kiểu là Decimal nên phải chuyển về int
            soluongSP = Convert.ToInt32(nUD_SoLuongSP.Value);
            this.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Nếu chọn hủy món thì sẽ lưu giá trị là 1
            huymon = 1;
            this.Close();
        }

        private void Frm_themSoluong_Mon_Load_1(object sender, EventArgs e)
        {
            // Hiện số lượng của sản phẩm đã chọn ở Form Banghang
            nUD_SoLuongSP.Value = Convert.ToDecimal(soluongSP.ToString());
        }
    }
}
