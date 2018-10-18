using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
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
        public GroupMemberPM GetSingleGroupMemberPM(string groupid, string userid, int tenant)
        {
            socialContext = SocialContext.GetContext(tenant);
            groupMemberQuery = new GroupMemberQueryService(socialContext);
            GroupMemberPM groupMember = groupMemberQuery.GetSingle(groupid, userid, false, false);
            return groupMember;
        }

        public GroupMemberList GetSingleGroupMemberList(string groupid, string userid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupMemberListQueryService listService = new GroupMemberListQueryService(socialContext);
            return listService.GetSingle(groupid, userid);
        }

        public List<GroupMemberList> GetGroupMemberLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupMemberListQueryService listService = new GroupMemberListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<GroupMemberList> GetGroupMembersFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupMemberListQueryService listService = new GroupMemberListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetGroupMemberFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            GroupMemberListQueryService queryService = new GroupMemberListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertGroupMember(GroupMemberPM entityPm)
        {
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            GroupMemberUpdateService service = new GroupMemberUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateGroupMember(GroupMemberPM entityPm)
        {
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            GroupMemberUpdateService service = new GroupMemberUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

    }

}