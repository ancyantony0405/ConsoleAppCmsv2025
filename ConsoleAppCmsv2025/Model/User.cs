using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public char Gender { get; set; } // 'M', 'F', 'O'
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime JoiningDate { get; set; }
        public int RoleId { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public Role Role { get; set; }
    }
}
