using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class RestrictionPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string ContactTenantId { get; set; }

        public string UserId { get; set; }
        public string ObjectFieldName { get; set; }
    }
}