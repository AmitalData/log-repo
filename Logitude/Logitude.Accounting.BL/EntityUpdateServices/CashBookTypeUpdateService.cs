using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class CashBookTypeUpdateService : EntityUpdateService<CashBookType, CashBookTypePM, EntityPM>
    {

        protected override void OnCreating(CashBookTypePM entityPM, EntityPM entityParentPM)
        {
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(CashBookTypePM entityPM)
        {
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            base.OnUpdating(entityPM);
        }

    }
}
