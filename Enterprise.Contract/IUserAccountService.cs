using DTO.UserAccount;
using Enterprise.Data.Dtos;
using Enterprise.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Contract
{
    public interface IUserAccountService
    {
        Task<RoleModel> GetRoleByRoleName(string RoleName);
        Task<bool> AddUserRole(UserRoleModel objUserRoleModel);
    }
}
