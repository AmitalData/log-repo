using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class NextLegRepository: IRepository<NextLeg>
    {
        IShipmentsContext shipmentsContext;

        public NextLegRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public NextLegRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public NextLegRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<NextLeg> GetNextLegs()
        {
            return context.NextLegs;
        }

        public NextLeg GetSingleNextLeg(string code)
        {
            return (from a in context.NextLegs where a.Code == code select a).FirstOrDefault();
        }

        public void Add(NextLeg entity)
        {
            context.NextLegs.Add(entity);
        }

        public void Remove(NextLeg entity)
        {
            context.NextLegs.Attach(entity);
            context.NextLegs.Remove(entity);
        }

        public void Update(NextLeg entity)
        {
            context.NextLegs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NextLeg> All()
        {
            return context.NextLegs.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<NextLeg> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public NextLeg GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}