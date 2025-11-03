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
    public partial class frmChucNang_ChiTietNhapKho : Form
    {
        public event EventHandler dataChanged;
        public frmChucNang_ChiTietNhapKho()
        {
            InitializeComponent();
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChucNang_ChiTietNhapKho_Load(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }
    }
}
