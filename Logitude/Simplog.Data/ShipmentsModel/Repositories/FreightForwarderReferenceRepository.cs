using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class FreightForwarderReferenceRepository: IRepository<FreightForwarderReference>
    {
        IShipmentsContext shipmentsContext;
        public FreightForwarderReferenceRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }
        public FreightForwarderReferenceRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public FreightForwarderReferenceRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }


        public IQueryable<FreightForwarderReference> GetFreightForwarderReferences(int tenant)
        {
            return (from record in context.FreightForwarderReferences where record.Tenant == tenant select record);
        }

        public FreightForwarderReference GetSingleFreightForwarderReference(int tenant, string id, int? authTokenTenant = null)
        {
            return (from record in context.FreightForwarderReferences 
                    where record.ShipmentId == id && record.Tenant == tenant 
                    select record
                    ).FirstOrDefault();
        }

        public List<FreightForwarderReference> GetFreightForwarderReferencesByIds(List<string> ids, int tenant)
        {
            List<FreightForwarderReference> myResult = new List<FreightForwarderReference>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.FreightForwarderReferences
                            where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                            select a).ToList();
            }

            return myResult;
        }

        public void Add(FreightForwarderReference entity)
        {
            context.FreightForwarderReferences.Add(entity);
        }

        public void Remove(FreightForwarderReference entity)
        {
            try
            {

                context.FreightForwarderReferences.Attach(entity);
            }
            catch { }
            context.FreightForwarderReferences.Remove(entity);
        }

        public void Update(FreightForwarderReference entity)
        {
            try
            {
                context.FreightForwarderReferences.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<FreightForwarderReference> All()
        {
            return context.FreightForwarderReferences.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FreightForwarderReference> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FreightForwarderReference GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
