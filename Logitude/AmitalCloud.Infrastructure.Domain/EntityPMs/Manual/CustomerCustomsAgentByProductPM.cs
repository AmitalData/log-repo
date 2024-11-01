using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CustomerCustomsAgentByProductPM : BaseClasses.BaseEntityPM
    {
        [Key]
        public string ProductTypeCode { get; set; }
        public string CustomsAgentId { get; set; }
        [Key]
        public string CustomerId { get; set; }
        public string CustomsAgentName { get; set; }
    }
}
