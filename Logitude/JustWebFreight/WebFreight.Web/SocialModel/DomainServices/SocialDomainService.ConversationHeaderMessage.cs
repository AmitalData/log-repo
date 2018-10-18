using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
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

        public ConversationHeaderMessagePM GetSingleConversationHeaderMessagePM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);


            socialContext = SocialContext.GetContext(tenant);
            ConversationHeaderMessageQueryService conversationHeaderMessageQuery = new ConversationHeaderMessageQueryService(socialContext);
            ConversationHeaderMessagePM conversationHeaderMessage = conversationHeaderMessageQuery.GetSingle(id, false, false);
            return conversationHeaderMessage;
        }

        public ConversationHeaderMessageList GetSingleConversationHeaderMessageList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderMessageListQueryService listService = new ConversationHeaderMessageListQueryService(socialContext);
            return listService.GetSingle(id);
        }

        public List<ConversationHeaderMessageList> GetConversationHeaderMessageLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderMessageListQueryService listService = new ConversationHeaderMessageListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<ConversationHeaderMessageList> GetConversationHeaderMessagesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderMessageListQueryService listService = new ConversationHeaderMessageListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetConversationHeaderMessageFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderMessageListQueryService queryService = new ConversationHeaderMessageListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

     


      public void InsertConversationHeaderMessage(ConversationHeaderMessagePM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }


            ConversationHeaderRepository conversationHeaderRepository = new ConversationHeaderRepository(entityPm.Tenant);
            ConversationHeaderKeys EntityKeys = new ConversationHeaderKeys() { Id = entityPm.ConversationHeaderId };
          ConversationHeader conversationHeader = conversationHeaderRepository.GetSingle(EntityKeys);
  

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            ConversationHeaderMessageUpdateService service = new ConversationHeaderMessageUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            if (conversationHeader.CreatedByUserId != entityPm.CreatedByUserId)
            {
                if (conversationHeader.IsWaitingForResponse)
                {
                    conversationHeader.IsWaitingForResponse = false;
                }
                conversationHeaderRepository.SubmitChanges();
            }
          
          
        }


        public void UpdateConversationHeaderMessage(ConversationHeaderMessagePM entityPm)

                 
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ConversationHeaderMessageUpdateService service = new ConversationHeaderMessageUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }


        public List<ConversationHeaderMessagePM> GetAllConversationMessageForHeader(string conversationHeaderId, int tenant , string userid)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }



            ConversationHeaderMessageQueryService service = new ConversationHeaderMessageQueryService(socialContext);
            return service.GetAllConversationMessageListForHeader(conversationHeaderId, userid, tenant);

        }



        public void DeleteConversationHeaderMessagePMs(ConversationHeaderMessagePM entitypm)
        {
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entitypm.Tenant);
            }

            ConversationHeaderMessageRepository conversationHeaderMessageRepository = new ConversationHeaderMessageRepository(entitypm.Tenant);

            List<ConversationHeaderMessage> ConversationHeaderMessages = conversationHeaderMessageRepository.GetAllMessage(entitypm.Id, entitypm.Tenant);
            foreach (ConversationHeaderMessage item in ConversationHeaderMessages)
            {
                ConversationHeaderMessageKeys conversationHeaderMessageKeysKeys = new ConversationHeaderMessageKeys() { Id = item.Id };
                ConversationHeaderMessage conversationHeaderMessage = conversationHeaderMessageRepository.GetSingle(conversationHeaderMessageKeysKeys);
                conversationHeaderMessageRepository.Remove(conversationHeaderMessage);
            }

        }

    }
}