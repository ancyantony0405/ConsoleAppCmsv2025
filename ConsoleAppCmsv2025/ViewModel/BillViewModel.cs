using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.ViewModel
{
    public class BillViewModel
    {
        public int BillId { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetPayable { get; set; }
    }
}
