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

            var result = _identityServices.Authentication(userId, matKhau);

            if (!result)
            {
                MessageBox.Show("Đăng nhập thất bại vui lòng kiểm tra lại mật khẩu hoặc mã nhân viên !", "Thông báo");
                return;
            }
            var frmMain = _container.Resolve<frmMain>();
            this.Hide();
            frmMain.ShowDialog();
            this.Close();
        }
    }
}
