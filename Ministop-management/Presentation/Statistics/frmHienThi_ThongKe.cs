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

namespace Presentation
{
    public partial class frmHienThi_ThongKe : Form
    {
        private readonly IUserSession _userSession;

        public frmHienThi_ThongKe(IUserSession userSession)
        {
            InitializeComponent();
            _userSession = userSession;
        }

        private void frmHienThi_ThongKe_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlTK.TabPages.Remove(tabTaiChinh);
            }
        }
    }
}
