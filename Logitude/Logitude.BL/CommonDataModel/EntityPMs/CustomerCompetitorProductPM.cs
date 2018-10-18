using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerCompetitorProductPM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }

        [Key]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }

        public string CustomerName { get; set; }

        public string ProductTypeName { get; set; }

        public string CompetitorName { get; set; }

        public Simplog.Server.Infrastructure.ChangeSetOperation ChangeSetOp { get; set; }
    }
}