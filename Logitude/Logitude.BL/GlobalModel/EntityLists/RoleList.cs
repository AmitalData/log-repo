using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class RoleList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RoleTypeCode { get; set; }
        public string Description { get; set; }
        public string ParentRoleId { get; set; }
        public bool IsCustomRole { get; set; }
        public string SearchFields { get; set; }
        public bool Inactive { get; set; }
    }
}
