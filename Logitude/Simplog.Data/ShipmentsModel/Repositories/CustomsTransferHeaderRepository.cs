using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class CustomsTransferHeaderRepository : IRepository<CustomsTransferHeader>
    {
        IShipmentsContext shipmentsContext;
        
        public CustomsTransferHeaderRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public CustomsTransferHeaderRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public CustomsTransferHeader GetSingleEntity(string id, int tenant)
        {
            return (from a in context.CustomsTransferHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public CustomsTransferHeader GetSingleCustomsTransferHeader(string id, int tenant)
        {
            return (from a in context.CustomsTransferHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsTransferHeader> GetCustomsTransferHeaders(int tenant)
        {
            return (from a in context.CustomsTransferHeaders
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(CustomsTransferHeader entity)
        {
            context.CustomsTransferHeaders.Add(entity);
        }

        public void Remove(CustomsTransferHeader entity)
        {
            context.CustomsTransferHeaders.Attach(entity);
            context.CustomsTransferHeaders.Remove(entity);
        }

        public void Update(CustomsTransferHeader entity)
        {
            context.CustomsTransferHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsTransferHeader> All()
        {
            return context.CustomsTransferHeaders.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomsTransferHeader> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomsTransferHeader GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}