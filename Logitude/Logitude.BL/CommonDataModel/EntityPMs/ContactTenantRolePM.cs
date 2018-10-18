using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ContactTenantRolePM
    {
        [Key]
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string ContactTenantId { get; set; }
        public int Tenant { get; set; }
    }
}
