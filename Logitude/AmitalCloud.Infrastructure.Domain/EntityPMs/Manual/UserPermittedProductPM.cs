using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class UserPermittedProductPM : BaseClasses.BaseEntityPM
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductTypeCode { get; set; }

    }
}
