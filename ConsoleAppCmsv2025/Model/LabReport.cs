using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class LabReport
    {
        public int ReportId { get; set; }
        public int LabDetailId { get; set; }
        public string Result { get; set; }
        public DateTime ReportDate { get; set; }

        // Navigation property
        public LabPrescriptionDetail LabPrescriptionDetail { get; set; }
    }
}
