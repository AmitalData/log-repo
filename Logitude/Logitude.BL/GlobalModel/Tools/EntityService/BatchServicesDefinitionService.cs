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
    public class BatchServicesDefinitionService
    {

        bool isNewEntity;
        private int tenant;
        public BatchServicesDefinition Poco { get; set; }
        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private BatchServicesDefinitionPM entityPm;
        private IGlobalContext objectContext;
        private BatchServicesDefinitionRepository entityRepository;

        public BatchServicesDefinitionService(IGlobalContext objectContext)
        {
            this.ObjectContext = objectContext;
            this.entityRepository = new BatchServicesDefinitionRepository(objectContext);
        }

        public void Create(BatchServicesDefinitionPM entityPM)
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                BatchServicesDefinitionMods ModsPoco = new BatchServicesDefinitionMods();
                this.isNewEntity = true;
                this.entityPm = entityPM;



                this.isNewEntity = true;
                this.entityPm = entityPM;
                this.Poco = new BatchServicesDefinition();


                this.Poco.Code = entityPM.Code;
                BatchServicesDefinitionMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                ModsPoco.Code = Poco.Code;
                ModsPoco.InActive = entityPM.InActive;
                ModsPoco.NumberOfThreads = entityPM.NumberOfThreads != null ? entityPM.NumberOfThreads : 1;
                BatchServicesDefinitionModsRepository ModsentityRepository = new BatchServicesDefinitionModsRepository(objectContext);
                ModsentityRepository.Add(ModsPoco);
                ModsentityRepository.SubmitChanges();
                scope.Complete();
            }

        }

        public void ModifyDefinitions(BatchServicesDefinitionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleBatchServicesDefinition(entityPM.Code);



            BatchServicesDefinitionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges(); 
        }

        public void Update(BatchServicesDefinitionPM entityPM)
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                BatchServicesDefinitionMods ModsPoco = new BatchServicesDefinitionMods();
                BatchServicesDefinitionModsRepository ModsentityRepository = new BatchServicesDefinitionModsRepository(objectContext);
                this.isNewEntity = false;
                this.entityPm = entityPM;

                this.Poco = entityRepository.GetSingleBatchServicesDefinition(entityPM.Code);



                BatchServicesDefinitionMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
                ModsPoco = ModsentityRepository.GetSingleBatchServicesDefinitionMod(Poco.Code); 
                ModsPoco.InActive = entityPM.InActive;
                ModsPoco.NumberOfThreads = entityPM.NumberOfThreads != null ? entityPM.NumberOfThreads : 1;

                ModsentityRepository.Update(ModsPoco);
                ModsentityRepository.SubmitChanges();
                scope.Complete();
            }
        }


    }
}
