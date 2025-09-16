using ConsoleAppCmsv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Service
{
    public class UserServiceImpl : IUserService
    {
        // Declare private variable for repository
        public readonly IUserRepository _userRepository;

        // DI - connstructor injection
        public UserServiceImpl(IUserRepository userrepository)
        {
            _userRepository = userrepository;
        }
        public Task<int> AuthenticateUserByRoleIdAsyn(string username, string password)
        {
            // checking validations for business rules
            return await _userRepository.AuthenticateUserByRoleIdAsyn(username, password);
        }
    }
}
