using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
    public class TariffSearchArgs
    {
        [Key]
        public string OriginPortId { get; set; }
        public string DestinationPortId { get; set; }
        public string ViaPortId { get; set; }
        public string WeightCode { get; set; }
        public string GrossWeightCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string CurrencyId { get; set; }
        public string TariffType { get; set; }
        public DateTime? BetweenDate { get; set; }
        public string Date { get; set; }
        public double Weight { get; set; }
        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public string ContainerType1Id { get; set; }
        public string ContainerType2Id { get; set; }
        public string ContainerType3Id { get; set; }
        public string ContainerType4Id { get; set; }
        public string ContainerType5Id { get; set; }
        public int? Quantity1 { get; set; }
        public int? Quantity2 { get; set; }
        public int? Quantity3 { get; set; }
        public int? Quantity4 { get; set; }
        public int? Quantity5 { get; set; }
        public string ProductId { get; set; }
    }
}
