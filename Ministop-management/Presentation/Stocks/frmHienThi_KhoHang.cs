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
    public partial class frmHienThi_KhoHang : Form
    {
        private readonly IUserSession _userSession;

        public frmHienThi_KhoHang(IUserSession userSession)
        {
            InitializeComponent();
            _userSession = userSession;
            
        }
        private void frmHienThi_KhoHang_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Nhân viên")
            {
                tabControlKH.TabPages.Remove(tabChiTietKho);
                tabControlKH.TabPages.Remove(tabNhapHang);
            }
        }
        #region StockDetail 
        #endregion
        #region StockImport
        #endregion
        #region StockExport 
        #endregion
        #region StockCheck
        #endregion
    }
}
