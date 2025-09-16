using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Service
{
    public interface IUserService
    {
        // Multithread -- asynchronous
        Task<int> AuthenticateUserByRoleIdAsyn(string username, string password);
    }
}
