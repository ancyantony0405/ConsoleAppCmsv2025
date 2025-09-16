using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Consultation
    {
        public int ConsultationId { get; set; }
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public DateTime CreatedOn { get; set; }

        // Navigation properties
        public Appointment Appointment { get; set; }
        public List<LabPrescription> LabPrescriptions { get; set; } = new List<LabPrescription>();
        public List<PharmacyPrescription> PharmacyPrescriptions { get; set; } = new List<PharmacyPrescription>();
        public Bill Bill { get; set; }
    }
}
