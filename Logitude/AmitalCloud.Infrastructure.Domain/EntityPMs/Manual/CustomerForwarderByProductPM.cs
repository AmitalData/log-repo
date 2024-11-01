using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CustomerForwarderByProductPM : BaseClasses.BaseEntityPM
    {
        [Key]
        public string ProductTypeCode { get; set; }
        public string ForwarderId { get; set; }
        [Key]
        public string CustomerId { get; set; }
        public string ForwarderName { get; set; }
    }
}
