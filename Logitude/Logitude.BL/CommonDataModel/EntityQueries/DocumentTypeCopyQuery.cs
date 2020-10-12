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
    public class DocumentTypeCopyQuery
    {
        DocumentTypeCopyRepository repository;

        public DocumentTypeCopyQuery()
        {
            repository = new DocumentTypeCopyRepository(); 
        }

        public DocumentTypeCopyQuery(int tenant)
        {
            repository = new DocumentTypeCopyRepository(tenant);
        }

        public DocumentTypeCopyQuery(DocumentTypeCopyRepository repository)
        {
            this.repository = repository;
        }

        public List<DocumentTypeCopyPM> GetDocumentTypeCopiesByDocumentType(string documentTypeId, string documentOutId, int tenant)
        {
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);

            List<DocumentTypeCopyPM> copies = (from a in repository.context.DocumentTypeCopies
                                               where a.DocumentTypeId == documentTypeId && a.Tenant == tenant
                                               select new DocumentTypeCopyPM()
                                               {
                                                   Code = a.Code,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Tenant = a.Tenant,
                                                   DocumentTypeId = a.DocumentTypeId,
                                                   IndexOrder = a.IndexOrder,
                                                   IsSelectedByDefault = a.IsSelectedByDefault,
                                                   InActive = a.InActive,
                                               }).ToList();

            if (documentOutId != null)
            {
                List<DocumentOutCopyPM> documentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(documentOutId, tenant);
                foreach (DocumentTypeCopyPM copy in copies)
                {
                    bool exists = (from a in documentOutCopies
                                   where a.DocumentTypeCopyId == copy.Id
                                   select a).Any();
                    copy.HasDocumentOutCopy = exists;
                }
            }

            return copies.Where(d => d.HasDocumentOutCopy || !d.InActive).ToList();
        }

        public List<DocumentTypeCopyPM> GetDocumentTypeCopiesByTenant(int tenant)
        {
            List<DocumentTypeCopyPM> copies = (from a in repository.context.DocumentTypeCopies
                                               where a.Tenant == tenant
                                               select new DocumentTypeCopyPM()
                                               {
                                                   InActive = a.InActive,
                                                   Code = a.Code,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Tenant = a.Tenant,
                                                   DocumentTypeId = a.DocumentTypeId,
                                                   IndexOrder = a.IndexOrder,
                                                   IsSelectedByDefault = a.IsSelectedByDefault,
                                               }).ToList();
            return copies;
        }


        public List<DocumentTypeCopyList> GetDocumentTypeCopiesByObjectTableId(string objectTableId,int tenant)
        {
            List<DocumentTypeCopyList> copies = (from a in repository.context.DocumentTypeCopies.Include("DocumentType")
                                               where a.Tenant == tenant  && (a.DocumentType!=null && a.DocumentType.ObjectTableId == objectTableId ) && !a.InActive
                                               select new DocumentTypeCopyList()
                                               {
                                                   InActive = a.InActive,
                                                   Code = a.Code,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Tenant = a.Tenant,
                                                   DocumentTypeId = a.DocumentTypeId,
                                               }).ToList();
            return copies;
        }


        public List<DocumentTypeCopyList> GetDocumentTypeCopyListsBydocumentTypeCopyIds(List<string> documentTypeCopyIds, int tenant)
        {

            List<DocumentTypeCopyList> documentTypeCopyLists = (from a in repository.context.DocumentTypeCopies
                                                    where a.Tenant == tenant && documentTypeCopyIds.Contains(a.Id)
                                                    select new DocumentTypeCopyList()
                                                    {
                                                        Id = a.Id,
                                                        Name = a.Name,
                                                        DocumentTypeId = a.DocumentTypeId,
                                                        
                                                    }).ToList();




            return documentTypeCopyLists;
        }




    }
}
