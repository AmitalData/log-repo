using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TruckerSetting
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }

        [Column("Tenant")]
        public int Tenant { get; set; }

        [Column("SearchFields")]
        public string SearchFields { get; set; }

        [ForeignKey("Address")]
        [Column("AddressId")]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }

        [ForeignKey("FromCity")]
        [Column("FromAddressCityId")]
        public string FromAddressCityId { get; set; }

        public virtual CountryCity FromCity { get; set; }

        [ForeignKey("ToCity")]
        [Column("ToAddressCityId")]
        public string ToAddressCityId { get; set; }

        public virtual CountryCity ToCity { get; set; }

        [ForeignKey("ShipmentTypeNavigation")]
        [Column("ShipmentType")]
        public string ShipmentType { get; set; }

        public virtual ShipmentType ShipmentTypeNavigation { get; set; }

        [ForeignKey("Trucker")]
        [Column("TruckerId")]
        public string TruckerId { get; set; }

        public virtual Trucker Trucker { get; set; }

        [ForeignKey("ResponsibilityEntity")]
        [Column("Responsibility")]
        public string Responsibility { get; set; }

        public virtual Responsibility ResponsibilityEntity { get; set; }


    }
}
