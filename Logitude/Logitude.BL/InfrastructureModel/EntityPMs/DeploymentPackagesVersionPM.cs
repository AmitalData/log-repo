using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DeploymentPackagesVersionPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string DeploymentPackageID { get; set; }
        public bool IsExported { get; set; }
        public string DocumentId { get; set; }
        public string VersionName { get; set; }
        public int VersionNumber { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

    }

}
