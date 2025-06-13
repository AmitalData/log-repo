using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class CustomPickListRepository:IRepository<CustomPickList,string>
    {
        IAmitalCloudContext currentContext;

        public CustomPickListRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public CustomPickListRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public CustomPickListRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }

        public IQueryable<CustomPickList> GetCustomPickLists(int tenant)
        {
			return context.CustomPickLists.Where(t=>t.Tenant == tenant);
		}

		public List<CustomPickList> GetCustomPickListsCashe(int tenant)
		{
			List<CustomPickList> customPickLists;
			string listName = "CustomPickList" + tenant;

			if (CacheManager.CacheWrapper.Get(listName) == null)
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{
					customPickLists = context.CustomPickLists.Where(t => t.Tenant == tenant).ToList();
				}

				CacheManager.CacheWrapper.Insert(listName, customPickLists, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
			}
			else
			{
				customPickLists = (List<CustomPickList>)CacheManager.CacheWrapper.Get(listName);
			}
			return customPickLists;
		}


		public CustomPickList GetSingleCustomPickList(string id,int tenant)
        {
            return (from a in context.CustomPickLists  where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();   
        }


        public CustomPickList GetSingleCustomPickListByValue(string code, string value, int tenant)
        {
            return (from a in context.CustomPickLists where a.Code == code && a.Value == value && a.Tenant == tenant select a).FirstOrDefault();
        }

        #region IRepository<CustomPickList> Members

        public void Add(CustomPickList entity)
        {
            context.CustomPickLists.Add(entity);
        }

        public void Remove(CustomPickList entity)
        {
            context.CustomPickLists.Attach(entity);
            context.CustomPickLists.Remove(entity);
        }

        public void Update(CustomPickList entity)
        {
            context.CustomPickLists.Attach(entity);
            context.SetAsModified(entity);


        }

        public List<CustomPickList> All()
        {
            return context.CustomPickLists.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        #endregion

        public List<CustomPickList> GetMulti(IEntityKeyFields<CustomPickList,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomPickList GetSingle(IEntityKeyFields<CustomPickList,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}