using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.ViewModel
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; } 
        public string Address { get; set; } 
        public string MMRNumber { get; set; }

        
        public int MembershipId { get; set; }   
        public string MembershipName { get; set; }

        public int BillId { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal LabFee { get; set; }
        public decimal MedicineFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
    }
}
