using Services.Interfaces;
using Services.Services;
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
    public partial class frmChucNang_ChiTietKiemKho : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStockCheckDetailService _stockCheckDetailService;
        public readonly IUserSession _userSession;
        public string _checkID;
        public frmChucNang_ChiTietKiemKho(IStockCheckDetailService stockCheckDetailService, IUserSession userSession, string CheckID = null)
        {
            InitializeComponent();
            _stockCheckDetailService = stockCheckDetailService;
            _userSession = userSession;
            _checkID = CheckID;
        }

        private void frmChucNang_ChiTietKiemKho_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_checkID))
            {
                var entity = _stockCheckDetailService.GetStockCheckDetailByID(_checkID);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }
    }
}
