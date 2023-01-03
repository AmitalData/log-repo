using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DeploymentPackage
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SearchFields { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public string DirectionId { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual Direction Direction { get; set; }
        public string VersionId { get; set; }
        public virtual DeploymentPackagesVersion DeploymentPackagesVersion { get; set; }

    }
}