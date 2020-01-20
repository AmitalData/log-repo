using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class QueryMapping
    {
        public static void MapEntity(QueryPM entityPM, Query entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Code = entityPM.Code;
                entityPOCO.UniqueCode = entityPM.UniqueCode;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.DisplayCount = entityPM.DisplayCount;
            entityPOCO.IndexOrder = entityPM.IndexOrder;
            entityPOCO.QuerySection = entityPM.QuerySection;
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.QueryGroupCode = entityPM.QueryGroupCode;
            entityPOCO.OriginalQueryId = entityPM.OriginalQueryId;
            entityPOCO.OriginalQueryCode = entityPM.OriginalQueryCode;

            entityPOCO.SystemLevel = entityPM.SystemLevel;           
            entityPOCO.TenantLevel = entityPM.TenantLevel;
            entityPOCO.UserId = entityPM.UserId;
            entityPOCO.IsAddNewEntityEnabled = entityPM.IsAddNewEntityEnabled;
            entityPOCO.NameTextCodeId = entityPM.NameTextCodeId;
            entityPOCO.DefaultSortColumn = entityPM.DefaultSortColumn;
            entityPOCO.DefaultSortDirection = entityPM.DefaultSortDirection;
            entityPOCO.SpotlightDataTemplate = entityPM.SpotlightDataTemplate;
            entityPOCO.Agent = entityPM.Agent;
            entityPOCO.Customer = entityPM.Customer;
            entityPOCO.Internal = entityPM.Internal;
            entityPOCO.Perspective = entityPM.Perspective;
            entityPOCO.IsHiddenFromView = entityPM.IsHiddenFromView;
            entityPOCO.IsNewFromTenantZeroOnly = entityPM.IsNewFromTenantZeroOnly;
            entityPOCO.EditWizardComponentPath = entityPM.EditWizardComponentPath;
            entityPOCO.SharedWithAll = entityPM.SharedWithAll;
            entityPOCO.SharedWithSpecificUsers = entityPM.SharedWithSpecificUsers;
            entityPOCO.SharedByUserId = entityPM.SharedByUserId;
            entityPOCO.SpotlightModeActivated = entityPM.SpotlightModeActivated;
            entityPOCO.NameTextCodeCode = entityPM.NameTextCodeCode;

        }
    }
}