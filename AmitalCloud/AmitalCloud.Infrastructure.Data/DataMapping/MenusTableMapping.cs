using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class MenusTableMapping
    {
        public static void MapEntity(MenusTablePM menusTablePM, MenusTable menusTable, bool isNewState)
        {
            menusTable.CategoryTypeCode = menusTablePM.CategoryTypeCode;
            menusTable.Icon = menusTablePM.Icon;
            menusTable.IndexOfOrder = menusTablePM.IndexOfOrder;
            menusTable.MenuTypeCode = menusTablePM.MenuTypeCode;
            menusTable.ObjectTableId = menusTablePM.ObjectTableId;
            menusTable.Tenant = menusTablePM.Tenant;
            menusTable.TextCode = menusTablePM.TextCode;
            menusTable.UserControlName = menusTablePM.UserControlName;
            menusTable.FeatureId = menusTablePM.FeatureId;
            menusTable.Code = menusTablePM.Code;
            menusTable.HtmlView = menusTablePM.HtmlView;
            menusTable.FeatureUniqeCode = menusTablePM.FeatureUniqeCode;
            menusTable.QuerySection = menusTablePM.QuerySection;


        }
    }
}