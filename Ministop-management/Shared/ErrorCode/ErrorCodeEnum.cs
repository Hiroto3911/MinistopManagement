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
        #region SupplierProduct
        [Description(@"Không nhà cung cấp loại sản phẩm.")]
        SLPRD_ERR_001,
        [Description(@"Nhà cung cấp sản phẩm đã tồn tại.")]
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
    }
}
