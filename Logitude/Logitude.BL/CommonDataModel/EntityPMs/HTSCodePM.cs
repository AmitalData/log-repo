using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class HTSCodePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ItemId { get; set; }
        public string DestinationCountryId { get; set; }
        public string Code { get; set; }
        public bool ApprovedByCustomer { get; set; }
        public bool InActive { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
