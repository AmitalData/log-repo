using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerSizeService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerSize Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerSizePM entityPm;
        private ICommonDataContext objectContext;
        private CustomerSizeRepository entityRepository;

        public CustomerSizeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerSizeRepository(objectContext);
        }

        public void Create(CustomerSizePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CustomerSize", tenant).ToString();
            this.Poco = new CustomerSize();
            this.Poco.Id = this.entityPm.Id;

            CustomerSizeTracing.Trace(entityPM, Poco, isNewEntity);
            CustomerSizeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerSizePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerSize(entityPM.Id, entityPm.Tenant);

            CustomerSizeTracing.Trace(entityPM, Poco, isNewEntity);
            CustomerSizeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }   
    }
}
