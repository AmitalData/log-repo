
using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class WebhookKeysRepository : IRepository<WebhookKeys>
    {
        IGlobalContext globalContext;
        public WebhookKeysRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public WebhookKeysRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public WebhookKeys GetSingleWebhookKeysById(string id)
        {
            WebhookKeys item = context.WebhookKeys.Where(d => d.Id == id).FirstOrDefault();
            return item;
        }

        public WebhookKeys GetSingleWebhookKeys(string id,int Tenant)
        {
            WebhookKeys item = context.WebhookKeys.Where(d => d.Id == id && d.Tenant == Tenant).FirstOrDefault();
            return item;
        }


        public IQueryable<WebhookKeys> GetWebhookKeys(int Tenant)
        {
            IQueryable<WebhookKeys> items = context.WebhookKeys.Where(d => d.Tenant == Tenant);
            return items;
        }
        



        public WebhookKeys GetSingleWebhookKeyByAccessKey(string key)
        {
            WebhookKeys item = context.WebhookKeys.Where(d => d.AccessKey == key).FirstOrDefault();
            return item;
        }



        public IQueryable<WebhookKeys> GetAllWebhookKeyss()
        {
            return from a in context.WebhookKeys
                   select a;
        }


        public void Add(WebhookKeys entity)
        {
            context.WebhookKeys.Add(entity);
        }

        public void Remove(WebhookKeys entity)
        {
            context.WebhookKeys.Attach(entity);
            context.WebhookKeys.Remove(entity);
        }

        public void Update(WebhookKeys entity)
        {
            context.WebhookKeys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WebhookKeys> All()
        {
            return context.WebhookKeys.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<WebhookKeys> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public WebhookKeys GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}