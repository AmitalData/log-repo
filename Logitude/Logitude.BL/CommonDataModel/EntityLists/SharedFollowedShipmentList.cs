using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class SharedFollowedShipmentList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime TrackDate { get; set; }
    }
}
