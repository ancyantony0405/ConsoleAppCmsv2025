using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Test
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }
        public decimal TestFee { get; set; }

        // Navigation property
        public List<LabPrescriptionDetail> LabPrescriptionDetails { get; set; } = new List<LabPrescriptionDetail>();
    }
}
