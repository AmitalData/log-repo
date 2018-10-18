using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Role
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

        [ForeignKey("RoleTypeCode")]
        public RoleType RoleType { get; set; }
    }
}