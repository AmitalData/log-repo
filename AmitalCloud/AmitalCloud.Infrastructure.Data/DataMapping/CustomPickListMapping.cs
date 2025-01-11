using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class CustomPickListMapping
    {
        public static void MapEntity(CustomPickListPM picklistpm, CustomPickList picklist, bool isNewState)
        {
            picklist.Code = picklistpm.Code;
            picklist.Tenant = picklistpm.Tenant;
            picklist.Value = picklistpm.Value;
            picklist.IsMultipleChoice = picklistpm.IsMultipleChoice;
        }
    }
}