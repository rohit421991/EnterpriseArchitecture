using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserAccount
{
    public class UserRoleModel
    {
        public long UserRolesId { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RoleModel
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
