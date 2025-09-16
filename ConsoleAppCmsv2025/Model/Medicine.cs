using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public string Manufacturer { get; set; }

        // Navigation property
        public List<PharmacyPrescriptionDetail> PharmacyPrescriptionDetails { get; set; } = new List<PharmacyPrescriptionDetail>();
    }
}

