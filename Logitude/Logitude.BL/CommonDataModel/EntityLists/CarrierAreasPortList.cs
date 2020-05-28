using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CarrierAreasPortList
    {
        [Key]
        public string Id { get; set; }
        public string CarrierAreaId { get; set; }
        public string PortId { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public DateTime? AddedDate { get; set; }
        public string AddedByUserId { get; set; }

    }
}