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
    public partial class frmChucNang_NhanVien : Form
    {
        public frmChucNang_NhanVien()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void frmChucNang_NhanVien_Load(object sender, EventArgs e)
        {

        }

        private void txt_tennv_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbo_tencuahang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_nam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2CustomRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dtp_ngaysinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_sodienthoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbo_chucvu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbo_loainhanvien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_matkhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbo_trangthai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            frmChucNang_HopDongLuong chucNang = new frmChucNang_HopDongLuong(); 
            chucNang.Show();
            this.Close();
        }
    }
}
