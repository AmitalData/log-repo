using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddObjectTableTabs
    {
        public static void AddObjectTableTab(ObjectTableTabDetails objectTableTabDetails,ObjectTableTabRepository objectTableTabRepository,Dictionary<string, ObjectTableTab> tenantObjectTableTab)
        {

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                if (string.IsNullOrEmpty(objectTableTabDetails.ControlPath))
                {
                    objectTableTabDetails.ControlPath = "-";
                }
            }

//#endif
            if (tenantObjectTableTab.Keys.Contains(objectTableTabDetails.Code))
            {
                ObjectTableTab objectTableTab = tenantObjectTableTab[objectTableTabDetails.Code];
                objectTableTab.ControlPath = objectTableTabDetails.ControlPath;
                objectTableTab.IndexOrder = objectTableTabDetails.IndexOrder;
                objectTableTab.ObjectTableId = objectTableTabDetails.ObjectTableId;
                objectTableTab.TabNameTextCodeId = objectTableTabDetails.TabNameTextCodeId;
                objectTableTab.TabNameTextCodeCode = objectTableTabDetails.TabNameTextCodeCode;
                objectTableTab.FeatureId = objectTableTabDetails.FeatureId;
                objectTableTab.HtmlComponentName = objectTableTabDetails.HtmlComponentName;
                objectTableTab.HtmlComponentUrl = objectTableTabDetails.HtmlComponentUrl;

                objectTableTabRepository.Update(objectTableTab);
            }
            else
            {
                ObjectTableTab newObjectTableTab = new ObjectTableTab()
                {
                    ObjectTableId = objectTableTabDetails.ObjectTableId,
                    IndexOrder = objectTableTabDetails.IndexOrder,
                    ControlPath = objectTableTabDetails.ControlPath,
                    Code = objectTableTabDetails.Code,
                    Id = IdCounter.GetNumber("ObjectTableTab", objectTableTabDetails.Tenant).ToString(),
                    TabNameTextCodeId = objectTableTabDetails.TabNameTextCodeId,
                    TabNameTextCodeCode = objectTableTabDetails.TabNameTextCodeCode,
                    Tenant = objectTableTabDetails.Tenant,
                    FeatureId = objectTableTabDetails.FeatureId,
                    HtmlComponentName = objectTableTabDetails.HtmlComponentName,
                    HtmlComponentUrl = objectTableTabDetails.HtmlComponentUrl,

                };
 

                objectTableTabRepository.Add(newObjectTableTab);
            }

        }
    }
}