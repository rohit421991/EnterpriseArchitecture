using DTO.UserAccount;
using Enterprise.Contract;
using Enterprise.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Enterprise.Manager.User
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRespository) 
        {
            _userAccountRepository = userAccountRespository;
        }
        public Task<bool> AddUserRole(UserRoleModel objUserRoleModel)
        {
            return _userAccountRepository.AddUserRole(objUserRoleModel);
        }

        public Task<RoleModel> GetRoleByRoleName(string RoleName)
        {
            return _userAccountRepository.GetRoleByRoleName(RoleName);

            //use mapper
            //return role == null ? null : new RoleModel
            //{
            //    RoleId = role.RoleId,
            //    RoleName = role.RoleName
            //};
        }
    }
}
