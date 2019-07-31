 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TimeManagement.Data.Repositories
{
    public partial class TMEmployeeTimeRepository : IRepository<TMEmployeeTime>
    {

        public List<TMEmployeeTime> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public IQueryable<TMEmployeeTime> GetMulti(string myProjectId, string myDescription, string myWINumber, int tenant)
        {
            return (from d in context.TMEmployeeTimes
                    where d.Tenant == tenant
                    && d.ProjectId == myProjectId
                    && d.Description == myDescription
                    && d.WINumber == myWINumber
                    select d);
        }


        public TMEmployeeTime GetSingleByPrjectandEmployeeandWIandDescription(string myProjectId, string myDescription, string myWINumber, string EmployeeUserId, int tenant)
        {
            return (from d in context.TMEmployeeTimes
                    where d.Tenant == tenant
                    && d.ProjectId == myProjectId
                    && d.Description == myDescription
                    &&d.EmployeeUserId == EmployeeUserId
                    && d.WINumber == myWINumber
                    select d).FirstOrDefault();
        }

        public bool GetTMEmployeeTimeByAnalyzeQueueId(string analyzeQueueId, int tenant)
        {
            return (from d in context.TMEmployeeTimes
                    where d.Tenant == tenant
                    && d.AnalyzeQueueId == analyzeQueueId
                    select d).Any();
        }

        public IQueryable<TMEmployeeTime> GetTasksWithoutProject(int tenant)
        {
            return (from d in context.TMEmployeeTimes
                    where d.Tenant == tenant
                    && d.ProjectId == null
                    select d);

        }

        public IQueryable<TMEmployeeTime> GetAllWithoutTenant()
        {
            return from a in context.TMEmployeeTimes select a;
        }
    }
}
   