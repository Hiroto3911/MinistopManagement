using Guna.UI2.WinForms;
using Services.Interfaces;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Unity;

namespace Presentation
{
    public partial class frmHienThi_ThongKe : Form
    {
        private readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        public frmHienThi_ThongKe(IUserSession userSession, IUnityContainer container)
        {
            InitializeComponent();
            _userSession = userSession;
            _container = container;
        }

        private void frmHienThi_ThongKe_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlTK.TabPages.Remove(tabTaiChinh);
            }
            cboThang.DataSource = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            LoadCboCuaHang(cboCuaHangDT);
            LoadCboCuaHang(cboCuaHangTK);
        }
        private void LoadCboCuaHang(Guna2ComboBox cboCuaHang)
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeServices = childContainer.Resolve<IStoreService>();
                var list = storeServices.GetAll();
                if (list.Succeeded == false && list.Data == null) { return; }
                cboCuaHang.DataSource = list.Data;
                cboCuaHang.ValueMember = "StoreID";
                cboCuaHang.DisplayMember = "StoreName";
            }

        }

        private void btnXemTK_Click(object sender, EventArgs e)
        {

        }
    }
}
