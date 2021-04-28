using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddMenusTables
    {
        public static void AddMenusTable(MenusTableDetails menusTableDetails,MenusTableRepository menusTableRepository,Dictionary<string,MenusTable> tenantMenusTables)
        {
            if (tenantMenusTables.Keys.Contains(menusTableDetails.Code))
            {
                MenusTable menusTable = tenantMenusTables[menusTableDetails.Code];
                menusTable.CategoryTypeCode = menusTableDetails.CategoryTypeCode;
                menusTable.FeatureId = menusTableDetails.FeatureId;
                menusTable.Icon = menusTableDetails.Icon;
                menusTable.IndexOfOrder = menusTableDetails.IndexOfOrder;
                menusTable.MenuTypeCode = menusTableDetails.MenuTypeCode;
                menusTable.ObjectTableId = menusTableDetails.ObjectTableId;
                menusTable.Tenant = menusTableDetails.Tenant;
                menusTable.TextCode = menusTableDetails.TextCode;
                menusTable.UserControlName = menusTableDetails.UserControlName;
                menusTable.HtmlView = menusTableDetails.HtmlView;
                menusTable.FeatureUniqeCode = menusTableDetails.FeatureUniqeCode;
                menusTable.QuerySection = menusTableDetails.QuerySection;


                
                menusTableRepository.Update(menusTable);
            }

            else
            {
                MenusTable newMenusTable = new MenusTable()
                {
                    UserControlName = menusTableDetails.UserControlName,
                    TextCode = menusTableDetails.TextCode,
                    Tenant = menusTableDetails.Tenant,
                    ObjectTableId = menusTableDetails.ObjectTableId,
                    MenuTypeCode = menusTableDetails.MenuTypeCode,
                    IndexOfOrder = menusTableDetails.IndexOfOrder,
                    Icon = menusTableDetails.Icon,
                    FeatureId = menusTableDetails.FeatureId,
                    CategoryTypeCode = menusTableDetails.CategoryTypeCode,
                    Code = menusTableDetails.Code,
                    FeatureUniqeCode = menusTableDetails.FeatureUniqeCode,
                    QuerySection = menusTableDetails.QuerySection,


                Id = IdCounter.GetNumber("MenusTable",menusTableDetails.Tenant).ToString(),
                };
                menusTableRepository.Add(newMenusTable);
            }
        }
    }
}