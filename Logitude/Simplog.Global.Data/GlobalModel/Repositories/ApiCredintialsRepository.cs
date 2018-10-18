using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class ApiCredintialsRepository : IRepository<ApiCredintials>
    {
        IGlobalContext globalContext;
        public ApiCredintialsRepository()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                globalContext = GlobalContext.GetContext();
                scope.Complete();
            }
        }

        public ApiCredintialsRepository(IGlobalContext context)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                globalContext = context;
                scope.Complete();
            }
        }

        public string GetApiCredintialsIdByHashedPrimaryAccessKey(string hashedPrimaryAccessKey )
        {
            string result = "";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ApiCredintials apiCredintials =(from a in context.ApiCredintials
                        where a.HashedPrimaryAccessKey == hashedPrimaryAccessKey
                        select a).FirstOrDefault();

                if (apiCredintials != null)
                {
                    result = apiCredintials.Id;
                }
            }
            return result;
        }

  

        public ApiCredintials GetSingleApiCredintials(string id, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                return (from a in context.ApiCredintials
                        where a.Id == id && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
        }

        public IQueryable<ApiCredintials> GetApiCredintials(int tenant)
        {
            return from a in context.ApiCredintials
                   where a.Tenant == tenant
                   select a;
        }

        public void Add(ApiCredintials entity)
        {
            context.ApiCredintials.Add(entity);
        }

        public void Remove(ApiCredintials entity)
        {
            context.ApiCredintials.Attach(entity);
            context.ApiCredintials.Remove(entity);
        }

        public void Update(ApiCredintials entity)
        {
            context.ApiCredintials.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ApiCredintials> All()
        {
            return context.ApiCredintials.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ApiCredintials> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ApiCredintials GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ApiCredintials GetApiCredintials_TFS(string hashedPrimaryAccessKey)
        {
            ApiCredintials apiCredintials = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                apiCredintials = (from a in context.ApiCredintials
                                  where a.HashedPrimaryAccessKey == hashedPrimaryAccessKey
                                  select a).FirstOrDefault();
            }
            return apiCredintials;
        }




    }
}