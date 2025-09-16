using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Repository
{
    public interface IUserRepository
    {
        // Login functionality - passing user name and password
        Task<int> AuthenticateUserByRoleIdAsyn(string username, string password);
    }
}
