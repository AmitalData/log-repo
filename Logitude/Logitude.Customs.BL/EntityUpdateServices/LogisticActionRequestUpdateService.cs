using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class LogisticActionRequestUpdateService : EntityUpdateService<LogisticActionRequest, LogisticActionRequestPM, EntityPM>
    {
        protected override void OnUpdating(LogisticActionRequestPM entityPM)
        {
            var res = new DeclarationRepository(entityPM.Tenant).GetDeclarationId(entityPM.ExportFileNo, entityPM.ExporterNumber, entityPM.TransportmodeId, entityPM.CargoIdentifierType, entityPM.CargoIdentifierKey1, entityPM.CargoIdentifierKey2, entityPM.CargoIdentifierKey3);

            if (res != null && res.Id != null)
                entityPM.DeclarationId = res.Id;

            base.OnUpdating(entityPM);
        }
    }
}
