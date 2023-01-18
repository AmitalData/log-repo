using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ReferenceCustomObjectPM : ChildEntitiesCustomFieldPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool InActive { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public string SearchFields { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }

    }
}
