using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityKeys;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using POCO = AmitalCloud.Infrastructure.Model.EntityClasses ;

namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public class DocumentsFilingQuery
    {
        DocumentsFilingRepository repository;
        #region Constructors

        public DocumentsFilingQuery(int tenant) : this(new DocumentsFilingRepository(tenant))
        {
        }
        public DocumentsFilingQuery(DocumentsFilingRepository repository)
        {
            this.repository = repository;
        }
        #endregion Constructors

        #region Private Methods
        private DocumentsFilingPM GetSinglePM(int tenant, Expression<Func<POCO.DocumentsFiling, bool>> predicate)
        {
            DocumentsFilingPM extDocPm = GetPMList(predicate).FirstOrDefault();
            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }
        private List<DocumentsFilingPM> GetPMList(Expression<Func<POCO.DocumentsFiling, bool>> predicate)
        {
            return repository.GetMulti(predicate, a => new DocumentsFilingPM(a));
        }

        private void SetDocumentFollowUp(DocumentsFilingPM document, string followUpId = null)
        {
            //if (followUpId == null) followUpId = new Repository<FollowUp>.GetMulti(x => x.DocumentsFilingId == document.Id && x.Tenant == document.Tenant,a=>a.Id).FirstOrDefault();
            if (string.IsNullOrEmpty(followUpId)) return;

            document.FollowUpCount = 1;
            document.HasFollowUp = true;
            document.FollowUpId = followUpId;
        }


        #endregion Private Methods


        #region Public Get Single DocumentsFilingPM Methods
        public DocumentsFilingPM GetDocumentsFilingByDocumentId(string docId, int tenant) => GetSinglePM(tenant, a => a.DocumentId == docId && a.Tenant == tenant);
        public DocumentsFilingPM GetSinglePMByCode(string code, int tenant) => GetSinglePM(tenant, a => a.Code == code && a.Tenant == tenant);
        public DocumentsFilingPM GetDocumentsFilingByDocumentCode(string code, int tenant) => GetSinglePM(tenant, a => a.Code == code && a.Tenant == tenant);
        public DocumentsFilingPM GetSinglePM(string id, int tenant, bool checkOcr = false)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = Regex.Replace(id, " ", "+");
            }
            DocumentsFilingPM extDocPm = GetSinglePM(tenant, a => a.Id == id && a.Tenant == tenant);
            //todo vladi - check if this is needed and resolve followup

            //if (extDocPm != null)
            //{
            //    FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
            //    List<FollowUp> FollowUps = followUpRepository.GetFollowUps(tenant).ToList();
            //    if (FollowUps != null)
            //    {
            //        List<FollowUp> docFollowUp = FollowUps.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            //        if (docFollowUp.Count != 0)
            //        {
            //            extDocPm.FollowUpCount = docFollowUp.Count;
            //            extDocPm.FollowUpId = docFollowUp.FirstOrDefault().Id;
            //            extDocPm.HasFollowUp = docFollowUp.Any();
            //        }
            //    }
            //    if (checkOcr)
            //    {
            //        OcrDocumentRepository ocrDocumentRepository = new OcrDocumentRepository(tenant);
            //        extDocPm.OcrReference = ocrDocumentRepository.GetSingleByDocId(id, tenant)?.Reference;
            //    }

            //    SetDocumentFollowUp(extDocPm);
            //    DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            //    extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            //}
            return extDocPm;
        }
        public DocumentsFilingPM GetSinglePMByForwarderId(string id, int tenant) => GetSinglePM(tenant, a => a.ForwarderDocumentId == id && a.Tenant == tenant && a.IsDeleted == false);
        public DocumentsFilingPM GetSinglePMByCustomerId(string id, int tenant) => GetSinglePM(tenant, a => a.CustomerDocumentId == id && a.Tenant == tenant && a.IsDeleted == false);
        public DocumentsFilingPM GetSinglePMBySecurityId(string securityId, int tenant) => GetSinglePM(tenant, a => a.SecurityId == securityId && a.Tenant == tenant);
        public DocumentsFilingPM GetDocumentsFilingByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
            => GetSinglePM(tenant, a => a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ObjectTableId == objectTableId && a.EntityId == entityId && a.IsDeleted == false);
        public DocumentsFilingPM GetDocumentsFilingByChild(string documentTypeId, string paymentNumber, int tenant)
            => GetSinglePM(tenant, a => a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ChildEntityReference == paymentNumber && a.IsDeleted == false);
        public DocumentsFilingPM GetDocumentsFilingByChildId(string documentTypeId, string childId, string childObjectTableId, string childEntityReference, int tenant)
            => GetSinglePM(tenant, a => a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ChildEntityId == childId && a.ChildObjectTableId == childObjectTableId && a.ChildEntityReference == childEntityReference && a.IsDeleted == false);

        #endregion Public Get Single DocumentsFilingPM Methods


        public List<string> GetCOOEDocument(string declarationId, string certificateOfOriginId, int tenant)
        {
            var type = new Repository<POCO.DocumentType>(tenant).GetSingle(new DocumentTypeKeys<string>() { Id = "COOE" });
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            POCO.ObjectTable objectTable = objectTableRep.GetObjectTableByName("Customs.Declaration", tenant, true);
            POCO.ObjectTable objectTableCertificate = objectTableRep.GetObjectTableByName("Customs.CertificateOfOrigin", tenant, true);

            if (type != null)
            {
                if (type.ObjectTableId == objectTable.Id)
                {
                    string documentTypeId = type.Id;

                    var DocumentsFilingList = (from a in repository.GetByEntityAndChiled(objectTable.Id, declarationId, objectTableCertificate.Id, certificateOfOriginId, tenant, documentTypeId)
                                               select a.DocumentId);

                    if (!AmitalCloudSettings.GetAmitalCustomsSettingsMInject(tenant).IsConnectedToUniFreight)
                    {

                        return DocumentsFilingList.ToList();

                    }


                }
            }
            return null;
        }
    }

}
