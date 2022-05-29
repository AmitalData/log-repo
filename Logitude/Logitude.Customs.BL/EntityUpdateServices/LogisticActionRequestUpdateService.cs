using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using System;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;

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

            string text = null;
            if (!CheckIfUpdatingAllowed(entityPM, ref text))
            {
                throw new Exception(text);
            }
            var res = new DeclarationRepository(entityPM.Tenant).GetDeclarationId(entityPM.ExportFileNo, entityPM.ExporterNumber, entityPM.TransportmodeId, entityPM.CargoIdentifierType, entityPM.CargoIdentifierKey1, entityPM.CargoIdentifierKey2, entityPM.CargoIdentifierKey3);

            if (res != null && res.Id != null)
                entityPM.DeclarationId = res.Id;

            base.OnUpdating(entityPM);

        }

        public bool CheckIfUpdatingAllowed(LogisticActionRequestPM myLogisticActionRequestPM, ref string text)
        {
            var context = CustomContext.GetContext(myLogisticActionRequestPM.Tenant);

            LogisticActionRequestQueryService logisticActionRequestQueryService = new LogisticActionRequestQueryService(context);
            if (!string.IsNullOrWhiteSpace(myLogisticActionRequestPM.Id))
            {
                var exist = logisticActionRequestQueryService.GetExistByCargoKey(myLogisticActionRequestPM.Id, myLogisticActionRequestPM.CargoIdentifierKey1, myLogisticActionRequestPM.CargoIdentifierKey2, myLogisticActionRequestPM.CargoIdentifierKey3, myLogisticActionRequestPM.CargoIdentifierType);
                if (exist)
                {
                    text = TranslateTextsClass.Translate("Customs.General.O.LogisticActionRequestAlreadyExist", myLogisticActionRequestPM.Tenant, true);
                    if (string.IsNullOrWhiteSpace(text)) text = "קיימת בקשה לביטול יצוא עם אותם מזהי מטען";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                LogMessagingUtil.Instance.AppendLine(text);
                return false;
            }

            return true;
        }
    }
}
