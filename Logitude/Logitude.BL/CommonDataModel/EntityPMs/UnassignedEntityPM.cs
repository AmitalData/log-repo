using System;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class UnassignedEntityPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string UnassignedCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
