using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class SharedFollowedShipmentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime TrackDate { get; set; }
    }
}
