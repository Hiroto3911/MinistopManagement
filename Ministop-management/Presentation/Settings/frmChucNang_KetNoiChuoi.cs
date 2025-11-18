using Shared;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Settings
{
    public partial class frmChucNang_KetNoiChuoi : Form
    {
        public frmChucNang_KetNoiChuoi()
        {
            InitializeComponent();
        }

        private string BuildConnectionString()
        {
            if (cboAuthen.SelectedIndex == 0) // Windows Auth
            {
                return $"Data Source={txtServerName.Text};Initial Catalog={txtDatabaseName.Text};Integrated Security=True;";
            }
            else // SQL Login
            {
                return $"Data Source={txtServerName.Text};Initial Catalog={txtDatabaseName.Text};User ID={txtUserName.Text};Password={txtPassword.Text};";
            }
        }
        private void btnTestConn_Click(object sender, EventArgs e)
        {
            string conn = BuildConnectionString();
            try
            {
                using (var con = new SqlConnection(conn))
                {
                    con.Open();
                    MessageBox.Show("Kết nối thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string conn = BuildConnectionString();
            ConnectionConfigHelper.Save(new AppConfig { ConnectionString = conn });

            MessageBox.Show("Lưu cấu hình thành công.\nKhởi động lại ứng dụng!");
            this.Close();
        }

        private void frmChucNang_KetNoiChuoi_Load(object sender, EventArgs e)
        {
            cboAuthen.DataSource = new List<string>() { "Windows Authentication", "SQL Server Authentication" };
            txtDatabaseName.Text = "MinistopManagement";
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboAuthen_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboAuthen.SelectedItem == "Windows Authentication")
            {
                txtUserName.Enabled = false; txtPassword.Enabled = false;
            }
            else
            {
                txtUserName.Enabled = true; txtPassword.Enabled = true;
            }
        }
    }
}
