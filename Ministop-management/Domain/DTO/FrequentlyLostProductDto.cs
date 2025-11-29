using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class FrequentlyLostProductDto
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int TimesLost { get; set; }
    }
}
