using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class AWBSpecialHandlingCode
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsIATA { get; set; }
        public string AirlineId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }

        [ForeignKey("AirlineId")]
        public Card Airline { get; set; }
    }
}