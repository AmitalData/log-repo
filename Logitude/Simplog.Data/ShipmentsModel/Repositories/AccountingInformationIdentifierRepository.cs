using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AccountingInformationIdentifierRepository : IRepository<AccountingInformationIdentifier>
    {
        IShipmentsContext shipmentsContext;
        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public AccountingInformationIdentifierRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AccountingInformationIdentifierRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AccountingInformationIdentifier GetSingleAccountingInformationIdentifier(string code)
        {
            return (from a in context.AccountingInformationIdentifiers where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AccountingInformationIdentifier> GetAll()
        {
            return (from a in context.AccountingInformationIdentifiers select a);
        }
        public IQueryable<AccountingInformationIdentifier> GetAccountingInformatiIdentifiers()
        {
            return (from a in context.AccountingInformationIdentifiers select a);
        }

        public void Add(AccountingInformationIdentifier entity)
        {
            context.AccountingInformationIdentifiers.Add(entity);
        }

        public void Remove(AccountingInformationIdentifier entity)
        {
            context.AccountingInformationIdentifiers.Attach(entity);
            context.AccountingInformationIdentifiers.Remove(entity);
        }

        public void Update(AccountingInformationIdentifier entity)
        {
            context.AccountingInformationIdentifiers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingInformationIdentifier> All()
        {
            return context.AccountingInformationIdentifiers.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AccountingInformationIdentifier> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountingInformationIdentifier GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
