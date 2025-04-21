using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Manager.EnterpriseDB
{
    public partial class UserToken
    {
        public long UserTokenId { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public bool? IsLoggedIn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public virtual AspNetUser? User { get; set; }
    }
}
