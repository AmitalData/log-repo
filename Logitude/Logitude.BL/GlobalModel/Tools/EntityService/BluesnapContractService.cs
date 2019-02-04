using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class BluesnapContractService
    {
        bool isNewEntity;
        private int tenant;
        public BluesnapContract Poco { get; set; }
        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private BluesnapContractPM entityPm;
        private IGlobalContext objectContext;
        private BluesnapContractRepository entityRepository;

        public BluesnapContractService(IGlobalContext objectContext, int tenant)
        {         
            this.ObjectContext = objectContext;
            this.entityRepository = new BluesnapContractRepository(objectContext);
        }

        public void Create(BluesnapContractPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("BluesnapContract", tenant).ToString();

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = "";
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                    scope.Complete();
                }

                msg = msg.Replace("%Entity", "Bluesnap Contract");
                throw new ApplicationException(msg);
            }
            else
            {
                this.isNewEntity = true;
                this.entityPm = entityPM;
                this.Poco = new BluesnapContract();
                this.Poco.Id = this.entityPm.Id;

                BluesnapContractTracing.Trace(entityPM, Poco, isNewEntity);
                this.Poco.Code = entityPM.Code;
                BluesnapContractMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
        }

        public void Update(BluesnapContractPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleBluesnapContract(entityPM.Id, 0);

            string entityName = "BluesnapContract" + entityPM.Id + 0;
            string entityPmName = "BluesnapContractPM" + entityPM.Id + 0;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            BluesnapContractTracing.Trace(entityPM, Poco, isNewEntity);
            BluesnapContractMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

                myResult = (from a in entityRepository.GetBluesnapContracts(0)
                            where a.Code == entityPm.Code
                            select a).Any();            

            return myResult;
        }
    }
}
