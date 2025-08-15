using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SharedFollowedShipmentRepository : IRepository<SharedFollowedShipment>
    {

        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }



        public SharedFollowedShipmentRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public SharedFollowedShipmentRepository(ICommonDataContext context)
        {
            this.Context = context;
        }



        public SharedFollowedShipment GetSingleSharedFollowedShipment(string id, int tenant)
        {
            return (from a in context.SharedFollowedShipments where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<SharedFollowedShipment> GetSharedFollowedShipments(int tenant)
        {
            return context.SharedFollowedShipments.Where(d => d.Tenant == tenant);
        }

        public List<string> GetAllIds(int tenant)
        {
            List<string> myResult = context.SharedFollowedShipments.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
            return myResult;
        }



        public void Add(SharedFollowedShipment entity)
        {
            context.SharedFollowedShipments.Add(entity);
        }

        public void Remove(SharedFollowedShipment entity)
        {
            context.SharedFollowedShipments.Remove(entity);
        }

        public void Update(SharedFollowedShipment entity)
        {
            context.SharedFollowedShipments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedFollowedShipment> All()
        {
            return context.SharedFollowedShipments.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SharedFollowedShipment> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SharedFollowedShipment GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


        public SharedFollowedShipment GetSingleSharedFollowedShipmentByShipmentId(string shipmentId)
        {
            return (from a in context.SharedFollowedShipments where a.ShipmentId == shipmentId  select a).FirstOrDefault();
        }


        public SharedFollowedShipment GetSingleSharedFollowedShipmentByShipmentIdAndContactId(string shipmentId, string contactId, int tenant)
        {
            return (from a in context.SharedFollowedShipments where a.ShipmentId == shipmentId && a.ContactId == contactId && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<SharedFollowedShipment> GetSharedFollowedShipmentByShipmentId(string shipmentId,int  tenant)
        {
            return (from a in context.SharedFollowedShipments where a.ShipmentId == shipmentId && a.Tenant == tenant select a);
        }


        public List<string> GetSharedFollowedShipmentByContactId(string contactId, int tenant, List<string> trackedIds)
        {
            //return context.SharedFollowedShipments.Where(d => d.ContactId == contactId && d.Tenant == tenant).ToList();

            List<string> SharedFollowedShipmentLists = (from d in context.SharedFollowedShipments
                                                                              where trackedIds.Contains(d.ShipmentId) && d.ContactId == contactId && d.Tenant == tenant
                                                                              select d.ShipmentId).ToList();
            return SharedFollowedShipmentLists;
        }


        public IQueryable<string> GetContactsFollowedShipmentIdByShipmentId(string shipmentid, int tenant)
        {

            IQueryable<string> contactFollowedShipmentLists = (from d in context.SharedFollowedShipments
                                                              where d.ShipmentId == shipmentid && d.Tenant == tenant
                                                              select d.ContactId);
            return contactFollowedShipmentLists;
        }
 





    }
}
