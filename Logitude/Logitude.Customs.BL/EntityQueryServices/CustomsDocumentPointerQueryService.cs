using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsDocumentPointerQueryService
    {
        public List<CustomsDocumentPointerPM> GetCustomsDocumentPointerPMsByEntityIdAndChilds(string entityId, string childEntityId1, string childEntityId2, string childEntityId3, int tenant)
        {
            List<CustomsDocumentPointer> DocumentPointers;

            DocumentPointers = repository.GetCustomsDocumentPointerPMsByEntityIdAndChilds(entityId, childEntityId1, childEntityId2, childEntityId3, tenant);
            var DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        private static List<CustomsDocumentPointerPM> GetDocuments(int tenant, List<CustomsDocumentPointer> DocumentPointers)
        {
            List<CustomsDocumentPointerPM> DocumentPointerPMs = new List<CustomsDocumentPointerPM>();

            foreach (CustomsDocumentPointer docPtr in DocumentPointers)
            {
                //CustomsDocumentsTicketId adjustment 16/10/2016 mohammad
                //CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
                //CustomsDocumentPM document = customsDocumentQueryService.GetSingle(docPtr.CustomsDocumentsTicketId,false, false);

                CustomsDocumentPointerPM DocPM = new CustomsDocumentPointerPM();
                DocPM.Id = docPtr.Id; 
                DocPM.Tenant = docPtr.Tenant;
                DocPM.ParentEntityCode = docPtr.ParentEntityCode;
                DocPM.ParentEntityId = docPtr.ParentEntityId;
                DocPM.OriginEntity = docPtr.OriginEntity;
                DocPM.Child1EntityCode = docPtr.Child1EntityCode;
                DocPM.Child1EntityId = docPtr.Child1EntityId;
                DocPM.Child2EntityCode = docPtr.Child2EntityCode;
                DocPM.Child2EntityId = docPtr.Child2EntityId;
                DocPM.Child3EntityCode = docPtr.Child3EntityCode;
                DocPM.Child3EntityId = docPtr.Child3EntityId;
                //DocPM.DocumentTypeCode = docPtr.DocumentTypeCode;
                //DocPM.DocumentTypeName = docPtr.CustomDocumentType == null ? null : docPtr.CustomDocumentType.LocalName;
                DocPM.CustomsDocumentsTicketId = docPtr.CustomsDocumentsTicketId;
               // DocPM.RequiredDocID = docPtr.RequiredDocID;
              //  DocPM.UploadApproved = docPtr.UploadApproved;

                //CustomsDocumentsTicketId adjustment 16/10/2016 mohammad
                //if (document != null)
                //{
                //    DocPM.CustomsDocId = document.CustomsDocId;
                //    DocPM.DocumentRemarks = document.DocumentRemarks;
                //    DocPM.DocumentStatusCode = document.DocumentStatusCode;
                //    DocPM.DocumentStatusName = document.DocumentStatusName;
                //    DocPM.Extension = document.Extension;
                //    DocPM.FileSize = document.FileSize;
                //    DocPM.Name = document.Name;
                //    DocPM.IsMetaDataReady = document.IsMetaDataReady;
                //}

                DocumentPointerPMs.Add(DocPM);
            }
            return DocumentPointerPMs;
        }

        //Yuval Chalup 03.11.2014 TASK-4238 --->
        public List<CustomsDocumentPointerPM> GetParentDocumentPointer(string parentEntityId, string parentEntityCode, int tenant)
        {
            List<CustomsDocumentPointer> DocumentPointers;
            DocumentPointers = repository.GetParentDocumentPointer(parentEntityId, parentEntityCode, tenant);
            var DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        public List<CustomsDocumentPointerPM> GetPointersForTicket(string customsDocumentsTicketId, int tenant)
        {
            List<CustomsDocumentPointer> DocumentPointers;

            DocumentPointers = repository.GetCustomDocumentPointersForTicketId(customsDocumentsTicketId, tenant);
            var DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        public List<CustomsDocumentPointerPM> GetPointersForMultipleTickets(List<string> customsDocumentsTicketIds, int tenant)
        {
            List<CustomsDocumentPointer> DocumentPointers;

            DocumentPointers = repository.GetCustomDocumentPointersForMultipleTicketIds(customsDocumentsTicketIds, tenant);
            var DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        public bool CheckForPointers(string parentEntityId, string child1EntityId, string child2EntityId, string child3EntityId,int tenant)
        {
            return repository.CheckForPointers(parentEntityId, child1EntityId, child2EntityId, child3EntityId, tenant);
        } 

        public List<CustomsDocumentPointerPM> GetCustomsDocumentPointerPMsByRequiredDocID(string requiredDocID, int tenant)
        {
            if (string.IsNullOrWhiteSpace(requiredDocID)) return null;

            List<CustomsDocumentPointer> DocumentPointers = repository.GetCustomsDocumentPointerPMsByRequiredDocID(requiredDocID, tenant);
            List<CustomsDocumentPointerPM> DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        public List<CustomsDocumentPointerPM> GetCustomDocumentPointersForCustomDocumentId(string documentsFilingId, int tenant)
        {
            if (string.IsNullOrWhiteSpace(documentsFilingId)) return null;

            List<CustomsDocumentPointer> DocumentPointers = repository.GetCustomDocumentPointersForCustomDocumentId(documentsFilingId, tenant);
            List<CustomsDocumentPointerPM> DocumentPointerPMs = GetDocuments(tenant, DocumentPointers);
            return DocumentPointerPMs;
        }

        public bool CheckIfDocumentPointerExistsForConstraint(string child1EntityId, string child1EntityCode, int tenant)
        {
            return repository.CheckIfDocumentPointerExistsForConstraint(child1EntityId, child1EntityCode, tenant);
        }

        public IQueryable<CustomsDocumentPointer> GetCustomsDocumentPointerList(GetTicketsParams parameters, int tenant)
        {
            return repository.GetCustomsDocumentPointerList(parameters, tenant);         
            
        }

        public IQueryable<CustomsDocumentPointer> GetCustomsDocumentPointerListParentOnly(GetTicketsParams parameters, int tenant)
        {
            return repository.GetCustomsDocumentPointerListParentOnly(parameters, tenant);

        }

        public List<CustomsDocumentPointerPM> GetCustomDocumentPoinersForItems(string parentEntityId, string invCounterKey, string itemLineNumbers, int tenant)
        {
            IQueryable<CustomsDocumentPointer> pointers = repository.GetCustomDocumentPoinersForItems(parentEntityId, invCounterKey, itemLineNumbers,tenant);

            List<CustomsDocumentPointerPM> pointerPms = (from a in pointers
                                                         select new CustomsDocumentPointerPM() {
                                                              ParentEntityId=a.ParentEntityId,
                                                              Child1EntityId=a.Child1EntityId,
                                                              Child2EntityId=a.Child2EntityId,
                                                              Id=a.Id,
                                                              Tenant=a.Tenant,
                                                         }).ToList();
            return pointerPms;
        }

    }
}
