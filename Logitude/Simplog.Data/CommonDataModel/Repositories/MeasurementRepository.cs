using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class MeasurementRepository:IRepository<Measurement>
    {
        ICommonDataContext commonDataContext;

        public MeasurementRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public MeasurementRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public MeasurementRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Measurement> GetMeasurements()
        {
            return context.Measurements;
        }


        public IQueryable<Measurement> GetMeasurements(int tenant)
        {
            return (from a in context.Measurements
                    where a.Tenant == tenant
                    select a);
        }

        public IQueryable<Measurement> GetMeasurementsByTenant(int tenant)
        {
            return (from a in context.Measurements
                    where a.Tenant == tenant
                    select a);
        }

        public Measurement GetSingleMeasurement(string id, int tenant)
        {
            return (from a in context.Measurements where a.Id == id && a.Tenant==tenant select a).FirstOrDefault();
        }

        public string GetMeasurementIdbyCode(string code, int tenant)
        {
            string result = null;
            Measurement m = context.Measurements.Where(d => d.Code == code && d.Tenant == tenant).FirstOrDefault();
            if (m != null)
            {
                result = m.Id;
            }

            return result;
        }

        public Measurement GetMeasurementbyCode(string code, int tenant)
        {
           
            Measurement result = context.Measurements.Where(d => d.Code == code && d.Tenant == tenant).FirstOrDefault();
           
            return result;
        }

        public IQueryable<Measurement> GetUnitOfMeasurements(int tenant)
        {
            return from a in context.Measurements
                   where a.Tenant == tenant
                   select a;
        }

        public void Add(Measurement entity)
        {
            context.Measurements.Add(entity);
        }

        public void Remove(Measurement entity)
        {
            context.Measurements.Attach(entity);
            context.Measurements.Remove(entity);
        }

        public void Update(Measurement entity)
        {
            context.Measurements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Measurement> All()
        {
            return context.Measurements.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Measurement> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Measurement GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
