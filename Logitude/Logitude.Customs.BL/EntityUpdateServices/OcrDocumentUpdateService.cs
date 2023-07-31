using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{

    public partial class OcrDocumentUpdateService : ICanUpdateClosedTable<OcrDocumentPM>
    {

        protected override void OnCreating(OcrDocumentPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.OcrDocument", entityPM.Tenant);  

        }
        protected override void OnUpdating(OcrDocumentPM entityPM)
        {
            //   ValidatePM(entityPM);
            //CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant)
            //;

            if(entityPM!= null && entityPM.ChangeSetOp == ChangeSetOperation.Insert) {
                var customContext = MainContext as ICustomContext;
                var customsDocumentQueryService = new CustomsDocumentQueryService(customContext);
                CustomsDocumentPM customsDocumentPM = customsDocumentQueryService.GetSingleCustomsDocumentPMWithDeclarationId(entityPM.Id, entityPM.Tenant);
                  if (customsDocumentPM != null && customsDocumentPM?.CustomsDocId==null)
                 {

                    CustomsDocumentUpdateService CustomsDocumentUpdateService = new CustomsDocumentUpdateService(entityPM.Tenant);
                    CustomsDocumentUpdateService.SendMessageToQueue(customsDocumentPM);

                 }

            }






        }
    }
}
