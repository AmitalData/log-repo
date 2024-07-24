using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentOutCopyQuery
    {
        DocumentOutCopyRepository repository;

        public DocumentOutCopyQuery()
        {
            repository = new DocumentOutCopyRepository(); 
        }

        public DocumentOutCopyQuery(int tenant)
        {
            repository = new DocumentOutCopyRepository(tenant);
        }

        public DocumentOutCopyQuery(DocumentOutCopyRepository repository)
        {
            this.repository = repository;
        }

        public DocumentOutCopyPM GetDocumentOutCopyByDocumentId(string documentId, int tenant)
        {
            return (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType").Include("Document").Include("LastPrintedByUser.Contact")
                    where a.DocumentId == documentId && a.Tenant == tenant
                    select new DocumentOutCopyPM()
                    {
                        DocumentId = a.DocumentId,
                        DocumentOutId = a.DocumentOutId,
                        DocumentTypeCopyId = a.DocumentTypeCopyId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        DocoumentTypeCopyName = a.DocumentTypeCopy.Name,
                        DocumentTypeCopyNameWithDocumentTypeName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                        // DocumentTypeCopyNameWithDocumentTypeName = a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                        FileName = a.Document != null ? a.Document.FileName : null,
                        FileSize = a.Document != null ? a.Document.FileSize : null,
                        LastPrintDate = a.LastPrintDate,
                        LastPrintedByUserId = a.LastPrintedByUserId,
                        LastPrintedByUserName = a.LastPrintedByUser != null ? (a.LastPrintedByUser.Contact != null ? a.LastPrintedByUser.Contact.EnglishName : null) : null,
                        CalculatedFileName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                    }).FirstOrDefault();
        }

        public List<DocumentOutCopyPM> GetDocumentOutCopiesForDocumentOut(string documentOutId, int tenant)
        {
            List<DocumentOutCopyPM> copies = (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType").Include("Document").Include("LastPrintedByUser.Contact")
                                              where a.DocumentOutId == documentOutId && a.Tenant == tenant
                                              select new DocumentOutCopyPM()
                                              {
                                                  DocumentId = a.DocumentId,
                                                  DocumentOutId = a.DocumentOutId,
                                                  DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  DocoumentTypeCopyName = a.DocumentTypeCopy.Name,
                                                  DocumentTypeCopyNameWithDocumentTypeName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  // DocumentTypeCopyNameWithDocumentTypeName = a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  FileName = a.Document != null ? a.Document.FileName : null,
                                                  FileSize = a.Document != null ? a.Document.FileSize : null,
                                                  LastPrintDate = a.LastPrintDate,
                                                  LastPrintedByUserId = a.LastPrintedByUserId,
                                                  LastPrintedByUserName = a.LastPrintedByUser != null ? (a.LastPrintedByUser.Contact != null ? a.LastPrintedByUser.Contact.EnglishName : null) : null,
                                                  CalculatedFileName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                              }).ToList();
            return copies;
        }
        public DocumentOutCopyPM GetDocumentOutCopiesForDocumentOutAndType(string documentOutId, int tenant,string code)
        {
            DocumentOutCopyPM copies = (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType").Include("Document").Include("LastPrintedByUser.Contact")
                                              where a.DocumentOutId == documentOutId && a.Tenant == tenant && a.DocumentTypeCopy.Code== code
                                              select new DocumentOutCopyPM()
                                              {
                                                  DocumentId = a.DocumentId,
                                                  DocumentOutId = a.DocumentOutId,
                                                  DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  DocoumentTypeCopyName = a.DocumentTypeCopy.Name,
                                                  DocumentTypeCopyNameWithDocumentTypeName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  // DocumentTypeCopyNameWithDocumentTypeName = a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  FileName = a.Document != null ? a.Document.FileName : null,
                                                  FileSize = a.Document != null ? a.Document.FileSize : null,
                                                  LastPrintDate = a.LastPrintDate,
                                                  LastPrintedByUserId = a.LastPrintedByUserId,
                                                  LastPrintedByUserName = a.LastPrintedByUser != null ? (a.LastPrintedByUser.Contact != null ? a.LastPrintedByUser.Contact.EnglishName : null) : null,
                                                  CalculatedFileName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                              }).FirstOrDefault();
            return copies;
        }


        public List<DocumentOutCopyList> GeDocumentOutCopiesListsBydocumentTypeCopyIdsAndDocumentOutIds(List<string> documentTypeCopyIds, List<string> documentOutIds, int tenant)
        {

            List<DocumentOutCopyList> documentOutCopyLists = (from a in repository.context.DocumentOutCopies
                                                               where a.Tenant == tenant && documentTypeCopyIds.Contains(a.DocumentTypeCopyId) && documentOutIds.Contains(a.DocumentOutId)
                                                              select new DocumentOutCopyList()
                                                                {
                                                                  Id = a.Id,
                                                                  DocumentOutId = a.DocumentOutId,
                                                                  DocumentId = a.DocumentId,
                                                                  DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                                  Tenant = a.Tenant,

                                                              }).ToList();




            return documentOutCopyLists;
        }


        public List<DocumentOutCopyPM> GeDocumentOutCopiesPMListsBydocumentTypeCopyIdsAndDocumentOutIds(List<string> documentTypeCopyIds, List<string> documentOutIds, int tenant)
        {
            List<DocumentOutCopyPM> copies = (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType").Include("Document").Include("LastPrintedByUser.Contact")
                                              where a.Tenant == tenant && documentTypeCopyIds.Contains(a.DocumentTypeCopyId) && documentOutIds.Contains(a.DocumentOutId)
                                              select new DocumentOutCopyPM()
                                              {
                                                  DocumentId = a.DocumentId,
                                                  DocumentOutId = a.DocumentOutId,
                                                  DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  DocoumentTypeCopyName = a.DocumentTypeCopy.Name,
                                                  DocumentTypeCopyNameWithDocumentTypeName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  // DocumentTypeCopyNameWithDocumentTypeName = a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  FileName = a.Document != null ? a.Document.FileName : null,
                                                  FileSize = a.Document != null ? a.Document.FileSize : null,
                                                  LastPrintDate = a.LastPrintDate,
                                                  LastPrintedByUserId = a.LastPrintedByUserId,
                                                  LastPrintedByUserName = a.LastPrintedByUser != null ? (a.LastPrintedByUser.Contact != null ? a.LastPrintedByUser.Contact.EnglishName : null) : null,
                                                  CalculatedFileName = a.Document != null && !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.DocumentOut.DocumentsFiling.DocumentType.Name != a.DocumentTypeCopy.Name ? a.DocumentOut.DocumentsFiling.DocumentType.Name + " - " + a.DocumentTypeCopy.Name : a.DocumentTypeCopy.Name,
                                                  DocumentTypeId = a.DocumentOut != null && a.DocumentOut.DocumentsFiling != null ? a.DocumentOut.DocumentsFiling.DocumentType.Id : null,
                                              }).ToList();
            return copies;
        }





        public DocumentOutCopyList GeDocumentOutCopyBydocumentTypeCopyAndDocumentOutId(string documentTypeCopyId, string documentOutId, int tenant)
        {

           DocumentOutCopyList documentOutCopy = (from a in repository.context.DocumentOutCopies
                                                              where a.Tenant == tenant &&   a.DocumentTypeCopyId  == documentTypeCopyId  && a.DocumentOutId == documentOutId
                                                              select new DocumentOutCopyList()
                                                              {
                                                                  Id = a.Id,
                                                                  DocumentOutId = a.DocumentOutId,
                                                                  DocumentId = a.DocumentId,
                                                                  DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                                  Tenant = a.Tenant
                                                              }).FirstOrDefault();




            return documentOutCopy;
        }




        public DocumentOutCopyPM GeDocumentOutCopyPMByDocumentOutId(string documentOutId, int tenant)
        {

            DocumentOutCopyPM documentOutCopy = (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy")
                                                   where a.Tenant == tenant && (a.DocumentTypeCopy.IsSelectedByDefault == true && a.DocumentTypeCopy.InActive == false)  && a.DocumentOutId == documentOutId
                                                   select new DocumentOutCopyPM()
                                                   {
                                                       Id = a.Id,
                                                       DocumentOutId = a.DocumentOutId,
                                                       DocumentId = a.DocumentId,
                                                       DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                       Tenant = a.Tenant,
                                                   }).FirstOrDefault();

            return documentOutCopy;
        }

        public DocumentOutCopyPM GeDocumentOutCopyPMByDocumentOutIdAndDocumentCopyId(string documentOutId, string documentCopyId, int tenant)
        {

            DocumentOutCopyPM documentOutCopy = (from a in repository.context.DocumentOutCopies.Include("DocumentTypeCopy")
                                                 where a.Tenant == tenant
                                                 && a.DocumentTypeCopy.InActive == false
                                                 && a.DocumentOutId == documentOutId && a.DocumentTypeCopyId == documentCopyId
                                                 select new DocumentOutCopyPM()
                                                 {
                                                     Id = a.Id,
                                                     DocumentOutId = a.DocumentOutId,
                                                     DocumentId = a.DocumentId,
                                                     DocumentTypeCopyId = a.DocumentTypeCopyId,
                                                     Tenant = a.Tenant,
                                                 }).FirstOrDefault();

            return documentOutCopy;
        }
    }
}