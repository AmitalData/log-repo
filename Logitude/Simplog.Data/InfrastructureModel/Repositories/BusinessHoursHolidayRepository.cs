using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class BusinessHoursHolidayRepository: IRepository<BusinessHoursHoliday>
    {
        public IWebFreightContext webFreightContext;

        public BusinessHoursHolidayRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public BusinessHoursHolidayRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public BusinessHoursHolidayRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public BusinessHoursHoliday GetSingleBusinessHoursHolidays(string id, int tenant)
        {
            BusinessHoursHoliday entity = this.webFreightContext.BusinessHoursHolidays.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
            return entity;
        }

        public BusinessHoursHoliday GetSingleBusinessHoursHoliday(string id, int tenant)
        {
            BusinessHoursHoliday entity = this.webFreightContext.BusinessHoursHolidays.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
            return entity;
        }

        

        public IQueryable<BusinessHoursHoliday> GetBusinessHoursHolidaysByTenant(int tenant)
        {
            IQueryable<BusinessHoursHoliday> BusinessHoursHolidays = from a in webFreightContext.BusinessHoursHolidays
                                                       where a.Tenant == tenant
                                                       select a;
            return BusinessHoursHolidays;

        }
       
        public IQueryable<BusinessHoursHoliday> GetBusinessHoursHolidays(int tenant)
        {
            return (from record in webFreightContext.BusinessHoursHolidays where record.Tenant == tenant select record);
        }
        public BusinessHoursHoliday GetBusinessHoursHoliday(string entityId, int tenant)
        {
            return (from d in webFreightContext.BusinessHoursHolidays
                    where d.Tenant == tenant
                    && d.Id == entityId
                    select d).FirstOrDefault();
        }

        public void Add(BusinessHoursHoliday entity)
        {
            webFreightContext.BusinessHoursHolidays.Add(entity);
        }

        public void Remove(BusinessHoursHoliday entity)
        {
            webFreightContext.BusinessHoursHolidays.Attach(entity);
            webFreightContext.BusinessHoursHolidays.Remove(entity);
        }

        public void Update(BusinessHoursHoliday entity)
        {
            webFreightContext.BusinessHoursHolidays.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<BusinessHoursHoliday> All()
        {
            return webFreightContext.BusinessHoursHolidays.ToList();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<BusinessHoursHoliday> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BusinessHoursHoliday GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<BusinessHoursHoliday> GetHolidaysByBusinessHour(string id, int tenant)
        {
            return (from a in webFreightContext.BusinessHoursHolidays
                    where a.BusinessHourId == id &&  a.Tenant == tenant
                    select a).ToList();
        }
    }
}
