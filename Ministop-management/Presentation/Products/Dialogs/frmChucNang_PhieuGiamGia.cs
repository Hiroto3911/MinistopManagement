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
    public partial class frmChucNang_PhieuGiamGia : Form
    {
        public frmChucNang_PhieuGiamGia()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            frmChucNang_GiamGiaSP chucNang = new frmChucNang_GiamGiaSP();
            chucNang.Show();
            this.Close();
        }
    }
}
