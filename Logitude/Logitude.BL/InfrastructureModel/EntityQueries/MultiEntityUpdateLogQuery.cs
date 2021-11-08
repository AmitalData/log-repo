using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class MultiEntityUpdateLogQuery
    {
        MultiEntityUpdateLogRepository repository;

        public MultiEntityUpdateLogQuery()
        {
            repository = new MultiEntityUpdateLogRepository();
        }

        public MultiEntityUpdateLogQuery(int tenant)
        {
            repository = new MultiEntityUpdateLogRepository(tenant);
        }

        public MultiEntityUpdateLogQuery(MultiEntityUpdateLogRepository multiEntityUpdateLogRepository)
        {
            repository = multiEntityUpdateLogRepository;
        }

        public MultiEntityUpdateLogPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.MultiEntityUpdateLogs
                    where a.Id == id && a.Tenant == tenant
                    select new MultiEntityUpdateLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate =a.CreateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        StatusCode = a.StatusCode,
                        ExceptionMessage = a.ExceptionMessage,
                        DoneDate = a.DoneDate,
                        XMLData = a.XMLData,
                        ObjectTableId = a.ObjectTableId,
                        RetryNumber = a.RetryNumber,
                        UpdatedEntitiesNumber = a.UpdatedEntitiesNumber,
                        StartDate = a.StartDate,

                    }).FirstOrDefault();
        }

    }
}
