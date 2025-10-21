using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Validations
{
    public static  class StoreValidator
    {
        public static (bool IsValid, string Message) ValidateInput(string name, string address, string phone)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tên cửa hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(address))
                return (false, "Địa chỉ không được để trống.");

            if (string.IsNullOrWhiteSpace(phone))
                return (false, "Số điện thoại không được để trống.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{9,11}$"))
                return (false, "Số điện thoại không hợp lệ.");

            return (true, string.Empty);
        }
    }
}
