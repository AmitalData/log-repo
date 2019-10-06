using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SchedulerProcedureRepository : IRepository<SchedulerProcedure>
    {
        public IWebFreightContext webFreightContext;

        public SchedulerProcedureRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public SchedulerProcedureRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public SchedulerProcedureRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(SchedulerProcedure entity)
        {
            webFreightContext.SchedulerProcedures.Add(entity);
        }

        public void Remove(SchedulerProcedure entity)
        {
            webFreightContext.SchedulerProcedures.Attach(entity);
            webFreightContext.SchedulerProcedures.Remove(entity);
        }

        public void Update(SchedulerProcedure entity)
        {
            webFreightContext.SchedulerProcedures.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<SchedulerProcedure> All()
        {
            return webFreightContext.SchedulerProcedures.ToList();
        }
        public SchedulerProcedure GetSingleSchedulerProcedure(string code)
        {
            return webFreightContext.SchedulerProcedures.Where(a => a.Code == code).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }
        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }
        public List<SchedulerProcedure> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SchedulerProcedure GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SchedulerProcedure> GetSchedulerProcedures()
        {
            return webFreightContext.SchedulerProcedures;
        }

        public IQueryable<SchedulerProcedure> GetAll()
        {
            return webFreightContext.SchedulerProcedures;
        }





    }
}

