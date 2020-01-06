using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddObjectTableHelperControls
    {
        public static void AddObjectTableHelperControl(ObjectTableHelperControlDetails objectTableHelperDetails, ObjectTableHelperControlRepository helperRepository, Dictionary<string, ObjectTableHelperControl> tenantHelpers)
        {
            if (tenantHelpers.Keys.Contains(objectTableHelperDetails.Code))
            {
                ObjectTableHelperControl helper = tenantHelpers[objectTableHelperDetails.Code];
                helper.ControlPath = objectTableHelperDetails.ControlPath;
                helper.ObjectTableId = objectTableHelperDetails.ObjectTableId;
                helper.Tenant = objectTableHelperDetails.Tenant;
                helper.FeatureId = objectTableHelperDetails.FeatureId;
                helper.FeatureUniqeCode = objectTableHelperDetails.FeatureUniqeCode;


                helperRepository.Update(helper);
            }
            else
            {
                ObjectTableHelperControl newHelper = new ObjectTableHelperControl()
                {
                    Tenant = objectTableHelperDetails.Tenant,
                    ObjectTableId = objectTableHelperDetails.ObjectTableId,
                    ControlPath = objectTableHelperDetails.ControlPath,
                    Code = objectTableHelperDetails.Code,
                    FeatureId=objectTableHelperDetails.FeatureId,
                    FeatureUniqeCode = objectTableHelperDetails.FeatureUniqeCode,
                    Id = IdCounter.GetNumber("ObjectTableHelperControl", objectTableHelperDetails.Tenant).ToString(),

                };
                helperRepository.Add(newHelper);
            }
        }
    }
}