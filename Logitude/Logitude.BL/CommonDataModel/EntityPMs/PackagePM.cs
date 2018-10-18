using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class PackagePM
    {
        [Key]
        public string Code { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }

        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FeaturePackageTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FeaturePackageTypeName { get; set; }

        public string SearchFields { get; set; }

        private List<PackageConnectedPackagePM> connectedPackages;
        [Include]
        [Association("PackageConnectedPackagePMPackagePM", "Code", "PackageCode")]
        [Composition]
        public virtual List<PackageConnectedPackagePM> ConnectedPackages
        {
            get
            {
                if (connectedPackages == null)
                {
                    connectedPackages = new List<PackageConnectedPackagePM>();
                }

                return connectedPackages;
            }

            set
            {
                connectedPackages = value;
            }
        }
    }
}