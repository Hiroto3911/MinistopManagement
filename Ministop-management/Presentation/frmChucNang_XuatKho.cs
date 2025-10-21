using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_XuatKho : Form
    {
        public frmChucNang_XuatKho()
        {
            InitializeComponent();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmChucNang_ChiTietXuatKho chucNang = new frmChucNang_ChiTietXuatKho();
            chucNang.Show();
            this.Close();
        }
    }
}
