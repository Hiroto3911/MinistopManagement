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
    public partial class frmChucNang_KiemKho : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStoreFixedExpenseServices _storeFixedExpenseServices;
        public readonly IUserSession _userSession;
        public string _expenseID;
        public frmChucNang_KiemKho(IStoreFixedExpenseServices storeFixedExpenseServices, IUserSession userSession, string expenseID = null)
        {
            InitializeComponent();
            _storeFixedExpenseServices = storeFixedExpenseServices;
            _userSession = userSession;
            _expenseID = expenseID;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }

        private void frmChucNang_KiemKho_Load(object sender, EventArgs e)
        {

        }
    }
}
