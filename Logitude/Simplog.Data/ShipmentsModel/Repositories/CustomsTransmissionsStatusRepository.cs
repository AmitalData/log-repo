using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class CustomsTransmissionsStatusRepository : IRepository<CustomsTransmissionsStatus>
    {
        IShipmentsContext shipmentsContext;

        public CustomsTransmissionsStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public CustomsTransmissionsStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public CustomsTransmissionsStatus GetSingleCustomsTransmissionsStatus(string code)
        {
            return (from a in context.CustomsTransmissionsStatus where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<CustomsTransmissionsStatus> GetAll()
        {
            return (from a in context.CustomsTransmissionsStatus select a);
        }
        public IQueryable<CustomsTransmissionsStatus> GetCustomsTransmissionsStatus()
        {
            return (from a in context.CustomsTransmissionsStatus select a);
        }

        public void Add(CustomsTransmissionsStatus entity)
        {
            context.CustomsTransmissionsStatus.Add(entity);
        }

        public void Remove(CustomsTransmissionsStatus entity)
        {
            context.CustomsTransmissionsStatus.Attach(entity);
            context.CustomsTransmissionsStatus.Remove(entity);
        }

        public void Update(CustomsTransmissionsStatus entity)
        {
            context.CustomsTransmissionsStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsTransmissionsStatus> All()
        {
            return context.CustomsTransmissionsStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomsTransmissionsStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomsTransmissionsStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}