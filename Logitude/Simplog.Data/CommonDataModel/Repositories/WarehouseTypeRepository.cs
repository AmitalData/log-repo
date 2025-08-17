using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WarehouseTypeRepository : IRepository<WarehouseType>
    {
        ICommonDataContext commonDataContext;

        public WarehouseTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public WarehouseTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public WarehouseType GetSingleWarehouseType(string code)
        {
            WarehouseType instance = (from i in context.WarehouseTypes
                                             where i.Code == code
                                             select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<WarehouseType> GetWarehouseTypes()
        {
            return context.WarehouseTypes;
        }

        public IQueryable<WarehouseType> GetAll()
        {
            return context.WarehouseTypes;
        }

        public void Add(WarehouseType entity)
        {
            context.WarehouseTypes.Add(entity);
        }

        public void Remove(WarehouseType entity)
        {
            context.WarehouseTypes.Attach(entity);
            context.WarehouseTypes.Remove(entity);
        }

        public void Update(WarehouseType entity)
        {
            context.WarehouseTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseType> All()
        {
            return context.WarehouseTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<WarehouseType> IRepository<WarehouseType>.GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        WarehouseType IRepository<WarehouseType>.GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
