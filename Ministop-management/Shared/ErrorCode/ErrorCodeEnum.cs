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
        [Description(@"Không tìm thấy loại sản phẩm.")]
        PRD_ERR_001,
        [Description(@"Loại sản phẩm đã tồn tại.")]
        PRD_ERR_002,
        [Description(@"Tạo loại sản phẩm thất bại.")]
        PRD_ERR_003,
        [Description(@"Sửa loại sản phẩm thất bại.")]
        PRD_ERR_004,
        [Description(@"Xóa loại sản phẩm thất bại.")]
        PRD_ERR_005,
        [Description(@"Bị trùng dữ liệu loại sản phẩm.")]
        PRD_ERR_006,
        #endregion

        #region Supplier
        [Description(@"Không nhà cung cấp loại sản phẩm.")]
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
    }
}
