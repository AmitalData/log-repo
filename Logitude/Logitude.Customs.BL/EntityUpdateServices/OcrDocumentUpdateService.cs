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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;

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
           
            if (!string.IsNullOrEmpty(entityPM?.JsonData))
            {

                SupplierInvoiceOcr convertJson = JsonConvert.DeserializeObject<SupplierInvoiceOcr>(entityPM.JsonData);//json מיפוי
                entityPM.Reference = convertJson?.pages.FirstOrDefault(page =>
                    page.prediction?.FirstOrDefault(p => p.label == "invoice_number") != null)
                    ?.prediction.FirstOrDefault(p => p.label == "invoice_number")?.ocr_text;

            }

            if (entityPM != null && entityPM.ChangeSetOp == ChangeSetOperation.Insert) 
            {
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(entityPM.Tenant);
                DocumentsFilingPM documentsFiling = documentsFilingQuery.GetSinglePM(entityPM.DocId, entityPM.Tenant);
                documentsFiling.OcrStatusCode = "2";
                ISendBondedCustomDocumentService myISendBondedCustomDocumentService = ContainerAccessor.Container.Resolve(typeof(ISendBondedCustomDocumentService), "SendBondedCustomDocumentService", new ParameterOverride("", entityPM.Tenant)) as ISendBondedCustomDocumentService;
                myISendBondedCustomDocumentService.JustDoIt(documentsFiling);
            }






        }
    }
}
