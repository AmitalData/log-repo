using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class MAWBStack
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int Number { get; set; }
        public string AirlineId { get; set; }
        public DateTime InsertionDate { get; set; }
        public string Notes { get; set; }

        public string AssignedToId { get; set; }
        public bool IsUsed { get; set; }

        //[Include]
        //[Association("MAWBStackAirline", "AirlineId", "Id", IsForeignKey = true)]
        [ForeignKey("AirlineId")]
        public virtual Airline Airline { get; set; }

        [ForeignKey("AssignedToId")]
        public virtual Card AssignedToCard { get; set; }
    }
}