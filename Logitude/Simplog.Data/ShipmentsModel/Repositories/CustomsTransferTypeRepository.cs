using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class CustomsTransferTypeRepository : IRepository<CustomsTransferType>
    {
        IShipmentsContext shipmentsContext;
        
        public CustomsTransferTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public CustomsTransferTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public CustomsTransferType GetSingleCustomsTransferType(string code)
        {
            return (from a in context.CustomsTransferTypes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<CustomsTransferType> GetCustomsTransferTypes()
        {
            return (from a in context.CustomsTransferTypes select a);
        }

        public IQueryable<CustomsTransferType> GetAll()
        {
            return (from a in context.CustomsTransferTypes select a);
        }

        public void Add(CustomsTransferType entity)
        {
            context.CustomsTransferTypes.Add(entity);
        }

        public void Remove(CustomsTransferType entity)
        {
            context.CustomsTransferTypes.Attach(entity);
            context.CustomsTransferTypes.Remove(entity);
        }

        public void Update(CustomsTransferType entity)
        {
            context.CustomsTransferTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsTransferType> All()
        {
            return context.CustomsTransferTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomsTransferType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomsTransferType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}