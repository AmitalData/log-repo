using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class CustomChildObjectPM: ChildEntitiesCustomFieldPM
    {

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ParentEntityId { get; set; }
        public string ParentObjectTableId { get; set; }
        public string ObjectTableId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }


    }

}
