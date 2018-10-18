using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class UserRolesPM
    {
        [Key]
        public string Id { get; set; }
       
        public string Name { get; set; }
        public int Tenant { get; set; }

        public bool Exists { get; set; }
        public bool Added { get; set; }
        public bool Removed { get; set; }

        public string UserId { get; set; }
    }
}