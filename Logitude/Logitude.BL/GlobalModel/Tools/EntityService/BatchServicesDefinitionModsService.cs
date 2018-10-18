using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
  public  class BatchServicesDefinitionModsService
    {

        bool isNewEntity;
        private int tenant;
        public BatchServicesDefinitionMods Poco { get; set; }
        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private BatchServicesDefinitionModsPM entityPm;
        private IGlobalContext objectContext;
        private BatchServicesDefinitionModsRepository entityRepository;

        public BatchServicesDefinitionModsService(IGlobalContext objectContext)
        {         
            this.ObjectContext = objectContext;
            this.entityRepository = new BatchServicesDefinitionModsRepository(objectContext);
        }

        public void Create(BatchServicesDefinitionModsPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

        

                this.isNewEntity = true;
                this.entityPm = entityPM;
                this.Poco = new BatchServicesDefinitionMods();

         
                this.Poco.Code = entityPM.Code;
                BatchServicesDefinitionModsMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            
        }

        public void Update(BatchServicesDefinitionModsPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleBatchServicesDefinitionMod(entityPM.Code);

       

            BatchServicesDefinitionModsMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }


    }
}
