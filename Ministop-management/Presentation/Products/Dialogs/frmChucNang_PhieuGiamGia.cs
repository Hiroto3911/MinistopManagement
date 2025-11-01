using CrystalDecisions.CrystalReports.ViewerObjectModel;
using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace Presentation
{
    public partial class frmChucNang_PhieuGiamGia : Form
    {
        public event EventHandler DataChanged;
        private readonly IPromotionService _promotionService;
        private readonly IUnityContainer _container;
        private string _promotionId;
        public frmChucNang_PhieuGiamGia(IPromotionService promotionService , IUnityContainer container, string promotionId=null)
        {
            InitializeComponent();
            _promotionService = promotionService;
            _container = container;
            _promotionId = promotionId;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            int douutien =(int)udMucDoUuTien.Value;
            bool trangThai = Convert.ToInt32(cboTrangThai.SelectedValue) == 0;
            if (trangThai)
            {
                cboTrangThai.SelectedItem = "Tạm ngưng hoạt động";
            }
            else
            {
                cboTrangThai.SelectedItem = "Đang hoạt động";
            }
            var promotion = new PromotionDto() { PromotionId = txtMaGG.Text, PromotionName = txtTenGG.Text, StartDate = DateTime.Parse(dtpNgayBD.Text), EndDate = DateTime.Parse(dtpNgayKT.Text), Priority = douutien,Status = trangThai};
            Result<bool> result;
            if (string.IsNullOrEmpty(txtMaGG.Text))
            {
                result = _promotionService.CreatePromotion(promotion);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                promotion.PromotionId = _promotionId;
                result = _promotionService.UpdatePromotion(promotion);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            if (result.Succeeded == false)
            {
                MessageBox.Show($"{result.Message}", "Lỗi");
                return;
            }

            MessageBox.Show($"Luu thanh cong", "Thong bao");
            this.Close();
        }

        private void frmChucNang_PhieuGiamGia_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Add("Đang hoạt động");
            cboTrangThai.Items.Add("Tạm ngưng hoạt động");
            if (!string.IsNullOrEmpty(_promotionId))
            {
                var entity = _promotionService.GetPromotionByID(_promotionId);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtMaGG.Text = entity.Data.PromotionId.ToString();
                txtTenGG.Text = entity.Data.PromotionName;
                dtpNgayBD.Value = entity.Data.StartDate;
                dtpNgayKT.Value = entity.Data.EndDate;
                udMucDoUuTien.Text = entity.Data.Priority.ToString();
                cboTrangThai.SelectedValue = entity.Data.Status;
            }
        }
    }
}
