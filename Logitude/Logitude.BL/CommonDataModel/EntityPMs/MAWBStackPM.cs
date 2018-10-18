using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class MAWBStackPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int Number { get; set; }
        public string AirlineId { get; set; }
        public DateTime InsertionDate { get; set; }
        public string Notes { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToShipperName { get; set; }
        public bool IsUsed { get; set; }
        public string AirlineName { get; set; }
        [Include]
        [Association("MAWBStackAirline", "AirlineId", "Id", IsForeignKey = true)]
        public  AirlinePM Airline { get; set; }
    }
}