using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorCode
{
    public enum ErrorCodeEnum
    {
        #region Common
        [Description(@"Thành công")]
        COM_SUC_000,
        [Description(@"Không xác định")]
        COM_ERR_000,
        [Description(@"xác thực thất bại")]
        COM_ERR_001,
        [Description(@"Bị trùng dữ liệu")]
        COM_ERR_002,
        [Description(@"Thiết lập thất bại")]
        COM_ERR_003,

        #endregion

        #region User
        [Description(@"Không tìm thấy người dùng.")]
        USE_ERR_001,
        [Description(@"Người dùng đã bị xóa.")]
        USE_ERR_002,
        [Description(@"Sai mật khẩu.")]
        USE_ERR_003,
        #endregion

        #region Store 
        [Description(@"Không tìm thấy cửa hàng.")]
        STR_ERR_001,
        [Description(@"Cửa hàng đã tồn tại.")]
        STR_ERR_002,
        [Description(@"Tạo cửa hàng thất bại.")]
        STR_ERR_003,
        [Description(@"Sửa cửa hàng thất bại.")]
        STR_ERR_004,
        [Description(@"Xóa cửa hàng thất bại.")]
        STR_ERR_005,
        [Description(@"Bị trùng dữ liệu cửa hàng.")]
        STR_ERR_006,
        #endregion

        #region Allowance
        [Description(@"Không tìm thấy phụ cấp.")]
        ALL_ERR_001,
        [Description(@"Phụ cấp đã tồn tại.")]
        ALL_ERR_002,
        [Description(@"Tạo phụ cấp thất bại.")]
        ALL_ERR_003,
        [Description(@"Sửa phụ cấp thất bại.")]
        ALL_ERR_004,
        [Description(@"Xóa phụ cấp thất bại.")]
        ALl_ERR_005,
        [Description(@"Bị trùng dữ liệu phụ cấp.")]
        ALL_ERR_006,
        #endregion

        #region Shift
        [Description(@"Không tìm thấy ca làm.")]
        SFT_ERR_001,
        [Description(@"Ca làm đã tồn tại.")]
        SFT_ERR_002,
        [Description(@"Tạo ca làm thất bại.")]
        SFT_ERR_003,
        [Description(@"Sửa ca làm thất bại.")]
        SFT_ERR_004,
        [Description(@"Xóa ca làm thất bại.")]
        SFT_ERR_005,
        [Description(@"Bị trùng dữ liệu ca làm.")]
        SFT_ERR_006,
        #endregion

        #region Employee
        [Description(@"Không tìm thấy nhân viên.")]
        EMP_ERR_001,
        [Description(@"nhân viên đã tồn tại.")]
        EMP_ERR_002,
        [Description(@"Tạo nhân viên thất bại.")]
        EMP_ERR_003,
        [Description(@"Sửa nhân viên thất bại.")]
        EMP_ERR_004,
        [Description(@"Xóa nhân viên thất bại.")]
        EMP_ERR_005,
        [Description(@"Bị trùng dữ liệu nhân viên.")]
        EMP_ERR_006,
        #endregion

        #region StoreFixedExpense 
        [Description(@"Không tìm thấy phi cửa hàng.")]
        SFE_ERR_001,
        [Description(@"Phiếu chi phí Cửa hàng đã tồn tại.")]
        SFE_ERR_002,
        [Description(@"Tạo Phiếu chi phí cửa hàng thất bại.")]
        SFE_ERR_003,
        [Description(@"Sửa Phiếu chi phí cửa hàng thất bại.")]
        SFE_ERR_004,
        [Description(@"Xóa Phiếu chi phí cửa hàng thất bại.")]
        SFE_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu chi phí cửa hàng.")]
        SFE_ERR_006,
        [Description(@"Phiếu chi phí cửa hàng chỉ được phép tạo bởi quản lý cửa hàng.")]
        SFE_ERR_007,
        [Description(@"Phiếu chi phí cửa hàng đã được tạo cho tháng này.")]
        SFE_ERR_008,
        #endregion

        #region ProductCategory 
        [Description(@"Không tìm thấy sản phẩm.")]
        PCT_ERR_001,
        [Description(@"Sản phẩm đã tồn tại.")]
        PCT_ERR_002,
        [Description(@"Tạo sản phẩm thất bại.")]
        PCT_ERR_003,
        [Description(@"Sửa sản phẩm thất bại.")]
        PCT_ERR_004,
        [Description(@"Xóa sản phẩm thất bại.")]
        PCT_ERR_005,
        [Description(@"Bị trùng dữ liệu sản phẩm.")]
        PCT_ERR_006,
        #endregion

        #region Product 
        [Description(@"Không tìm thấy sản phẩm.")]
        PRD_ERR_001,
        [Description(@"Sản phẩm đã tồn tại.")]
        PRD_ERR_002,
        [Description(@"Tạo sản phẩm thất bại.")]
        PRD_ERR_003,
        [Description(@"Sửa sản phẩm thất bại.")]
        PRD_ERR_004,
        [Description(@"Xóa sản phẩm thất bại.")]
        PRD_ERR_005,
        [Description(@"Bị trùng dữ liệu sản phẩm.")]
        PRD_ERR_006,
        #endregion

        #region Supplier
        [Description(@"Không tìm thấy nhà cung cấp.")]
        SLE_ERR_001,
        [Description(@"Nhà cung cấp đã tồn tại.")]
        SLE_ERR_002,
        [Description(@"Tạo nhà cung cấp thất bại.")]
        SLE_ERR_003,
        [Description(@"Sửa nhà cung cấp thất bại.")]
        SLE_ERR_004,
        [Description(@"Xóa nhà cung cấp thất bại.")]
        SLE_ERR_005,
        [Description(@"Bị trùng dữ liệu nhà cung cấp.")]
        SLE_ERR_006,
        #endregion

        #region StockDetail
        [Description(@"Không tìm thấy phiếu chi tiết kho .")]
        SDD_ERR_001,
        [Description(@"Phiếu chi tiết kho đã tồn tại.")]
        SDD_ERR_002,
        [Description(@"Tạo Phiếu chi tiết kho thất bại.")]
        SDD_ERR_003,
        [Description(@"Sửa Phiếu chi tiết kho thất bại.")]
        SDD_ERR_004,
        [Description(@"Xóa Phiếu chi tiết kho thất bại.")]
        SDD_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu chi tiết kho.")]
        SDD_ERR_006,
        [Description(@"số lượng của sản phẩm trong kho đã được thay đổi. Vui lòng cập nhập lại thông tin")]
        SDD_ERR_007,
        #endregion

        #region StockImport
        [Description(@"Không tìm thấy Phiếu nhập .")]
        SIT_ERR_001,
        [Description(@"Phiếu nhập đã tồn tại.")]
        SIT_ERR_002,
        [Description(@"Tạo Phiếu nhập thất bại.")]
        SIT_ERR_003,
        [Description(@"Sửa Phiếu nhập thất bại.")]
        SIT_ERR_004,
        [Description(@"Xóa Phiếu nhập thất bại.")]
        SIT_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu nhập.")]
        SIT_ERR_006,
        #endregion

        #region StockImportDetail
        [Description(@"Không tìm thấy Phiếu chi tiết nhập .")]
        SID_ERR_001,
        [Description(@"Phiếu chi tiết nhập đã tồn tại.")]
        SID_ERR_002,
        [Description(@"Tạo Phiếu chi tiết nhập thất bại.")]
        SID_ERR_003,
        [Description(@"Sửa Phiếu chi tiết nhập thất bại.")]
        SID_ERR_004,
        [Description(@"Xóa Phiếu chi tiết nhập thất bại.")]
        SID_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu chi tiết nhập.")]
        SID_ERR_006,
        #endregion

        #region StockExport
        [Description(@"Không tìm thấy Phiếu Xuất .")]
        SET_ERR_001,
        [Description(@"Phiếu Xuất đã tồn tại.")]
        SET_ERR_002,
        [Description(@"Tạo Phiếu Xuất thất bại.")]
        SET_ERR_003,
        [Description(@"Sửa Phiếu Xuất thất bại.")]
        SET_ERR_004,
        [Description(@"Xóa Phiếu Xuất thất bại.")]
        SET_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu Xuất.")]
        SET_ERR_006,
        #endregion

        #region StockExportDetail
        [Description(@"Không tìm thấy Phiếu chi tiết Xuất .")]
        SED_ERR_001,
        [Description(@"Phiếu chi tiết Xuất đã tồn tại.")]
        SED_ERR_002,
        [Description(@"Tạo Phiếu chi tiết Xuất thất bại.")]
        SED_ERR_003,
        [Description(@"Sửa Phiếu chi tiết Xuất thất bại.")]
        SED_ERR_004,
        [Description(@"Xóa Phiếu chi tiết Xuất thất bại.")]
        SED_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu chi tiết Xuất.")]
        SED_ERR_006,
        #endregion

        #region StockCheck
        [Description(@"Không tìm thấy Phiếu kiểm .")]
        SCT_ERR_001,
        [Description(@"Phiếu kiểm đã tồn tại.")]
        SCT_ERR_002,
        [Description(@"Tạo Phiếu kiểm thất bại.")]
        SCT_ERR_003,
        [Description(@"Sửa Phiếu kiểm thất bại.")]
        SCT_ERR_004,
        [Description(@"Xóa Phiếu kiểm thất bại.")]
        SCT_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu kiểm.")]
        SCT_ERR_006,
        #endregion

        #region StockCheckDetail
        [Description(@"Không tìm thấy Phiếu chi tiết kiểm .")]
        SCD_ERR_001,
        [Description(@"Phiếu chi tiết kiểm đã tồn tại.")]
        SCD_ERR_002,
        [Description(@"Tạo Phiếu chi tiết kiểm thất bại.")]
        SCD_ERR_003,
        [Description(@"Sửa Phiếu chi tiết kiểm thất bại.")]
        SCD_ERR_004,
        [Description(@"Xóa Phiếu chi tiết kiểm thất bại.")]
        SCD_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu chi tiết kiểm.")]
        SCD_ERR_006,
        #endregion

        #region SupplierProduct
        [Description(@"Không tìm thấy nhà cung cấp loại sản phẩm.")]
        SLPRD_ERR_001,
        [Description(@"Nhà cung cấp loại sản phẩm này đã tồn tại.")]
        SLPRD_ERR_002,
        [Description(@"Tạo nhà cung cấp sản phẩm thất bại.")]
        SLPRD_ERR_003,
        [Description(@"Sửa nhà cung cấp sản phẩm thất bại.")]
        SLPRD_ERR_004,
        [Description(@"Xóa nhà cung cấp sản phẩm thất bại.")]
        SLPRD_ERR_005,
        [Description(@"Bị trùng dữ liệu nhà cung cấp sản phẩm.")]
        SLPRD_ERR_006,
        #endregion

        #region Promotion
        [Description(@"Không tìm thấy phiếu giảm giá.")]
        PRM_ERR_001,
        [Description(@"Phiếu giảm giá đã tồn tại.")]
        PRM_ERR_002,
        [Description(@"Tạo phiếu giảm giá thất bại.")]
        PRM_ERR_003,
        [Description(@"Sửa phiếu giảm giá thất bại.")]
        PRM_ERR_004,
        [Description(@"Xóa phiếu giảm giá thất bại.")]
        PRM_ERR_005,
        [Description(@"Bị trùng dữ liệu phiếu giảm giá.")]
        PRM_ERR_006,
        #endregion

        #region PromotionProduct
        [Description(@"Không tìm thấy phiếu giảm giá sản phẩm.")]
        PRP_ERR_001,
        [Description(@"Phiếu giảm giá sản phẩm đã tồn tại.")]
        PRP_ERR_002,
        [Description(@"Tạo phiếu giảm giá sản phẩm thất bại.")]
        PRP_ERR_003,
        [Description(@"Sửa phiếu giảm giá sản phẩm thất bại.")]
        PRP_ERR_004,
        [Description(@"Xóa phiếu giảm giá sản phẩm thất bại.")]
        PRP_ERR_005,
        [Description(@"Bị trùng dữ liệu phiếu giảm giá sản phẩm.")]
        PRP_ERR_006,
        #endregion


        #region Salary Contract
        [Description(@"Không có hợp đồng lương.")]
        SAL_ERR_001,
        [Description(@"Hợp đồng lương đã tồn tại.")]
        SAL_ERR_002,
        [Description(@"Tạo hợp đồng lương thất bại.")]
        SAL_ERR_003,
        [Description(@"Sửa hợp đồng lương thất bại.")]
        SAL_ERR_004,
        [Description(@"Xóa hợp đồng lương thất bại.")]
        SAL_ERR_005,
        [Description(@"Bị trùng dữ liệu hợp đồng lương.")]
        SAL_ERR_006,
        #endregion

        #region ShiftAssignment
        [Description(@"Không tìm thấy phân công ca làm việc.")]
        SA_ERR_001,

        [Description(@"Phân công ca làm việc đã tồn tại.")]
        SA_ERR_002,

        [Description(@"Tạo phân công ca làm việc thất bại.")]
        SA_ERR_003,

        [Description(@"Sửa phân công ca làm việc thất bại.")]
        SA_ERR_004,

        [Description(@"Xóa mềm phân công ca làm việc thất bại.")]
        SA_ERR_005,

        [Description(@"Bị trùng dữ liệu phân công ca (Nhân viên + Ca + Ngày).")]
        SA_ERR_006,
        [Description(@"Định dạng tháng năm không hợp lệ (yyyy-MM)")]

        SA_ERR_007,
        #endregion

        #region Salary
        [Description(@"Không tìm thấy bảng lương.")]
        SLL_ERR_001,

        [Description(@"Bảng lương đã tồn tại.")]
        SLL_ERR_002,

        [Description(@"Tạo bảng lương thất bại.")]
        SLL_ERR_003,

        [Description(@"Sửa bảng lương thất bại.")]
        SLL_ERR_004,

        [Description(@"Xóa bảng lương thất bại.")]
        SLL_ERR_005,

        [Description(@"Bị trùng bảng lương (Hợp đồng + Tháng/Năm).")]
        SLL_ERR_006,
        #endregion

        #region SalaryContract_Allowance
        [Description(@"Không tìm thấy phụ cấp trong hợp đồng lương.")]
        SCA_ERR_001,

        [Description(@"ContractID không được để trống.")]
        SCA_ERR_002,

        [Description(@"Phụ cấp này đã tồn tại trong hợp đồng lương (trùng ContractID + AllowanceID).")]
        SCA_ERR_003,

        [Description(@"Hợp đồng lương không tồn tại.")]
        SCA_ERR_004,

        [Description(@"Loại phụ cấp không tồn tại.")]
        SCA_ERR_005,

        [Description(@"Thêm phụ cấp vào hợp đồng thất bại.")]
        SCA_ERR_006,

        [Description(@"Cập nhật phụ cấp trong hợp đồng thất bại.")]
        SCA_ERR_007,

        [Description(@"Xóa phụ cấp khỏi hợp đồng thất bại.")]
        SCA_ERR_008,
        #endregion

        #region Absence
        [Description(@"Không tìm thấy chấm công vắng.")]
        ABS_ERR_001,

        [Description(@"Chấm công vắng đã tồn tại.")]
        ABS_ERR_002,

        [Description(@"Tạo chấm công vắng thất bại.")]
        ABS_ERR_003,

        [Description(@"Sửa chấm công vắng thất bại.")]
        ABS_ERR_004,

        [Description(@"Xóa chấm công vắng thất bại.")]
        ABS_ERR_005,

        [Description(@"Bị trùng chấm công vắng.")]
        ABS_ERR_006,
        #endregion

        #region Invoice
        [Description(@"Không tìm thấy Hoá đơn.")]
        IVC_ERR_001,
        [Description(@"Hoá đơn đã tồn tại.")]
        IVC_ERR_002,
        [Description(@"Tạo Hoá đơn thất bại.")]
        IVC_ERR_003,
        [Description(@"Sửa Hoá đơn thất bại.")]
        IVC_ERR_004,
        [Description(@"Xóa Hoá đơn thất bại.")]
        IVC_ERR_005,
        [Description(@"Bị trùng dữ liệu Hoá đơn.")]
        IVC_ERR_006,
        #endregion
        #region InvoiceDetail
        [Description(@"Không tìm thấy Hoá đơn chi tiết .")]
        IVD_ERR_001,
        [Description(@"Hoá đơn chi tiết đã tồn tại.")]
        IVD_ERR_002,
        [Description(@"Tạo Hoá chi tiết đơn thất bại.")]
        IVD_ERR_003,
        [Description(@"Sửa Hoá đơn chi tiết thất bại.")]
        IVD_ERR_004,
        [Description(@"Xóa Hoá đơn chi tiết thất bại.")]
        IVD_ERR_005,
        [Description(@"Bị trùng dữ liệu Hoá đơn chi tiết.")]
        IVD_ERR_006,
        #endregion
        #region ReturnProduct
        [Description(@"Không tìm thấy Phiếu trả hàng.")]
        RTP_ERR_001,
        [Description(@"Phiếu trả hàng đã tồn tại.")]
        RTP_ERR_002,
        [Description(@"Tạo Phiếu trả hàng thất bại.")]
        RTP_ERR_003,
        [Description(@"Sửa Phiếu trả hàng thất bại.")]
        RTP_ERR_004,
        [Description(@"Xóa Phiếu trả hàng thất bại.")]
        RTP_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu trả hàng.")]
        RTP_ERR_006,
        #endregion
        #region ReturnProductDetail
        [Description(@"Không tìm thấy Phiếu trả hàng chi tiết .")]
        RTD_ERR_001,
        [Description(@"Phiếu trả hàng chi tiết đã tồn tại.")]
        RTD_ERR_002,
        [Description(@"Tạo Phiếu trả hàng chi tiết đơn thất bại.")]
        RTD_ERR_003,
        [Description(@"Sửa Phiếu trả hàng chi tiết thất bại.")]
        RTD_ERR_004,
        [Description(@"Xóa Phiếu trả hàng chi tiết thất bại.")]
        RTD_ERR_005,
        [Description(@"Bị trùng dữ liệu Phiếu trả hàng chi tiết.")]
        RTD_ERR_006,
        #endregion

    }
}
