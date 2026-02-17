using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AccountingSystemRepository:IRepository<AccountingSystem>
    {
        ICommonDataContext commonDataContext;

        public AccountingSystemRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AccountingSystemRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AccountingSystemRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AccountingSystem> GetAccountingSystems()
        {
            return context.AccountingSystems;
        }
        public IQueryable<AccountingSystem> GetAll()
        {
            return context.AccountingSystems;
        }

        public AccountingSystem GetSingleAccountingSystem(string code)
        {
            return (from record in context.AccountingSystems where record.Code == code select record).FirstOrDefault();
        }

        public void Add(AccountingSystem entity)
        {
            context.AccountingSystems.Add(entity);
        }

        public void Remove(AccountingSystem entity)
        {
            context.AccountingSystems.Attach(entity);
            context.AccountingSystems.Remove(entity);
        }

        public void Update(AccountingSystem entity)
        {
            context.AccountingSystems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingSystem> All()
        {
            return context.AccountingSystems.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AccountingSystem> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountingSystem GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
