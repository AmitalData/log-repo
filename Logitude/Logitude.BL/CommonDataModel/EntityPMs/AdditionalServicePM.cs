using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AdditionalServicePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }       
        public string Name { get; set; }        
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
    }
}