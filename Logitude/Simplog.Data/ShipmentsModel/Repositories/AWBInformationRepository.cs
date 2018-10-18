using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBInformationRepository:IRepository<AWBInformation>
    {
        IShipmentsContext shipmentsContext;

        public AWBInformationRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBInformationRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AWBInformation GetSingleAWBInformation(string code)
        {
            return (from a in context.AWBInformations where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AWBInformation> GetAll()
        {
            return (from a in context.AWBInformations select a);
        }

        public IQueryable<AWBInformation> GetAWBInformations()
        {
            return (from a in context.AWBInformations select a);
        }

        public void Add(AWBInformation entity)
        {
            context.AWBInformations.Add(entity);
        }

        public void Remove(AWBInformation entity)
        {
            context.AWBInformations.Attach(entity);
            context.AWBInformations.Remove(entity);
        }

        public void Update(AWBInformation entity)
        {
            context.AWBInformations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBInformation> All()
        {
            return context.AWBInformations.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBInformation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBInformation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
