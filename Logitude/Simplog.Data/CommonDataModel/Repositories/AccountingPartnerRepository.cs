using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AccountingPartnerRepository : IRepository<AccountingPartner>
    {
        ICommonDataContext commonDataContext;

        public AccountingPartnerRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AccountingPartnerRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AccountingPartnerRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AccountingPartner> GetAccountingPartners(int tenant)
        {
            return (from record in context.AccountingPartners.Include("Card") where record.Tenant == tenant select record);
        }

        public AccountingPartner GetSingleAccountingPartner(int tenant, string id)
        {
            return (from record in context.AccountingPartners.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AccountingPartner GetSingleAccountingPartner(string id, int tenant)
        {
            return (from record in context.AccountingPartners.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AccountingPartner GetSingleAccountingPartnerByCode(string code,int tenant)
        {
            return (from record in context.AccountingPartners.Include("Card") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }
        public List<AccountingPartner> GetAccountingPartnersByIds(List<string> ids, int tenant)
        {
            return (from record in context.AccountingPartners where ids.Contains(record.Id) && record.Tenant == tenant select record).ToList();
        }
        public void Add(AccountingPartner entity)
        {
            context.AccountingPartners.Add(entity);
        }

        public void Remove(AccountingPartner entity)
        {
            try
            {
                context.AccountingPartners.Attach(entity);
            }
            catch { }
            context.AccountingPartners.Remove(entity);
        }

        public void Update(AccountingPartner entity)
        {
            try
            {
                context.AccountingPartners.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<AccountingPartner> All()
        {
            return context.AccountingPartners.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AccountingPartner> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountingPartner GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<AccountingPartner> GetAccountingPartner(int tenant)
        {
            return (from record in context.AccountingPartners.Include("Card") where record.Tenant == tenant select record);
        }
    }
}