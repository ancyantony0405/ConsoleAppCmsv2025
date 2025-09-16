using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class PharmacyPrescription
    {
        public int PharmacyPresId { get; set; }
        public int PatientId { get; set; }
        public int ConsultationId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Consultation Consultation { get; set; }
        public List<PharmacyPrescriptionDetail> PharmacyPrescriptionDetails { get; set; } = new List<PharmacyPrescriptionDetail>();
    }
}
