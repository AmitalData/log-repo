using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class IATACodeRepository:IRepository<IATACode>
    {
        IWebFreightContext webFreightContext;

        public IATACodeRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public IATACodeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IATACodeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        
        public IATACode GetSingleIATACode(string id)
        {
            return (from a in context.IATACodes where a.Id == id select a).FirstOrDefault();
        }      

        public IQueryable<IATACode> GetIATACodes()
        {
            return context.IATACodes;
        }
  
        public void Add(IATACode entity)
        {
            context.IATACodes.Add(entity);
        }

        public void Remove(IATACode entity)
        {
            context.IATACodes.Attach(entity);
            context.IATACodes.Remove(entity);
        }

        public void Update(IATACode entity)
        {
            context.IATACodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<IATACode> All()
        {
            return context.IATACodes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<IATACode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IATACode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<IATACode> GetIATACodesByAirlineId(string airlineId)
        {
            return context.IATACodes.Where(a => a.AirlineId == airlineId);
        }
    }
}