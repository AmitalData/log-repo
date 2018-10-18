using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class PasswordPolicy
    {
        [Key]
        public string Code { get; set; }

        public string PasswordStrength { get; set; }
        public string SearchFields { get; set; }

        //public List<Tenant> Tenants { get; set; }
    }
}