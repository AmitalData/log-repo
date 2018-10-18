using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
  public  class CustomerTenantAccessRequestService
    {

       bool isNewEntity;
        private int tenant;
        public CustomerTenantAccessRequest Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerTenantAccessRequestPM entityPM;
        private ICommonDataContext objectContext;
        private CustomerTenantAccessRequestRepository entityRepository;
        public CustomerTenantAccessRequestService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerTenantAccessRequestRepository(objectContext);
        }

        public void Create(CustomerTenantAccessRequestPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("CustomerTenantAccessRequest", tenant).ToString();
            this.Poco = new CustomerTenantAccessRequest();
            this.Poco.Id = this.entityPM.Id;
            entityPM.RequestDateTime =TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.RequestStatus = "N";
            CustomerTenantAccessRequestMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerTenantAccessRequestPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCustomerTenantAccessRequest(entityPM.Id, tenant);
            CustomerTenantAccessRequestMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            if (entityPM.RequestStatus.ToUpper() == "W")
            {
            try
            {
                IQueueService queue = new DbQueueService();
                queue.InitializeQueue("CustomerTenantAccessRequestQueue", 0);
                queue.Send(new Dictionary<string, string>() { { "RequestId", entityPM.Id }, { "Tenant", entityPM.Tenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() } });
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CustomerTenantAccessRequest Role", null, ip);
            }
        }
        }
    }
}
