using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public bool IsAvailable { get; set; }
        public decimal DoctorFee { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Department Department { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
