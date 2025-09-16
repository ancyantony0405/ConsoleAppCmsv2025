using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public string MembershipName { get; set; }
        public decimal DiscountPercent { get; set; }
        public int DurationMonths { get; set; }
        public decimal Fee { get; set; }

        // Navigation property
        public List<PatientMembership> PatientMemberships { get; set; } = new List<PatientMembership>();
    }
}
