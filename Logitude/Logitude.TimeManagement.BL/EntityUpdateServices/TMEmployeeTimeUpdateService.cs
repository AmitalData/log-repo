using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityQueryServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{
    public partial class TMEmployeeTimeUpdateService
    {
        protected override void OnCreating(TMEmployeeTimePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TMEmployeeTime", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.FullDuration = entityPM.TimeInMinutes;
                entityPM.NeedsProrating = true;
                SendQueueMessage(entityPM);
            }
        }

        protected override void OnUpdating(TMEmployeeTimePM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }
            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdateDate = myDate;
            if (entityPM.CreatedByUserId == null)
            {
                entityPM.UpdatedByUserId = myLoggedUserId;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

        protected override void OnUpdating(EntityPMs.TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                if (entityPM.TimeInMinutes != entityPOCO.TimeInMinutes)
                {
                    if (entityPM.ProratedDuration == 0)
                    {
                        entityPM.FullDuration = entityPM.TimeInMinutes;
                    }

                    entityPM.NeedsProrating = true;
                    SendQueueMessage(entityPM);
                }
            }
        }

        protected override void AfterUpdating(TMEmployeeTimePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);

        }

        private void SendQueueMessage(TMEmployeeTimePM entityPM)
        {
            string queueName = "timemanagementqueue";
            int tenant = entityPM.Tenant;
            string wINumber = entityPM.WINumber;
            double completedWork = 0;

            if (!string.IsNullOrEmpty(wINumber))
            {
                ITimeManagementContext context = this.MainContext as TimeManagementContext;
                var list = (from d in context.TMEmployeeTimes where d.Tenant == tenant && d.WINumber == wINumber && d.Id != entityPM.Id select d).ToList();

                if (list != null)
                {
                    var minutes = list.Sum(s => s.TimeInMinutes);
                    completedWork = (minutes + entityPM.TimeInMinutes) / 60.00;
                }
                else
                {
                    completedWork = entityPM.TimeInMinutes/ 60.00;
                }

                try
                {
                    DbQueueService queueservice = new DbQueueService(queueName, tenant);
                    Dictionary<string, string> message = new Dictionary<string, string>()
                    {
                        { "Tenant", tenant.ToString() },
                        { "WorkItemNumber",  wINumber },
                        { "CompletedWork", completedWork.ToString("0.##")},
                    };

                    queueservice.Send(message);
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
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "TMEmployeeTimeUpdateService SendQueueMessage() Method", null, ip);
                    throw;
                }

            }
        }
    }
}
