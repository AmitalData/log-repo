using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class INTTRABookingStatusRepository : IRepository<INTTRABookingStatus>
    {
        IShipmentsContext shipmentsContext;
        public INTTRABookingStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public INTTRABookingStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public INTTRABookingStatus GetSingleINTTRABookingStatus(string code)
        {
            return (from a in context.INTTRABookingStatuses where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<INTTRABookingStatus> GetINTTRABookingStatuses()
        {
            return (from a in context.INTTRABookingStatuses select a);
        }

        public void Add(INTTRABookingStatus entity)
        {
            context.INTTRABookingStatuses.Add(entity);
        }

        public void Remove(INTTRABookingStatus entity)
        {
            context.INTTRABookingStatuses.Attach(entity);
            context.INTTRABookingStatuses.Remove(entity);
        }

        public void Update(INTTRABookingStatus entity)
        {
            context.INTTRABookingStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<INTTRABookingStatus> All()
        {
            return context.INTTRABookingStatuses.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRABookingStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public INTTRABookingStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<INTTRABookingStatus> GetAll()
        {
            return context.INTTRABookingStatuses;
        }
    }
}
