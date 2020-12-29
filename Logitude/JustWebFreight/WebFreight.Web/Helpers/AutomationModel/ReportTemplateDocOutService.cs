using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class ReportTemplateDocOutService
    {

        private ReportTemplateDocOutArgs reportTemplateDocOutArgs = null;
        public ReportTemplateDocOutService(ReportTemplateDocOutArgs reportTemplateDocOutArgs)
        {
            this.reportTemplateDocOutArgs = reportTemplateDocOutArgs;
        }

        public string GetDocOutDocumentId()
        {
            string documentId = string.Empty;
            string documentOutId = GetDocumentOutId();

            if (!string.IsNullOrEmpty(documentOutId))
            {
                DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(reportTemplateDocOutArgs.Tenant);
                var documentOutCopyPM = documentOutCopyQuery.GeDocumentOutCopyPMByDocumentOutId(documentOutId, reportTemplateDocOutArgs.Tenant);
                documentId = documentOutCopyPM != null ? documentOutCopyPM.DocumentId : "";
            }
            return documentId;
        }

        private string GetDocumentOutId()
        {
            string documentOutId = string.Empty;
            var commonDataContext = CommonDataContext.GetContext(reportTemplateDocOutArgs.Tenant);
            string parentObjectTableId = GetParentObjectTableId(reportTemplateDocOutArgs.ObjectTableId, reportTemplateDocOutArgs.Tenant);
            if (!string.IsNullOrEmpty(parentObjectTableId))
            {
                documentOutId = (from a in commonDataContext.DocumentOuts.Include("DocumentsFiling")
                                 where a.Tenant == reportTemplateDocOutArgs.Tenant && a.DocumentsFiling.DocumentTypeId == reportTemplateDocOutArgs.DocumentTypeId && a.DocumentTemplateId == reportTemplateDocOutArgs.ReportTemplateId && a.DocumentsFiling.ObjectTableId == parentObjectTableId && a.DocumentsFiling.DirectionCode == "O" && a.DocumentsFiling.ChildEntityId == reportTemplateDocOutArgs.EntityId
                                 select a.Id).FirstOrDefault();
            }
            else
            {
                documentOutId = (from a in commonDataContext.DocumentOuts.Include("DocumentsFiling")
                                 where a.Tenant == reportTemplateDocOutArgs.Tenant && a.DocumentsFiling.DocumentTypeId == reportTemplateDocOutArgs.DocumentTypeId && a.DocumentTemplateId == reportTemplateDocOutArgs.ReportTemplateId && a.DocumentsFiling.ObjectTableId == reportTemplateDocOutArgs.ObjectTableId && a.DocumentsFiling.DirectionCode == "O" && a.DocumentsFiling.EntityId == reportTemplateDocOutArgs.EntityId
                                 select a.Id).FirstOrDefault();
            }

            return documentOutId;
        }

        private string GetParentObjectTableId(string objectTableId, int tenant)
        {
            string parentObjectTableId = string.Empty;
            ObjectTable objectTable = ObjectTableRepository.GetSingleObjectTableById(objectTableId, tenant);
            if (objectTable != null && (objectTable.Name == "ARInvoice" || objectTable.Name == "APInvoice"))
            {
                parentObjectTableId = ObjectTableRepository.GetObjectTableByName("Shipment");
            }
            return parentObjectTableId;
        }
    }

    public class ReportTemplateDocOutArgs
    {
        public int Tenant { get; set; }
        public string ReportTemplateId { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string DocumentTypeId { get; set; }




    }
}