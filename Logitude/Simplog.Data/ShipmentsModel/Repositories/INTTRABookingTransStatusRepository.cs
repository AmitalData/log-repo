using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class INTTRABookingTransStatusRepository : IRepository<INTTRABookingTransStatus>
    {
        IShipmentsContext shipmentsContext;
        public INTTRABookingTransStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public INTTRABookingTransStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public INTTRABookingTransStatus GetSingleINTTRABookingTransStatus(string code)
        {
            return (from a in context.INTTRABookingTransStatuses where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<INTTRABookingTransStatus> GetINTTRABookingTransStatuses()
        {
            return (from a in context.INTTRABookingTransStatuses select a);
        }

        public void Add(INTTRABookingTransStatus entity)
        {
            context.INTTRABookingTransStatuses.Add(entity);
        }

        public void Remove(INTTRABookingTransStatus entity)
        {
            context.INTTRABookingTransStatuses.Attach(entity);
            context.INTTRABookingTransStatuses.Remove(entity);
        }

        public void Update(INTTRABookingTransStatus entity)
        {
            context.INTTRABookingTransStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<INTTRABookingTransStatus> All()
        {
            return context.INTTRABookingTransStatuses.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRABookingTransStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public INTTRABookingTransStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<INTTRABookingTransStatus> GetAll()
        {
            return context.INTTRABookingTransStatuses;
        }

    }
}
