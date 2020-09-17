using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class HelpResourceMapping
    {
        public static void MapEntity(HelpResourcePM entityPM, HelpResource poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Code = entityPM.Code;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
            }

            poco.Name = entityPM.Name;
            poco.Type = entityPM.Type;
            poco.Category = entityPM.Category;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.Language = entityPM.Language;
            poco.VideoURL = entityPM.VideoURL;
            poco.Duration = entityPM.Duration;
            poco.FileName = entityPM.FileName;
            poco.IsNew = entityPM.IsNew;
            poco.FeatureCode = entityPM.FeatureCode;
            poco.Inactive = entityPM.Inactive;

            BuildSearchFields(entityPM, poco);
        }
        private static void BuildSearchFields(HelpResourcePM entityPM, HelpResource entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
