using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class FeatureMapping
    {
        public static void MapEntity(FeaturePM entityPM, Feature entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Code = entityPM.Code;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.IsOld = entityPM.IsOld;
            }

            entityPOCO.NameTextCodeId = entityPM.NameTextCodeId;
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.FeatureTypeCode = entityPM.FeatureTypeCode;
            entityPOCO.Packagable = entityPM.Packagable;
            entityPOCO.IsBusinessUnitEnabled = entityPM.IsBusinessUnitEnabled;
            entityPOCO.IsCoreFeature = entityPM.IsCoreFeature;
            entityPOCO.FeatureUniqeCode = entityPM.FeatureUniqeCode;

            entityPOCO.NameTextCodeCode = entityPM.NameTextCodeCode;
        }
    }
}