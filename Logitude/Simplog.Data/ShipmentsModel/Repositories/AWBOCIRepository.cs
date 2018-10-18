using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBOCIRepository : IRepository<AWBOCI>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public AWBOCIRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBOCIRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public AWBOCI GetSingleAWBOCI(string id, int tenant)
        {
            return (from a in Context.AWBOCIs where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<AWBOCI> GetAWBOCIs()
        {
            return (from a in Context.AWBOCIs select a);
        }

        public IQueryable<AWBOCI> GetAWBOCIsbyShipmentId(string shipmentId, int tenant)
        {
            return (from a in Context.AWBOCIs where a.ShipmentId == shipmentId select a);
        }

        public void Add(AWBOCI entity)
        {
            Context.AWBOCIs.Add(entity);
        }

        public void Remove(AWBOCI entity)
        {
            Context.AWBOCIs.Attach(entity);
            Context.AWBOCIs.Remove(entity);
        }

        public void Update(AWBOCI entity)
        {
            Context.AWBOCIs.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<AWBOCI> All()
        {
            return Context.AWBOCIs.ToList();
        }

        public List<AWBOCI> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBOCI GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
