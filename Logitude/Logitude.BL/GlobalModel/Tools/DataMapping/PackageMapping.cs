using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class PackageMapping
    {
        public static void MapEntity(PackagePM entityPM, Package entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Code = entityPM.Code;
                entityPOCO.FeaturePackageTypeCode = entityPM.FeaturePackageTypeCode;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.InActive = entityPM.InActive;
            BuildSearchField(entityPM, entityPOCO);
        }

        private static void BuildSearchField(PackagePM entityPM, Package entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
