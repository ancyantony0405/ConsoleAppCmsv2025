using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class PharmacyPrescriptionDetail
    {
        public int PharmacyDetailId { get; set; }
        public int PharmacyPresId { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public int MedicineId { get; set; }

        // Navigation properties
        public PharmacyPrescription PharmacyPrescription { get; set; }
        public Medicine Medicine { get; set; }
    }
}
