using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentFolderQuery
    {
        DocumentFolderRepository repository;

        public DocumentFolderQuery()
        {
            repository = new DocumentFolderRepository(); 
        }

        public DocumentFolderQuery(int tenant)
        {
            repository = new DocumentFolderRepository(tenant);
        }

        public DocumentFolderQuery(DocumentFolderRepository repository)
        {
            this.repository = repository;
        }

        public DocumentFolderPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.DocumentFolders
                    where a.Id == id
                    select new DocumentFolderPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        EnglishName = a.EnglishName,
                        LocalName = a.LocalName,
                        IsExternalFolder = a.IsExternalFolder,
                        ParentFolderId = a.ParentFolderId,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public DocumentFolderPM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.DocumentFolders
                    where a.Code == code
                    select new DocumentFolderPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        EnglishName = a.EnglishName,
                        LocalName = a.LocalName,
                        IsExternalFolder = a.IsExternalFolder,
                        ParentFolderId = a.ParentFolderId,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<DocumentFolderPM> GetDocumentFolderPMs()
        {
            return from a in repository.context.DocumentFolders
                   select new DocumentFolderPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Code = a.Code,
                       EnglishName = a.EnglishName,
                       LocalName = a.LocalName,
                       IsExternalFolder = a.IsExternalFolder,
                       ParentFolderId = a.ParentFolderId,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<DocumentFolderList> GetIQueryableEntityList(IQueryable<DocumentFolder> iQueryable)
        {
            IQueryable<DocumentFolderList> result = from a in iQueryable
                                                    select new DocumentFolderList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        Code = a.Code,
                                                        EnglishName = a.EnglishName,
                                                        LocalName = a.LocalName,
                                                        IsExternalFolder = a.IsExternalFolder,
                                                        ParentFolderId = a.ParentFolderId,
                                                        SearchFields = a.SearchFields,
                                                    };
            return result;
        }
    }
}
