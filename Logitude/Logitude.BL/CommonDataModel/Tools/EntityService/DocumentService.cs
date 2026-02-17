using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentService
    {
        bool isNewEntity;
        private int tenant;
        public Document Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private Document entity;
        private ICommonDataContext objectContext;
        private DocumentRepository entityRepository;
        public DocumentService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentRepository(objectContext);
        }

        public void Create(Document entity)
        {
            this.isNewEntity = true;

            this.entity.Id = IdCounter.GetNumber("Document", tenant).ToString();
            this.Poco = new Document();
            this.Poco.Id = this.entity.Id;
   
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(Document entity)
        {
            this.isNewEntity = false;         
            entityRepository.Update(entity);
            entityRepository.SubmitChanges();
        }
    }
}
