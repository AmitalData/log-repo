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
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class BranchService
    {
        bool isNewEntity;
        private int tenant;
        public Branch Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private BranchPM entityPm;
        private ICommonDataContext objectContext;
        private BranchRepository entityRepository;
        public BranchService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new BranchRepository(objectContext);
        }

        public void Create(BranchPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Branch", tenant).ToString();
            this.Poco = new Branch();
            this.Poco.Id = this.entityPm.Id;

            BranchValidating.Validate(entityPM);
            if (!entityPm.IsHybrid)
            {
                BranchTracing.Trace(entityPM, Poco, isNewEntity);

            }
            BranchMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(BranchPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleBranch(entityPM.Id, entityPm.Tenant);

            BranchValidating.Validate(entityPM);
            if (!entityPm.IsHybrid)
            {
                BranchTracing.Trace(entityPM, Poco, isNewEntity);
            }
            BranchMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }   
           
    }
}