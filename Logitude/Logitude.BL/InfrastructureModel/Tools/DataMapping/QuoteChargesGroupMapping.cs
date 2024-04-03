using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class QuoteChargesGroupMapping
    {
        public static void MapEntity(QuoteChargesGroupPM entityPM, QuoteChargesGroup entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.ViewOrder = entityPM.ViewOrder;
            entityPOCO.SearchFields = entityPM.Code + "," + entityPM.Name + "," + entityPM.LocalName;
        }
    }
}
