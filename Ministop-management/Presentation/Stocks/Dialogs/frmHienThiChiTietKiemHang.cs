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

namespace Presentation.Stocks.Dialogs
{
    public partial class frmHienThiChiTietKiemHang : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStockCheckDetailService _stockCheckDetailService;
        public readonly IUserSession _userSession;
        public string _expenseID;
        public frmHienThiChiTietKiemHang(IStockCheckDetailService stockCheckDetailService, IUserSession userSession, string expenseID = null)
        {
            InitializeComponent();
            _stockCheckDetailService = stockCheckDetailService;
            _userSession = userSession;
            _expenseID = expenseID;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThiChiTietKiemHang_Load(object sender, EventArgs e)
        {

        }
    }
}
