using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class AirlineAreasPort
    {
        [Key]
        public string Id { get; set; }
        public string AirlineAreaId { get; set; }
        public string PortId { get; set; }

        public int Tenant { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? AddedDate { get; set; }       
        public string AddedByUserId { get; set; }

        [ForeignKey("AddedByUserId")]
        public virtual User AddedByUser { get; set; }

        [ForeignKey("AirlineAreaId")]
        public AirlineArea AirlineArea { get; set; }


        [ForeignKey("PortId")]
        public Port Port { get; set; }
    }
}
