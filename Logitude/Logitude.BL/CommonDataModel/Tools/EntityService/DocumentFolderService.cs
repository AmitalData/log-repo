using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
   public class DocumentFolderService
    {
         bool isNewEntity;
        private int tenant;
        public DocumentFolder Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentFolderPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentFolderRepository entityRepository;
        public DocumentFolderService(ICommonDataContext objectContext,int tenant)
        {
          
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentFolderRepository(objectContext);
        }

        public void Create(DocumentFolderPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;

            this.entityPM.Id = IdCounter.GetNumber("DocumentFolder", tenant).ToString();
            this.Poco = new DocumentFolder();
            this.Poco.Id = this.entityPM.Id;

            DocumentFolderMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentFolderPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleDocumentFolder(entityPM.Id);
            DocumentFolderMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
