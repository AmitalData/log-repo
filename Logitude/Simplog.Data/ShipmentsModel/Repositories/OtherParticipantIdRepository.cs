using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OtherParticipantIdRepository : IRepository<OtherParticipantId>
    {
        IShipmentsContext shipmentsContext;

        public OtherParticipantIdRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public OtherParticipantIdRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public OtherParticipantId GetSingleOtherParticipantId(string code)
        {
            return (from a in context.OtherParticipantIds where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<OtherParticipantId> GetAll()
        {
            return (from a in context.OtherParticipantIds select a);
        }
        public IQueryable<OtherParticipantId> GetOtherParticipantIds()
        {
            return (from a in context.OtherParticipantIds select a);
        }

        public void Add(OtherParticipantId entity)
        {
            context.OtherParticipantIds.Add(entity);
        }

        public void Remove(OtherParticipantId entity)
        {
            context.OtherParticipantIds.Attach(entity);
            context.OtherParticipantIds.Remove(entity);
        }

        public void Update(OtherParticipantId entity)
        {
            context.OtherParticipantIds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OtherParticipantId> All()
        {
            return context.OtherParticipantIds.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<OtherParticipantId> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OtherParticipantId GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
