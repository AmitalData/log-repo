using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class WorkerRoleNameQuery
    {
        WorkerRoleNameRepository repository;

        public WorkerRoleNameQuery()
        {
            repository = new WorkerRoleNameRepository();
        }

        public WorkerRoleNameQuery(int tenant)
        {
            repository = new WorkerRoleNameRepository(tenant);
        }

        public WorkerRoleNameQuery(WorkerRoleNameRepository WorkerRoleNameRepository)
        {
            repository = WorkerRoleNameRepository;
        }

        public WorkerRoleNamePM GetSinglePM(string name)
        {
            WorkerRoleNamePM entity;
            entity = (from a in repository.context.WorkerRoleNames
                      where a.Name == name
                      select new WorkerRoleNamePM()
                      {
                          Name = a.Name,
                          CreateDate = a.CreateDate,
                          WaitingStatus = a.WaitingStatus
                      }).FirstOrDefault();

            return entity;
        }

        public IQueryable<WorkerRoleNameList> GetIQueryableEntityList(IQueryable<WorkerRoleName> iQueryable)
        {
            IQueryable<WorkerRoleNameList> result = from a in iQueryable
                                                       select new WorkerRoleNameList()
                                                       {
                                                           Name = a.Name,
                                                           CreateDate = a.CreateDate,
                                                           WaitingStatus = a.WaitingStatus
                                                       };
            return result;
        }

        public int GetLatestWaitingStatus()
        {
            int latestWaitingStatus = (from a in repository.context.WorkerRoleNames.OrderByDescending(w => w.CreateDate)
                                 select a.WaitingStatus).FirstOrDefault();
            return latestWaitingStatus;
        }

        public int GetLatestWaitingStatusAfterCheckIfNotExist(string name)
        {
            if (GetSinglePM(name) == null)
            {
                int latestWaitingStatus = GetLatestWaitingStatus();
                if (latestWaitingStatus > -102)
                    latestWaitingStatus = -1001;
                return latestWaitingStatus;
            }
            return 1;
        }
    }
}
