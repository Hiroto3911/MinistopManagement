using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class IdGenerator
    {
        public static string CreateID(string prefix)
        {
            var now = DateTime.UtcNow;
            string datePart = now.ToString("yyyyMMddHHmmss"); // chuẩn định dạng
            string randomPart = Guid.NewGuid().ToString("N").Substring(0,4); // 4 ký tự ngẫu nhiên
            return $"{prefix}{datePart}{randomPart}";
        }
    }
}
