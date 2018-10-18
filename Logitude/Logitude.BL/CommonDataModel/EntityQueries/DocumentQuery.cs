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
    public class DocumentQuery
    {
        DocumentRepository repository;

        public DocumentQuery()
        {
            repository = new DocumentRepository(); 
        }

        public DocumentQuery(int tenant)
        {
            repository = new DocumentRepository(tenant);
        }

        public DocumentQuery(DocumentRepository repository)
        {
            this.repository = repository;
        }

        public DocumentPM GetSinglePM(int tenant, string id)
        {
            Document entity = repository.GetSingleDocument(tenant, id);
            DocumentPM pm = null;

            if (entity != null)
            {
                pm = new DocumentPM()
                {
                };
            }
            return pm;
        }
    }
}