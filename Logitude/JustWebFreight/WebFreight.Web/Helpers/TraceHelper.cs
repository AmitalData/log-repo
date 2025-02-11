using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace WebFreight.Web.Helpers
{
    public class TraceHelper
    {
        public static void Create(int tenant, string entityId, string tableName, string notes, string eventTypeCode, string loggedUserEmail)
        {
            User loggedUser = new UserRepository(tenant).GetSingleUserByEmail(loggedUserEmail, tenant, true);
            EventTypePM eventTypePM = new EventTypeQuery(tenant).GetSinglePMByCode(eventTypeCode, tenant);
            ObjectTablePM table = new ObjectTableQuery(tenant).GetObjectTableByName(tableName, tenant);
            TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

            TraceEvent newEvent = new TraceEvent()
            {
                Id = IdCounter.GetNumber("TraceEvent", tenant).ToString(),
                LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                Tenant = tenant,
                UserId = loggedUser.Id,
                ObjectTableId = table.Id,
                EntityId = entityId,
                IsAddedManually = false,
                EventTypeId = eventTypePM.Id,
                Notes = notes
            };
            traceEventRepository.Add(newEvent);
            traceEventRepository.SubmitChanges();

        }

        public class EventTypeCodes
        {
            public static readonly string Created = "CREV";
            public static readonly string Updated = "UPEV";
        }
    }
}