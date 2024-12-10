
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomsShipperRepository : IRepository<CustomsShipper>
    {
        ICommonDataContext commonDataContext;

        public CustomsShipperRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomsShipperRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomsShipperRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomsShipper GetSinglePM(string id, int tenant)
        {
            return (from a in context.CustomsShippers where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }


        public CustomsShipper GetSingleCustomsShipper(string id, int tenant)
        {
            return (from a in context.CustomsShippers.Include("Card") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }


        public IQueryable<CustomsShipper> GetCustomsShippers(int tenant)
        {
            return from a in context.CustomsShippers.Include("Card")
                   where a.Tenant == tenant
                   select a;
        }

        public void Add(CustomsShipper entity)
        {
            context.CustomsShippers.Add(entity);
        }

        public void Remove(CustomsShipper entity)
        {
            context.CustomsShippers.Attach(entity);
            context.CustomsShippers.Remove(entity);
        }

        public void Update(CustomsShipper entity)
        {
            context.CustomsShippers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsShipper> All()
        {
            return context.CustomsShippers.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomsShipper> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomsShipper GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfCustomsShipperExist(string shipperCode, int tenant)
        {
            return (from a in context.CustomsShippers where a.CustomsShipperCode == shipperCode && a.Tenant == tenant select a).Any();
        }
  
    }
}

