using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class SpecialServicesTypeRepository : IRepository<SpecialServicesType>
    {
        IShipmentsContext shipmentContext;

        public SpecialServicesTypeRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public SpecialServicesTypeRepository()
        {
            shipmentContext = new  ShipmentsContext();
        }

        public SpecialServicesTypeRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<SpecialServicesType> GetSpecialServicesTypes(int tenant)
        {
            return (from d in context.SpecialServicesTypes where d.Tenant == tenant select d);
        }

        public SpecialServicesType GetSingleSpecialServicesType(string id, int tenant)
        {
            return (from record in context.SpecialServicesTypes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();           
        }

        public SpecialServicesType GetSingleSpecialServicesTypeByCode(string code, int tenant)
        {
            return (from record in context.SpecialServicesTypes where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }     

        public void Add(SpecialServicesType entity)
        {
            context.SpecialServicesTypes.Add(entity);
        }

        public void Remove(SpecialServicesType entity)
        {
            try
            {
                context.SpecialServicesTypes.Attach(entity);
            }
            catch { }
            context.SpecialServicesTypes.Remove(entity);
        }

        public void Update(SpecialServicesType entity)
        {
            try
            {
                context.SpecialServicesTypes.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<SpecialServicesType> All()
        {
            return context.SpecialServicesTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SpecialServicesType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SpecialServicesType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
