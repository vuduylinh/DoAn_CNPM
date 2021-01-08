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
            InitializeComponent();
            soluongSP = soluong;
        }

        public int soluongSP_Text
        {
            get { return soluongSP; }
            set { soluongSP = Convert.ToInt32(value); }
        }

        public int huychonSP
        {
            get { return huymon; }
            set { huymon = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            soluongSP = Convert.ToInt32(nUD_SoLuongSP.Value);
            this.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            huymon = 1;
            this.Close();
        }

        private void Frm_themSoluong_Mon_Load_1(object sender, EventArgs e)
        {
            nUD_SoLuongSP.Value = Convert.ToDecimal(soluongSP.ToString());
        }
    }
}
