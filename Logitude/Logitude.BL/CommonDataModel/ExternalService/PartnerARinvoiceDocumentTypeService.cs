using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.ExternalService
{
    class PartnerARinvoiceDocumentTypeService
    {
        private int tenant;
        private List<DocumentTypeList> documentTypeLists;
        public PartnerARinvoiceDocumentTypeService(int tenant)
        {
            this.tenant = tenant;
            documentTypeLists =  new DocumentTypeQuery(tenant).GetDocumentTypeListsByObjectTableId(ObjectTableRepository.GetObjectTableByName("ARInvoice"), tenant);

        }
        public CardPM Set(CardPM card)
        {
            card.SingleInvoiceTemplateId = (card.SingleInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999S") : card.SingleInvoiceTemplateId;
            card.CustomsInvoiceTemplateId = (card.CustomsInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999CI") : card.CustomsInvoiceTemplateId; 
            card.ConsolidationInvoiceTemplateId = (card.ConsolidationInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999C") : card.ConsolidationInvoiceTemplateId;
            card.ManifestInvoiceTemplateId = (card.ManifestInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999M") : card.ManifestInvoiceTemplateId;
            return card;
        }

        private string GetDefaultDocumentTypeTemplateId(string documentTypeCode)
        {
            var documentTypeList = documentTypeLists.Where(d => d.Code == documentTypeCode).FirstOrDefault();
            return documentTypeList?.DocumentTypeDefaultReportTemplateId;
        }
    }
}
