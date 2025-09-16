using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Bill
    {
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public int ConsultationId { get; set; }
        public DateTime BillDate { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal LabFee { get; set; }
        public decimal MedicineFee { get; set; }
        public decimal TotalAmount => (decimal)(ConsultationFee + LabFee + MedicineFee);

        // Navigation properties
        public Patient Patient { get; set; }
        public Consultation Consultation { get; set; }
        public List<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
    }
}
