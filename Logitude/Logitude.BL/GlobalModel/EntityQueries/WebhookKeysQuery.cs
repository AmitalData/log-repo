using System;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class WebhookKeysQuery
    {
        private WebhookKeysRepository repository;
        public WebhookKeysQuery()
        {
            repository = new WebhookKeysRepository();
        }
        public WebhookKeysQuery(int tenant)
        {
            repository = new WebhookKeysRepository();
        }
        public WebhookKeysQuery(WebhookKeysRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<WebhookKeysPM> GetWebhookKeysPMs(int tenant)
        {
            return (from a in repository.context.WebhookKeys
                    where a.Tenant == tenant
                    select new WebhookKeysPM()
                    {
                        Id = a.Id,
                        CreateDate = a.CreateDate,
                        CreatedByUserName = a.CreatedByUserName,
                        AccessKey = a.AccessKey, 
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserName = a.UpdatedByUserName,
                        Description = a.Description,
                        Tenant = a.Tenant,
                        InActive = a.InActive,
                        PartnerName = a.PartnerName
                    });
        }

        public WebhookKeysPM GetSinglePM(string id, int tenant)
        {
            WebhookKeysPM entity = (from a in repository.context.WebhookKeys
                                       where a.Id == id && a.Tenant == tenant
                                       select new WebhookKeysPM()
                                       {
                                           Id = a.Id,
                                           CreateDate = a.CreateDate,
                                           CreatedByUserName = a.CreatedByUserName,
                                           AccessKey = a.AccessKey,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserName = a.UpdatedByUserName,
                                           Description = a.Description,
                                           Tenant = a.Tenant,
                                           InActive = a.InActive,
                                           PartnerName = a.PartnerName
                                       }).FirstOrDefault();



            return entity;
        }

        public WebhookKeysList GetSingleList(int tenant, string id)
        {
            WebhookKeysList entity = (from a in repository.context.WebhookKeys
                                         where a.Id == id && a.Tenant == tenant
                                         select new WebhookKeysList()
                                         {
                                             Id = a.Id,
                                             CreateDate = a.CreateDate,
                                             CreatedByUserName = a.CreatedByUserName,
                                             AccessKey = a.AccessKey,
                                             UpdateDate = a.UpdateDate,
                                             UpdatedByUserName = a.UpdatedByUserName,
                                             Description = a.Description,
                                             Tenant = a.Tenant,
                                             InActive = a.InActive,
                                             PartnerName = a.PartnerName
                                         }).FirstOrDefault();

            return entity;
        }

        public IQueryable<WebhookKeysList> GetWebhookKeysLists(int tenant)
        {
            return (from a in repository.context.WebhookKeys
                    where a.Tenant == tenant
                    select new WebhookKeysList()
                    {
                        Id = a.Id,
                        CreateDate = a.CreateDate,
                        CreatedByUserName = a.CreatedByUserName,
                        AccessKey = a.AccessKey,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserName = a.UpdatedByUserName,
                        Description = a.Description,
                        Tenant = a.Tenant,
                        InActive = a.InActive,
                        PartnerName = a.PartnerName
                    });
        }

        public IQueryable<WebhookKeysList> GetIQueryableEntityList(IQueryable<WebhookKeys> iQueryable)
        {
            return from a in iQueryable
                   select new WebhookKeysList()
                   {
                       Id = a.Id,
                       CreateDate = a.CreateDate,
                       CreatedByUserName = a.CreatedByUserName,
                       AccessKey = a.AccessKey,
                       UpdateDate = a.UpdateDate,
                       UpdatedByUserName = a.UpdatedByUserName,
                       Description = a.Description,
                       Tenant = a.Tenant,
                       InActive = a.InActive,
                       PartnerName = a.PartnerName
                   };
        }
    }
}