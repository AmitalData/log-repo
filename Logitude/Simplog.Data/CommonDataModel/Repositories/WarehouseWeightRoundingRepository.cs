using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WarehouseWeightRoundingRepository : IRepository<WarehouseWeightRounding>
    {
        ICommonDataContext commonDataContext;

        public WarehouseWeightRoundingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public WarehouseWeightRoundingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public WarehouseWeightRounding GetSingleWarehouseWeightRounding(string code)
        {
            WarehouseWeightRounding instance = (from i in context.WarehouseWeightRoundings
                                                where i.Code == code
                                                select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<WarehouseWeightRounding> GetWarehouseWeightRoundings()
        {
            return context.WarehouseWeightRoundings;
        }

        public IQueryable<WarehouseWeightRounding> GetAll()
        {
            return context.WarehouseWeightRoundings;
        }

        public void Add(WarehouseWeightRounding entity)
        {
            context.WarehouseWeightRoundings.Add(entity);
        }

        public void Remove(WarehouseWeightRounding entity)
        {
            context.WarehouseWeightRoundings.Attach(entity);
            context.WarehouseWeightRoundings.Remove(entity);
        }

        public void Update(WarehouseWeightRounding entity)
        {
            context.WarehouseWeightRoundings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseWeightRounding> All()
        {
            return context.WarehouseWeightRoundings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<WarehouseWeightRounding> IRepository<WarehouseWeightRounding>.GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        WarehouseWeightRounding IRepository<WarehouseWeightRounding>.GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
