using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ChargesGroupRepository:IRepository<ChargesGroup>
    {


        IWebFreightContext webFreightContext;
        public ChargesGroupRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public ChargesGroupRepository(IWebFreightContext context)
        {
            webFreightContext = context;
           
        }
        public ChargesGroupRepository(int tenant)
        {
      
            webFreightContext = WebFreightContext.GetContext(tenant);
          
        }


      


        public ChargesGroup GetSingleChargesGroupByCode(string code, int tenant)
        {
            return (from a in context.ChargesGroups
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }






        public ChargesGroup GetSingleChargesGroup(string id, int tenant)
        {
            return (from a in context.ChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


      
        public IQueryable<ChargesGroup> GetChargesGroups(int tenant)
        {
       
            return context.ChargesGroups.Where(d=>d.Tenant == tenant);
        }

      


        public void Add(ChargesGroup entity)
        {
            context.ChargesGroups.Add(entity);
        }

        public void Remove(ChargesGroup entity)
        {
            context.ChargesGroups.Attach(entity);
            context.ChargesGroups.Remove(entity);
        }

        public void Update(ChargesGroup entity)
        {
            context.ChargesGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChargesGroup> All()
        {
            return context.ChargesGroups.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ChargesGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ChargesGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}