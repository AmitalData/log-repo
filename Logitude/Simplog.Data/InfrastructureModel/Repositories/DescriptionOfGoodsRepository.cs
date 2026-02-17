using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DescriptionOfGoodsRepository:IRepository<DescriptionOfGoods>
    {

        IWebFreightContext webFreightContext;
        public DescriptionOfGoodsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DescriptionOfGoodsRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public DescriptionOfGoodsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public DescriptionOfGoods GetSingleDescriptionOfGoods(string id,int tenant = 0)
        {
            return (from a in context.DescriptionOfGoods
                    where a.Id == id
                    select a).FirstOrDefault();
        }
       
         

        public IQueryable<DescriptionOfGoods> GetDescriptionOfGoodsByTenant(int tenant)
        {
            return from a in context.DescriptionOfGoods
                   where a.Tenant == tenant
                   select a;
        }

        public IQueryable<DescriptionOfGoods> GetDescriptionOfGoods(int tenant)
        {
            return from a in context.DescriptionOfGoods
                   where a.Tenant == tenant
                   select a;
        }

        


        public void Add(DescriptionOfGoods entity)
        {
            context.DescriptionOfGoods.Add(entity);
        }

        public void Remove(DescriptionOfGoods entity)
        {
            context.DescriptionOfGoods.Attach(entity);
            context.DescriptionOfGoods.Remove(entity);
        }

        public void Update(DescriptionOfGoods entity)
        {
            context.DescriptionOfGoods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DescriptionOfGoods> All()
        {
            return context.DescriptionOfGoods.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<DescriptionOfGoods> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DescriptionOfGoods GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}