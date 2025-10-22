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
    }
}
