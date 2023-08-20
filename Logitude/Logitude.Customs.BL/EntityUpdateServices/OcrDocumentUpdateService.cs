using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityPOCOs;
using Newtonsoft.Json;
using static Logitude.Customs.BL.Messaging.Customs.SupplierInvoiceByOcr;
using System.Linq;

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
            if (string.IsNullOrEmpty(entityPM?.JsonData))
            {

                SupplierInvoiceOcr convertJson = JsonConvert.DeserializeObject<SupplierInvoiceOcr>(entityPM.JsonData);//json מיפוי
                entityPM.Reference = convertJson?.pages[0]?.prediction.FirstOrDefault(p => p.label == "invoice_number")?.ocr_text;

            }

            if (entityPM != null && entityPM.ChangeSetOp == ChangeSetOperation.Insert) {
                var customContext = MainContext as ICustomContext;
                var customsDocumentQueryService = new CustomsDocumentQueryService(customContext);
                CustomsDocumentPM customsDocumentPM = customsDocumentQueryService.GetSingleCustomsDocumentPMWithDeclarationId(entityPM.DocId, entityPM.Tenant);
                if (customsDocumentPM != null && customsDocumentPM?.CustomsDocId == null)
                {
                    CustomsDocumentUpdateService CustomsDocumentUpdateService = new CustomsDocumentUpdateService(entityPM.Tenant);
                    CustomsDocumentUpdateService.SendMessageToQueue(customsDocumentPM);
                }

            }






        }
    }
}
