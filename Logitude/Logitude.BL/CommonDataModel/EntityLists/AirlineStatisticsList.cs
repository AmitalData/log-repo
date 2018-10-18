using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AirlineStatisticsList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int SourceTenant { get; set; }
        public string SourceTenantName { get; set; }
        public string ShipmentId { get; set; }//(no relation)
        public string ShipmentLevelCode { get; set; }//(relation)
        public string BookingId { get; set; }//(no relation)
        public string EntityReference { get; set; }
        public string AWBNumber { get; set; }
        public string HWBNumber { get; set; }
        public string AirlineCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime EntitiyCreateDate { get; set; }
        public DateTime EntitiyUpdateDate { get; set; }
        public string EntityCreatedByUserName { get; set; }
        public string MessageType { get; set; }
        public DateTime? LastSentDate { get; set; }
        public string EntityStatus { get; set; }
        public int? NumberOfPackages { get; set; }
        public decimal? ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public decimal? GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public decimal? Volume { get; set; }
        public string VolumeUnitCode { get; set; }
        public string OriginCode { get; set; }
        public string DestinationCode { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string Flight1 { get; set; }
        public DateTime? Flight1Date { get; set; }//(ATD if empty then ETD)
        public string Flight2 { get; set; }
        public DateTime? Flight2Date { get; set; }
        public string Flight3 { get; set; }
        public DateTime? Flight3Date { get; set; }
        public string OnCarriageTo { get; set; }
        public DateTime? OnCarriageDate { get; set; }
        public string PreCarriageFrom { get; set; }
        public DateTime? PreCarriageDate { get; set; }
        public string Allotment { get; set; }
        public string SearchFields { get; set; }
    }
}
