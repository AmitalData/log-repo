using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class HelpResourceService
    {
        bool isNewEntity;
        private int tenant;
        public HelpResource Poco { get; set; }
        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private HelpResourcePM entityPm;
        private IGlobalContext objectContext;
        private HelpResourceRepository entityRepository;

        public HelpResourceService(IGlobalContext objectContext, int tenant)
        {
            this.ObjectContext = objectContext;
            this.entityRepository = new HelpResourceRepository(objectContext);
        }

        public void Create(HelpResourcePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            //this.entityPm.Id = IdCounter.GetNumber("HelpResource", tenant).ToString();

            //bool exist = this.IsEntityExists();

            //if (exist)
            //{
                string msg = "";
                //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                //{
                //    msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                //    scope.Complete();
                //}

            //    msg = msg.Replace("%Entity", "Bluesnap Contract");
            //    throw new ApplicationException(msg);
            //}
            //else
            //{
            //    this.isNewEntity = true;
            //    this.entityPm = entityPM;
            //    this.Poco = new HelpResource();
                //this.Poco.Id = this.entityPm.Id;

                //HelpResourceTracing.Trace(entityPM, Poco, isNewEntity);
                //this.Poco.Code = entityPM.Code;
                //HelpResourceMapping.MapEntity(entityPM, Poco, isNewEntity);
                //entityRepository.Add(Poco);
                //entityRepository.SubmitChanges();
            //}
        }

        public void Update(HelpResourcePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            //this.Poco = entityRepository.GetSingleHelpResource(entityPM.Id, 0);

            //string entityName = "HelpResource" + entityPM.Id + 0;
            //string entityPmName = "HelpResourcePM" + entityPM.Id + 0;

            //if (CacheManager.CacheWrapper.Get(entityName) != null)
            //{
            //    CacheManager.CacheWrapper.Invalidate(entityName);
            //}

            //if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            //{
            //    CacheManager.CacheWrapper.Invalidate(entityPmName);
            //}

            //HelpResourceTracing.Trace(entityPM, Poco, isNewEntity);
            //HelpResourceMapping.MapEntity(entityPM, Poco, isNewEntity);
            //entityRepository.Update(Poco);
            //entityRepository.SubmitChanges();
        }
        
    }
}
