using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class PatientMembership
    {
        public int PatientMembershipId { get; set; }
        public int PatientId { get; set; }
        public int MembershipId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Membership Membership { get; set; }
    }

}
