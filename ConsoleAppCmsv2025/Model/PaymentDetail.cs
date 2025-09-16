using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class PaymentDetail
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } // Cash, Card, UPI, Insurance
        public decimal AmountPaid { get; set; }
        public string TransactionRef { get; set; }
        public bool IsSuccessful { get; set; }

        // Navigation property
        public Bill Bill { get; set; }
    }
}
