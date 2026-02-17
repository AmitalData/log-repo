using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
   public class CustomerTenantAccessCardsBatchService
   {
       private Tenant loggedTenant;
        bool isNewEntity;
        private int tenant;
        public CustomerTenantAccessCardsBatch Poco { get; set; }
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private CustomerTenantAccessCardsBatch entity;
        private CustomerTenantAccessCardsBatchPM entityPM;
        private ICommonDataContext objectContext;
        private CustomerTenantAccessCardsBatchRepository entityRepository;
        public CustomerTenantAccessCardsBatchService(ICommonDataContext objectContext, int tenant, CustomerTenantAccessCardsBatchPM entityPM)
        {
            this.tenant = tenant;
            this.entityPM = entityPM;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerTenantAccessCardsBatchRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
        }

        public CustomerTenantAccessCardsBatchService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerTenantAccessCardsBatchRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
        }

        public void Create(CustomerTenantAccessCardsBatchPM EntityPM)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.entityPM = EntityPM;
                this.isNewEntity = true;

                this.entityPM.BatchNumber = CodeCounter.GetNumber("CustomerTenantAccessCardsBatch", tenant).ToString();
                this.Poco = new CustomerTenantAccessCardsBatch();
                this.Poco.BatchNumber = entityPM.BatchNumber;
                CustomerTenantAccessCardsBatchMapping.MapEntity(entityPM, Poco, true, loggedTenant);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "CustomerId", Poco.CustomerId.ToString() }, { "CustomerTenantAccessId", Poco.CustomerTenantAccessId.ToString() }, { "tenant", tenant.ToString() }, { "BatchNumber", Poco.BatchNumber }, { "CorrelationId", Guid.NewGuid().ToString() } });

                scope.Complete();
            }
        }


        public void Create()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.isNewEntity = true;

                this.entityPM.BatchNumber = CodeCounter.GetNumber("CustomerTenantAccessCardsBatch", tenant).ToString();
                this.Poco = new CustomerTenantAccessCardsBatch();
                this.Poco.BatchNumber = entityPM.BatchNumber;
                CustomerTenantAccessCardsBatchMapping.MapEntity(entityPM, Poco, true, loggedTenant);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "CustomerId", Poco.CustomerId.ToString() }, { "CustomerTenantAccessId", Poco.CustomerTenantAccessId.ToString() }, { "tenant", tenant.ToString() }, { "BatchNumber", Poco.BatchNumber }, { "CorrelationId", Guid.NewGuid().ToString() } });

                scope.Complete();
            }
        }

        public void Delete()
        {
            this.Poco = entityRepository.GetSingleCustomerTenantAccessCardsBatch(entityPM.CustomerTenantAccessId,entityPM.CustomerId,entityPM.BatchNumber,entityPM.Tenant); 
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleCustomerTenantAccessCardsBatch(entityPM.CustomerTenantAccessId, entityPM.CustomerId, entityPM.BatchNumber, entityPM.Tenant);
            CustomerTenantAccessCardsBatchMapping.MapEntity(entityPM, Poco,false, loggedTenant);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
      
        }

        public void Update(CustomerTenantAccessCardsBatchPM EntityPM)
        {
            this.entityPM = EntityPM;
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleCustomerTenantAccessCardsBatch(entityPM.CustomerTenantAccessId, entityPM.CustomerId, entityPM.BatchNumber, entityPM.Tenant);
            CustomerTenantAccessCardsBatchMapping.MapEntity(entityPM, Poco, false, loggedTenant);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }






    }
}
