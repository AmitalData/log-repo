using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class HTSCode
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual ProductItem  Item { get; set; }
        public string DestinationCountryId { get; set; }
        [ForeignKey("DestinationCountryId")]
        public virtual Country Country { get; set; }
        public string Code { get; set; }
        public bool ApprovedByCustomer { get; set; }
        public bool InActive { get; set; }

    }
}
