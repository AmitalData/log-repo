using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VendorRepository : IRepository<Vendor>
    {
        ICommonDataContext commonDataContext;

        public VendorRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public VendorRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public VendorRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Vendor> GetVendors(int tenant)
        {
            return (from record in context.Vendors.Include("Card") where record.Tenant == tenant select record);
        }

        public Vendor GetSingleVendor(int tenant, string id)
        {
            return (from record in context.Vendors.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Vendor GetSingleVendor(string id, int tenant)
        {
            return (from record in context.Vendors.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Vendor GetSingleVendorByCode(string code,int tenant)
        {
            return (from record in context.Vendors.Include("Card") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }
        public void Add(Vendor entity)
        {
            context.Vendors.Add(entity);
        }

        public void Remove(Vendor entity)
        {
            try
            {
                context.Vendors.Attach(entity);
            }
            catch { }
            context.Vendors.Remove(entity);
        }

        public void Update(Vendor entity)
        {
            try
            {
                context.Vendors.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Vendor> All()
        {
            return context.Vendors.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Vendor> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Vendor GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Vendor GetFirstSingleByName(string name, int tenant)
        {
            return (from record in context.Vendors.Include("Card")
                    where record.Card.EnglishName == name && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
    }
}