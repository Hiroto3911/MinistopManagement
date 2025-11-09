using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Stocks
{
    public partial class frmChucNang_CaiDat : Form
    {
        private readonly int _quantityWarming;

        public event EventHandler<int> dataChanged;
        public frmChucNang_CaiDat(int quantityWarming = 0 )
        {
            InitializeComponent();
            _quantityWarming = quantityWarming;
        }

        private void frmChucNang_CaiDat_Load(object sender, EventArgs e)
        {
            if(_quantityWarming < 0)
            {
                txtSLCanhBao.Text ="0";
            }
            else
            {
                txtSLCanhBao.Text =_quantityWarming.ToString();
            }
        }

        private void btnMacDinhCaiDat_Click(object sender, EventArgs e)
        {
            txtSLCanhBao.Text = "50";
        }

        private void btnLuuCaiDat_Click(object sender, EventArgs e)
        {
            if(!int.TryParse(txtSLCanhBao.Text, out int newQuantity))
            {
                MessageBox.Show("Số lượng cảnh báo  phải là số hợp lệ và không được để trống hoặc bằng 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSLCanhBao.Focus();
                return;
            }
            dataChanged.Invoke(sender, newQuantity);
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
