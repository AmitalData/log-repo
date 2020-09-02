using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TarrifFromToPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public string PortId { get; set; }
        public string CountryId { get; set; }
        public string TarrifFromToTypeCode { get; set; }

        //Dummy
        public string PortCode { get; set; }
        public string CountryCode { get; set; }
        public ChangeSetOperation changeOp { get; set; }
    }
}