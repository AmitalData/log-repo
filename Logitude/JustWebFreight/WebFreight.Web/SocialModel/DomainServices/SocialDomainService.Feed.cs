using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.SocialModel.DomainServices
{
    public partial class SocialDomainService
    {
        public FeedPM GetSingleFeedPM(string postid, string userid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            socialContext = SocialContext.GetContext(tenant);
            feedQuery = new FeedQueryService(socialContext);
            FeedPM feed = feedQuery.GetSingle(postid, userid, false, false);
            return feed;
        }

        public FeedList GetSingleFeedList(string postid, string userid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
             

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }
           
            FeedListQueryService listService = new FeedListQueryService(socialContext);
            return listService.GetSingle(postid, userid);
        }

        public List<FeedList> GetFeedLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FeedListQueryService listService = new FeedListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<FeedList> GetFeedsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FeedListQueryService listService = new FeedListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetFeedFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FeedListQueryService queryService = new FeedListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertFeed(FeedPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            FeedUpdateService service = new FeedUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateFeed(FeedPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            FeedUpdateService service = new FeedUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }




        [Invoke]
        public string GetDeflutColorForUser(string userid, int tenant)
        {

            string color = "";
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact contact = contactRepository.GetSingleContact(userid, tenant);

            if (contact != null)
            {
                ColorIndexRepository colorIndexRepository = new ColorIndexRepository(tenant);
                color = colorIndexRepository.GetSingleHasColor(contact.IndexColor);
            }

            return color;
        }
    }
}