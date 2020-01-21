using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class PackageFeaturePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageCode { get; set; }
        public string FeatureId { get; set; }
        public string FeatureUniqeCode { get; set; }

    }
}