using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AddressRepository : IRepository<Address>
    {
        ICommonDataContext commonDataContext;

        public AddressRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AddressRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AddressRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<Address> GetAddresses(int tenant)
        {
            return (from record in context.Addresses.Include("Country").Include("State") where record.Tenant == tenant select record);
        }

        public IQueryable<Address> GetMainAddresses(int tenant)
        {
            return (from record in context.Addresses.Include("Country").Include("State") where record.Tenant == tenant && record.AddressTypeId == "M" select record);
        }

        public Address GetSingleAddress(string id, int tenant)
        {
            return (from record in context.Addresses.Include("Country").Include("State") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Address GetSingleAddressByExternalId(string id, int tenant)
        {
            return (from record in context.Addresses.Include("Country").Include("State") where record.ExternalId == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Address GetSingleAddressByCardIdAndTypeId(string cardId,string addressTypeId, int tenant)
        {
            return (from record in context.Addresses.Include("Country").Include("State") where record.AddressTypeId == addressTypeId && record.CardId == cardId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Address GetMainAddressByCardId(string cardId, int tenent)
        {
            return (from a in context.Addresses.Include("Country").Include("State") where a.Tenant == tenent && a.CardId == cardId && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();
        }

        public Address GetPickupDeliveryAddressByCardId(string cardId, int tenent)
        {
            return (from a in context.Addresses.Include("Country").Include("State") where a.Tenant == tenent && a.CardId == cardId && a.AddressTypeId.ToUpper() == "P" select a).FirstOrDefault();
        }

        public void Add(Address entity)
        {
            context.Addresses.Add(entity);
        }

        public void Remove(Address entity)
        {
            context.Addresses.Attach(entity);
            context.Addresses.Remove(entity);
        }

        public void Update(Address entity)
        {
            context.Addresses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Address> All()
        {
            return context.Addresses.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Address> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Address GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}