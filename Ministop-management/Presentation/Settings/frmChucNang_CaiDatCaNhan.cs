using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Settings
{
    public partial class frmChucNang_CaiDatCaNhan : Form
    {
        private Dictionary<string, string> languages = new Dictionary<string, string>()
        {
            {"VN","vi-VN"},
            {"EN","en-US" }
        };
        public frmChucNang_CaiDatCaNhan()
        {
            InitializeComponent();
        }

        private void btnLuuCaiDat_Click(object sender, EventArgs e)
        {
           string selectedLang = cboLanguage.SelectedValue.ToString();
           var langUsedNow = Properties.Settings.Default.Language;
            if ((selectedLang == langUsedNow ))
            {
                MessageBox.Show("Vui lòng chọn ngôn ngữ khác vì ngôn ngữ hiện tại đang chính là ngôn ngữ bạn đang chọn!");
                return ;     

            }
            Properties.Settings.Default.Language = selectedLang;
            Properties.Settings.Default.Save();
            Application.Restart();  

           
        }

        private void btnMacDinhCaiDat_Click(object sender, EventArgs e)
        {

        }

        private void frmChucNang_CaiDatCaNhan_Load(object sender, EventArgs e)
        {
            cboLanguage.Items.Clear();
            cboLanguage.DataSource = languages.ToList();
            cboLanguage.DisplayMember = "Key";
            cboLanguage.ValueMember = "Value";

        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
