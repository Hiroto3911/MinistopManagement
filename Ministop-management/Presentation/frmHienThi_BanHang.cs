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
    public partial class frmHienThi_BanHang : Form
    {
        public frmHienThi_BanHang()
        {
            InitializeComponent();
        }


     

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            frmChucNang_ChiTietHoaDon child = new frmChucNang_ChiTietHoaDon();
            child.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frmChucNang_PhieuTraHang child = new frmChucNang_PhieuTraHang();
            child.Show();
        }
    }
}
