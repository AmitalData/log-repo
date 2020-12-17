using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class ReportTemplateDocOutService
    {

        public string GetDocOutDocumentId(string documentTypeId, AutomationSendEmailArgs automationSendEmailArgs)
        {
            string documentId = string.Empty;
            var commonDataContext = CommonDataContext.GetContext(automationSendEmailArgs.Tenant);
            var documentOutId = (from a in commonDataContext.DocumentOuts.Include("DocumentsFiling")
                                 where a.Tenant == automationSendEmailArgs.Tenant && a.DocumentsFiling.DocumentTypeId == documentTypeId && a.DocumentTemplateId == automationSendEmailArgs.ReportTemplateId && a.DocumentsFiling.ObjectTableId == automationSendEmailArgs.ObjectTableId && a.DocumentsFiling.DirectionCode == "O" && a.DocumentsFiling.EntityId == automationSendEmailArgs.EntityId
                                 select a.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(documentOutId))
            {
                DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(automationSendEmailArgs.Tenant);
                var documentOutCopyPM = documentOutCopyQuery.GeDocumentOutCopyPMByDocumentOutId(documentOutId, automationSendEmailArgs.Tenant);
                documentId = documentOutCopyPM != null ? documentOutCopyPM.DocumentId : "";
            }
            return documentId;
        }

    }
}