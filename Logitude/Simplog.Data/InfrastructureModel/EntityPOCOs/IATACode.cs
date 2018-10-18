using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class IATACode
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DueTypeCode { get; set; }
        public string MeasurementCode { get; set; }
        public string SearchFields { get; set; }
        public bool IsIATA { get; set; }
        public bool InActive { get; set; }
        public string AirlineId { get; set; }

        [ForeignKey("DueTypeCode")]
        public DueType DueType { get; set; }

        [ForeignKey("AirlineId")]
        public Card Airline { get; set; }
    }
}