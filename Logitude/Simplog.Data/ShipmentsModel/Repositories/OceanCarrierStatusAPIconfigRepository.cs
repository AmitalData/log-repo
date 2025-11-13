using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OceanCarrierStatusAPIconfigRepository : IRepository<OceanCarrierStatusAPIconfig>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public OceanCarrierStatusAPIconfigRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public OceanCarrierStatusAPIconfigRepository(IShipmentsContext context)
        {
            myContext = context;
        }



        public void Add(OceanCarrierStatusAPIconfig entity)
        {
            Context.OceanCarrierStatusAPIconfigs.Add(entity);
        }

        public void Remove(OceanCarrierStatusAPIconfig entity)
        {
            Context.OceanCarrierStatusAPIconfigs.Attach(entity);
            Context.OceanCarrierStatusAPIconfigs.Remove(entity);
        }

        public void Update(OceanCarrierStatusAPIconfig entity)
        {
            Context.OceanCarrierStatusAPIconfigs.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<OceanCarrierStatusAPIconfig> All()
        {
            return Context.OceanCarrierStatusAPIconfigs.ToList();
        }

        public List<OceanCarrierStatusAPIconfig> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OceanCarrierStatusAPIconfig GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
