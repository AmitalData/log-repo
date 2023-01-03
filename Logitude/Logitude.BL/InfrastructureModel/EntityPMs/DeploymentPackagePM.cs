using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DeploymentPackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DirectionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string VersionId { get; set; }
        public string DocumentId { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public DeploymentPackageDetails DeploymentPackageDetails { get; set; }
    }

    public class DeploymentPackageDetails
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ObjectFieldPM> CustomFields { get; set; }
    }
}
