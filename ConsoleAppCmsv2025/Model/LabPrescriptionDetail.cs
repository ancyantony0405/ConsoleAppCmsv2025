using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class LabPrescriptionDetail
    {
        public int LabDetailId { get; set; }
        public int LabPrescriptionId { get; set; }
        public string TestName { get; set; }
        public decimal TestFee { get; set; }
        public int TestId { get; set; }

        // Navigation properties
        public LabPrescription LabPrescription { get; set; }
        public Test Test { get; set; }
        
    }
}
