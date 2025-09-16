using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string MMRNumber { get; set; }

        // Navigation properties
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<LabPrescription> LabPrescriptions { get; set; } = new List<LabPrescription>();
        public List<PharmacyPrescription> PharmacyPrescriptions { get; set; } = new List<PharmacyPrescription>();
        public List<PatientMembership> PatientMemberships { get; set; } = new List<PatientMembership>();
        public List<Bill> Bills { get; set; } = new List<Bill>();
    }
}
