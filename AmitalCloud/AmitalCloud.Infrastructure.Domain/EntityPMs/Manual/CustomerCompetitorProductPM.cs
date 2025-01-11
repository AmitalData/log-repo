using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs 
{
    public class CustomerCompetitorProductPM : BaseClasses.BaseEntityPM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }

        [Key]
        public string ProductTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductTypeName { get; set; }
        public string CompetitorName { get; set; }
    }
}