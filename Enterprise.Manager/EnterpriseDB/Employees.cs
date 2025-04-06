using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Manager.EnterpriseDB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EMPLOYEES")]
    public class Employees
    {
        [Key]
        public int Id { get; set; }

        [Column("FIRSTNAME")]
        public string? FirstName { get; set; }

        [Column("LASTNAME")]
        public string? LastName { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        [Column("MOBILE")]
        public string? Mobile { get; set; }

        [Column("ADDRESS")]
        public string? Address { get; set; }
    }

}
