using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.SocialModel.DomainServices
{
    public partial class SocialDomainService
    {

        public GroupPM GetSingleGroupPM(string id, int tenant)
        {
            socialContext = SocialContext.GetContext(tenant);
            groupQuery = new GroupQueryService(socialContext);
            GroupPM group = groupQuery.GetSingle(id, false, false);
            return group;
        }

        public GroupList GetSingleGroupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupListQueryService listService = new GroupListQueryService(socialContext);
            return listService.GetSingle(id);
        }

        public List<GroupList> GetGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupListQueryService listService = new GroupListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<GroupList> GetGroupsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupListQueryService listService = new GroupListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupListQueryService queryService = new GroupListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertGroup(GroupPM entityPm)
        {
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            GroupUpdateService service = new GroupUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateGroup(GroupPM entityPm)
        {
            

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            GroupUpdateService service = new GroupUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

    }
}