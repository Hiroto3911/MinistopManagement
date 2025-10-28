using Services.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Unity;

namespace Presentation
{
    public partial class frmDangNhap : Form
    {
        private readonly IIdentityService _identityServices;
        private readonly IUnityContainer _container;

        public frmDangNhap(IIdentityService identityServices, IUnityContainer container)
        {
            InitializeComponent();
            _identityServices = identityServices;
            _container = container;

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string userId = txtMaNhanVien.Text;
            string matKhau = txtMatKhau.Text;
            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("Mã nhân viên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            var result = _identityServices.Authentication(userId, matKhau);
            if (!result)
            {
                MessageBox.Show("Đăng nhập thất bại vui lòng kiểm tra lại mật khẩu hoặc mã nhân viên !", "Thông báo");
                return;
            }
            var frmMain = _container.Resolve<frmMain>();
            this.Hide();
            frmMain.ShowDialog();
            this.Show();
        }

        private void chkHienThiMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            var hienThiPass = txtMatKhau.UseSystemPasswordChar == true ? txtMatKhau.UseSystemPasswordChar = false : txtMatKhau.UseSystemPasswordChar = true;
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            //txtMaNhanVien.Clear();
            //txtMatKhau.Clear();
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMatKhau.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void frmDangNhap_Shown(object sender, EventArgs e)
        {
            txtMaNhanVien.Focus();
        }
    }
}
