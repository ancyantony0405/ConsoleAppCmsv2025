using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.ViewModel
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int TokenNo { get; set; }
        public string MMRNumber { get; set; }
        public string AppointmentTime => AppointmentDate.ToString("HH:mm");

    }
}
