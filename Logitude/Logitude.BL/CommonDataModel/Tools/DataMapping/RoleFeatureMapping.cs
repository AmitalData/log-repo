using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class RoleFeatureMapping
    {
        public static void MapEntity(RoleFeaturePM roleFeaturePm, RoleFeature roleFeature, bool isNewState)
        {
            roleFeature.FeatureId = roleFeaturePm.FeatureId;
            roleFeature.Tenant = roleFeaturePm.Tenant;
            roleFeature.RoleId = roleFeaturePm.RoleId;
            roleFeature.FeatureAccessLevelCode = roleFeaturePm.FeatureAccessLevelCode;
            roleFeature.FeatureUniqeCode = roleFeaturePm.FeatureUniqeCode;
        }
    }
}