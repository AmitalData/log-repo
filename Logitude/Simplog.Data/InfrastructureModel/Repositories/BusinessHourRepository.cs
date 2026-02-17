using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class BusinessHourRepository: IRepository<BusinessHour>
    {
        public IWebFreightContext webFreightContext;

        public BusinessHourRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public BusinessHourRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public BusinessHourRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }




        public BusinessHour GetSingleBusinessHour(string id, int tenant)
        {

            BusinessHour entity = this.webFreightContext.BusinessHours.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;

        }
        public BusinessHour GetSingleBusinessHours(string id, int tenant)
        {

            BusinessHour entity = this.webFreightContext.BusinessHours.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;

        }

        public IQueryable<BusinessHour> GetBusinessHoursByTenant(int tenant)
        {
            IQueryable<BusinessHour> BusinessHours = from a in webFreightContext.BusinessHours
                                                       where a.Tenant == tenant
                                                       select a;
            return BusinessHours;

        }
       
        public IQueryable<BusinessHour> GetBusinessHours(int tenant)
        {
            return (from record in webFreightContext.BusinessHours where record.Tenant == tenant select record);
        }
        public BusinessHour GetBusinessHour(string entityId, int tenant)
        {
            return (from d in webFreightContext.BusinessHours
                    where d.Tenant == tenant
                    && d.Id == entityId
                    select d).FirstOrDefault();
        }

        public void Add(BusinessHour entity)
        {
            webFreightContext.BusinessHours.Add(entity);
        }

        public void Remove(BusinessHour entity)
        {
            webFreightContext.BusinessHours.Attach(entity);
            webFreightContext.BusinessHours.Remove(entity);
        }

        public void Update(BusinessHour entity)
        {
            webFreightContext.BusinessHours.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<BusinessHour> All()
        {
            return webFreightContext.BusinessHours.ToList();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<BusinessHour> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BusinessHour GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BusinessHour GetBusinessHourByCode(string code, int tenant)
        {
            return (from d in webFreightContext.BusinessHours
                    where d.Tenant == tenant
                    && d.Code == code
                    select d).FirstOrDefault();
        }
    }
}
