using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.SocialModel.DomainServices
{
    public partial class SocialDomainService
    {
        public ConversationHeaderPM GetSingleConversationHeaderPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            socialContext = SocialContext.GetContext(tenant);
            ConversationHeaderQueryService conversationHeaderQuery = new ConversationHeaderQueryService(socialContext);
           ConversationHeaderPM conversationHeader = conversationHeaderQuery.GetSingle(id, false, false);
           return conversationHeader;
        }

        public ConversationHeaderList GetSingleConversationHeaderList(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
      

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderListQueryService listService = new ConversationHeaderListQueryService(socialContext);
            return listService.GetSingle(id);
        }

        public List<ConversationHeaderList> GetConversationHeaderLists(int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

     

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderListQueryService listService = new ConversationHeaderListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<ConversationHeaderList> GetConversationHeadersFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderListQueryService listService = new ConversationHeaderListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetConversationHeaderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderListQueryService queryService = new ConversationHeaderListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertConversationHeader(ConversationHeaderPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            ConversationHeaderUpdateService service = new ConversationHeaderUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateConversationHeader(ConversationHeaderPM entityPm)
        {

            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ConversationHeaderUpdateService service = new ConversationHeaderUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }


        [Invoke]
        public int GetCountConversationHeaderPMsQuery(byte[] filters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(filters);
            XmlSerializer serializer = new XmlSerializer(typeof(MessageFilters));
            MessageFilters messageFilters = (MessageFilters)serializer.Deserialize(memorystream);

            ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);
            return service.GetCountMessagePMsByFilter(messageFilters, true, tenant);
        
        }


        public List<ConversationHeaderPM> GetConversationHeaderPMs(byte[] filters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(filters);
            XmlSerializer serializer = new XmlSerializer(typeof(MessageFilters));
            MessageFilters messageFilters = (MessageFilters)serializer.Deserialize(memorystream);

            ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);
            return service.GetMessagePMsByFilter(messageFilters, true, tenant);
        }

        public void DeleteConversationHeaderPMs(ConversationHeaderPM entitypm)
        {
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entitypm.Tenant);
            }


            ConversationHeaderRepository conversationHeaderRepository = new ConversationHeaderRepository(entitypm.Tenant);
            ConversationHeaderKeys conversationHeaderKeys = new ConversationHeaderKeys() { Id = entitypm.Id };
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            ConversationHeader entity = conversationHeaderRepository.GetSingle(conversationHeaderKeys);
            conversationHeaderRepository.Remove(entity);
      
        }



        [Invoke]
        public int GetCountUnReadConversationHeaderPMsQuery(string userid, int tenant, string entityId, string objectTableId ,string areaMessage)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);
            return service.GetCountUnReadMessagePMsByFilter(userid, tenant, entityId, objectTableId, areaMessage);
        
        }




        [Invoke]
        public string GetMyImageDetailIdQuery(string userid, int tenant)
        {

            string ImageDetailId = "";
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM item = contactQuery.GetSinglePM(userid, tenant);

            if (item != null)
            {
                ImageDetailId = item.ImageDetailId;
            }
            return ImageDetailId;

        }


        [Invoke]
        public int GetCountOpenActivitiesQuery(string userid, int tenant, string entityId, string objectTableId, string areaMessage)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //if (socialContext == null)
            //{
            //    socialContext = SocialContext.GetContext(tenant);
            //}

            //ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);
            //return service.GetCountUnReadMessagePMsByFilter(userid, tenant, entityId, objectTableId, areaMessage);
            return 0;

        }
    }
}