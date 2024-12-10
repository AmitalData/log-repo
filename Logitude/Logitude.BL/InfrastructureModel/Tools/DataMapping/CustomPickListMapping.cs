using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
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