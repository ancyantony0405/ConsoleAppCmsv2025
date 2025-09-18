using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.ViewModel
{
    public class MedicineViewModel
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
