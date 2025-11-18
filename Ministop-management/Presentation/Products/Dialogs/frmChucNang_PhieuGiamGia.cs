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
        public frmChucNang_PhieuGiamGia(IPromotionService promotionService, IUnityContainer container, string promotionId = null)
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
            if (!KiemTraDuLieuNhap(out PromotionDto promotion))
                return; // Dừng nếu dữ liệu không hợp lệ

            Result<bool> result;

            if (string.IsNullOrEmpty(txtMaGG.Text))
            {
                result = _promotionService.CreatePromotion(promotion);
            }
            else
            {
                promotion.PromotionId = _promotionId;
                result = _promotionService.UpdatePromotion(promotion);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DataChanged?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
        private bool KiemTraDuLieuNhap(out PromotionDto promotion)
        {
            promotion = null;

            string maGG = txtMaGG.Text.Trim();
            string tenGG = txtTenGG.Text.Trim();
            DateTime ngayBD = dtpNgayBD.Value;
            DateTime ngayKT = dtpNgayKT.Value;
            int mucDoUuTien = (int)udMucDoUuTien.Value;
            if (cboTrangThai.SelectedIndex < 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
            bool trangThai = Convert.ToInt32(cboTrangThai.SelectedValue) == 1;
            if (string.IsNullOrEmpty(tenGG))
            {
                MessageBox.Show("Tên chương trình giảm giá không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenGG.Focus();
                return false;
            }
            if (ngayBD > ngayKT)
            {
                MessageBox.Show("Ngày bắt đầu không được sau ngày kết thúc.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayBD.Focus();
                return false;
            }
            if (ngayKT < ngayBD)
            {
                MessageBox.Show("Ngày kết thúc không thể trước ngày bắt đầu.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayKT.Focus();
                return false;
            }
            if (udMucDoUuTien.Value < 0)
            {
                MessageBox.Show("Mức độ ưu tiên phải lớn hơn 0.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                udMucDoUuTien.Focus();
                return false;
            }
            promotion = new PromotionDto()
            {
                PromotionId = maGG,
                PromotionName = tenGG,
                StartDate = ngayBD,
                EndDate = ngayKT,
                Priority = mucDoUuTien,
                Status = trangThai
            };

            return true;
        }
        private void frmChucNang_PhieuGiamGia_Load(object sender, EventArgs e)
        {
            // Setup ComboBox
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;

            var statusList = new Dictionary<string, int>()
    {
        { "Đang hoạt động", 1 },
        { "Tạm ngưng hoạt động", 0 }
    };

            cboTrangThai.DataSource = statusList.ToList();
            cboTrangThai.DisplayMember = "Key";
            cboTrangThai.ValueMember = "Value";

            // Mặc định chọn item đầu tiên
            cboTrangThai.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(_promotionId))
            {
                var entity = _promotionService.GetPromotionByID(_promotionId);
                if (entity.Succeeded == false || entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtMaGG.Text = entity.Data.PromotionId.ToString();
                txtTenGG.Text = entity.Data.PromotionName;
                dtpNgayBD.Value = entity.Data.StartDate;
                dtpNgayKT.Value = entity.Data.EndDate;
                udMucDoUuTien.Text = entity.Data.Priority.ToString();

                // Set trạng thái theo giá trị từ DB
                cboTrangThai.SelectedValue = entity.Data.Status ? 1 : 0;
            }
        }
    }
}
