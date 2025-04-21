using DTO.UserAccount;
using Enterprise.Contract;
using Enterprise.Manager.EnterpriseDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Manager.User
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly EnterpriseDbContext _context;
        public UserAccountRepository(EnterpriseDbContext enterpriseDbContext)
        {
            _context = enterpriseDbContext;
        }
        public async Task<bool> AddUserRole(UserRoleModel objUserRoleModel)
        {
            bool status = false;
            try
            {
                // Check if already assigned
                var exists = await _context.UserRoles
                    .AnyAsync(ur => ur.UserId == objUserRoleModel.UserId && ur.RoleId == objUserRoleModel.RoleId);

                if (!exists)
                {
                    var userRole = new IdentityUserRole<string>
                    {
                        UserId = objUserRoleModel.UserId,
                        RoleId = objUserRoleModel.RoleId
                    };

                    _context.UserRoles.Add(userRole);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }

            return status;
        }
        public Task<RoleModel> GetRoleByRoleName(string RoleName)
        {
            try
            {
                var data = _context.Roles.Where(u => u.Name.Trim().Equals(RoleName.Trim()))
                     .Select(x => new RoleModel { RoleId = x.Id, RoleName = x.Name }).FirstOrDefaultAsync();

                return data;
            }
            catch (Exception) { throw; }
        }
    }
}
