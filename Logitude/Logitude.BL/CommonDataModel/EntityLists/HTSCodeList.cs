
using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class HTSCodeList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ItemId { get; set; }
        public string DestinationCountryId { get; set; }
        public string Code { get; set; }
        public bool ApprovedByCustomer { get; set; }
        public bool InActive { get; set; }
        public int? LineNumber { get; set; }
    }
}
