using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBCustomsInformationRepository:IRepository<AWBCustomsInformation>
    {
        IShipmentsContext shipmentsContext;

        public AWBCustomsInformationRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBCustomsInformationRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AWBCustomsInformation GetSingleAWBCustomsInfo(string code)
        {
            return (from a in context.AWBCustomsInformations where a.Code == code select a).FirstOrDefault();
        }


        public AWBCustomsInformation GetSingleAWBCustomsInformation(string code)
        {
            return (from a in context.AWBCustomsInformations where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AWBCustomsInformation> GetAll()
        {
            return (from a in context.AWBCustomsInformations select a);
        }

        public IQueryable<AWBCustomsInformation> GetAWBCustomsInfos()
        {
            return (from a in context.AWBCustomsInformations select a);
        }

        public IQueryable<AWBCustomsInformation> GetAWBCustomsInformations()
        {
            return (from a in context.AWBCustomsInformations select a);
        }

        public void Add(AWBCustomsInformation entity)
        {
            context.AWBCustomsInformations.Add(entity);
        }

        public void Remove(AWBCustomsInformation entity)
        {
            context.AWBCustomsInformations.Attach(entity);
            context.AWBCustomsInformations.Remove(entity);
        }

        public void Update(AWBCustomsInformation entity)
        {
            context.AWBCustomsInformations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBCustomsInformation> All()
        {
            return context.AWBCustomsInformations.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBCustomsInformation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBCustomsInformation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
