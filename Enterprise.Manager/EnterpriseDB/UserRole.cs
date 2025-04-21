using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Manager.EnterpriseDB
{
    public partial class UserRole
    {
        public long UserRoleId { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public bool IsActive { get; set; }

        public virtual AspNetRole? Role { get; set; }
        public virtual AspNetUser? User { get; set; }
    }
}
