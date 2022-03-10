using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using System;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class LogisticActionRequestUpdateService : EntityUpdateService<LogisticActionRequest, LogisticActionRequestPM, EntityPM>
    {
        protected override void OnCreating(LogisticActionRequestPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.LogisticActionRequest", entityPM.Tenant).ToString();
            if (entityPM.RequestDate == null)
                entityPM.RequestDate = DateTime.Now;

            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(LogisticActionRequestPM entityPM)
        {
            var res = new DeclarationRepository(entityPM.Tenant).GetDeclarationId(entityPM.ExportFileNo, entityPM.ExporterNumber, entityPM.TransportmodeId, entityPM.CargoIdentifierType, entityPM.CargoIdentifierKey1, entityPM.CargoIdentifierKey2, entityPM.CargoIdentifierKey3);

            if (res != null && res.Id != null)
                entityPM.DeclarationId = res.Id;

            base.OnUpdating(entityPM);
        }
    }
}
