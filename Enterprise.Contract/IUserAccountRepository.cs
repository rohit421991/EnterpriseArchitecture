using DTO.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Contract
{
    public interface IUserAccountRepository
    {
        Task<RoleModel> GetRoleByRoleName(string RoleName);
        Task<bool> AddUserRole(UserRoleModel objUserRoleModel);
    }
}
