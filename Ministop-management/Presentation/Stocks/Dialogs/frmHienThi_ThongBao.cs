using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Unity;

namespace Presentation.Stocks.Dialogs
{
    public partial class frmHienThi_ThongBao : Form
    {
        private readonly IUnityContainer _container;
        private readonly string _storeID;
        private readonly DateTime _date;
        private readonly string _type;
        private int _totalPage;

        public frmHienThi_ThongBao(string storeID, DateTime date, string type, IUnityContainer container)
        {
            InitializeComponent();
            _container = container;
            _storeID = storeID;
            _date = date;
            _type = type;

            LoadData();
        }


        private void LoadData(int pageNumber = 1, int pageSize = 20)
        {
            lblTieuDe.Text = _type == "IMPORT"
    ? Properties.Resources.Title_ThongBaoNhap
    : Properties.Resources.Title_ThongBaoNhap;

            using (var scope = _container.CreateChildContainer())
            {
                if (_type == "IMPORT")
                {
                    var service = scope.Resolve<IStockImportDetailSerivce>();
                    var result = service.GetImportDetails(_storeID, _date.Month, _date.Year, pageNumber, pageSize);
                    dgvDuLieu.DataSource = result.Data;
                }
                else // EXPORT
                {
                    var service = scope.Resolve<IStockExportDetailService>();
                    var result = service.GetExportDetails(_storeID, _date.Month, _date.Year,pageNumber,pageSize);
                    dgvDuLieu.DataSource = result.Data;
                }
            }

            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;
            btnTrangTruoc.Enabled = pageNumber > 1;
            btnTrangSau.Enabled = pageNumber <= _totalPage;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            btnTrangTruoc.Enabled = true;
            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData( pageNumber);
            }
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(pageNumber);

            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }
    }
}
