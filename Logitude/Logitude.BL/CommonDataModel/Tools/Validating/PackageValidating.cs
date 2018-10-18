using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class PackageValidating
    {
        public static void Validate(PackagePM entityPM, bool isNewEntity)
        {
            if (entityPM.FeaturePackageTypeCode == "PK" || entityPM.FeaturePackageTypeCode == "AD")
            {
                if (entityPM.ConnectedPackages.Count == 0)
                {
                    throw new ApplicationException("You must select 1 package at least");
                }
            }
        }

    }
}