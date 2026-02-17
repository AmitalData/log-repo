using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
    public class DocumentFilingBackupBatchService
    {
        private Tenant loggedTenant;
        bool isNewEntity;
        private int tenant;
        public DocumentFilingBackupBatch Poco { get; set; }
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private DocumentFilingBackupBatch entity;
        private DocumentFilingBackupBatchPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentFilingBackupBatchRepository entityRepository;
        public DocumentFilingBackupBatchService(ICommonDataContext objectContext, int tenant, DocumentFilingBackupBatchPM entityPM)
        {
            this.tenant = tenant;
            this.entityPM = entityPM;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentFilingBackupBatchRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
        }

        public DocumentFilingBackupBatchService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentFilingBackupBatchRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
        }

        public void Create(DocumentFilingBackupBatchPM EntityPM)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                this.entityPM = EntityPM;

                DocumentFilingBackupSettingRepository documentFilingBackupSettingRepository = new DocumentFilingBackupSettingRepository(this.entityPM.Tenant);


                if (!documentFilingBackupSettingRepository.IsDocumentFilingBackupSettingActive(this.entityPM.Tenant))
                {
                    throw new Exception("Document filing backup setting not active");
                }

                this.isNewEntity = true;

                this.entityPM.BatchNumber = CodeCounter.GetNumber("DocumentFilingBackupBatch", tenant).ToString();
                this.entityPM.Id = IdCounter.GetNumber("DocumentFilingBackupBatch", tenant).ToString();

                this.Poco = new DocumentFilingBackupBatch();
                this.Poco.BatchNumber = entityPM.BatchNumber;

                //DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                //if (EntityPM.FromDatetime != null && EntityPM.ToDatetime != null)
                //{
                //    List<string> documentIds = documentsFilingQuery.GetDocumentFilingIdsByTenantCreateDate(tenant, EntityPM.FromDatetime.Value, EntityPM.ToDatetime.Value, EntityPM.IncludeBackedUp);
                //    entityPM.TotalDocuments = documentIds.Count;
                //}


                DocumentFilingBackupBatchMapping.MapEntity(entityPM, Poco, true, loggedTenant);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFilingBackupQueueBuilderQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "BatchId", Poco.Id.ToString() }, { "Tenant", tenant.ToString() } });

                scope.Complete();
            }
        }


        public void Create()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.isNewEntity = true;

                this.entityPM.BatchNumber = CodeCounter.GetNumber("DocumentFilingBackupBatch", tenant).ToString();
                this.Poco = new DocumentFilingBackupBatch();
                this.Poco.BatchNumber = entityPM.BatchNumber;
                //DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                //if (entityPM.FromDatetime != null && entityPM.ToDatetime != null)
                //{
                //    List<string> documentIds = documentsFilingQuery.GetDocumentFilingIdsByTenantCreateDate(tenant, entityPM.FromDatetime.Value, entityPM.ToDatetime.Value, entityPM.IncludeBackedUp);
                //    entityPM.TotalDocuments = documentIds.Count;
                //}
                DocumentFilingBackupBatchMapping.MapEntity(entityPM, Poco, true, loggedTenant);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFilingBackupQueueBuilderQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "BatchId", Poco.Id.ToString() }, { "Tenant", tenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() } });

                scope.Complete();
            }
        }

        public void Delete()
        {
            this.Poco = entityRepository.GetSingleDocumentFilingBackupBatch(entityPM.Id, entityPM.Tenant);
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleDocumentFilingBackupBatch(entityPM.Id, entityPM.Tenant);
            DocumentFilingBackupBatchMapping.MapEntity(entityPM, Poco, false, loggedTenant);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DocumentFilingBackupBatchPM EntityPM)
        {
            this.entityPM = EntityPM;
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleDocumentFilingBackupBatch(entityPM.Id, entityPM.Tenant);
            DocumentFilingBackupBatchMapping.MapEntity(entityPM, Poco, false, loggedTenant);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }






    }
}
