using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class RoleType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<Role> Roles { get; set; }
    }
}