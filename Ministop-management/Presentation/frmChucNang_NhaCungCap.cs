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
    public partial class frmChucNang_NhaCungCap : Form
    {
        public frmChucNang_NhaCungCap()
        {
            InitializeComponent();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmChucNang_NhaCungCapSanPham chucNang = new frmChucNang_NhaCungCapSanPham();
            chucNang.Show();
            this.Close();
        }
    }
}
