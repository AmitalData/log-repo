using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ConversationHeaderParticipantPM GetSingleConversationHeaderParticipantPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            socialContext = SocialContext.GetContext(tenant);
            ConversationHeaderParticipantQueryService conversationHeaderParticipantQuery = new ConversationHeaderParticipantQueryService(socialContext);
            ConversationHeaderParticipantPM conversationHeaderParticipant = conversationHeaderParticipantQuery.GetSingle(id, false, false);
            return conversationHeaderParticipant;
        }

        public ConversationHeaderParticipantList GetSingleConversationHeaderParticipantList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderParticipantListQueryService listService = new ConversationHeaderParticipantListQueryService(socialContext);
            return listService.GetSingle(id);
        }

        public List<ConversationHeaderParticipantList> GetConversationHeaderParticipantLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderParticipantListQueryService listService = new ConversationHeaderParticipantListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<ConversationHeaderParticipantList> GetConversationHeaderParticipantsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderParticipantListQueryService listService = new ConversationHeaderParticipantListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetConversationHeaderParticipantFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            ConversationHeaderParticipantListQueryService queryService = new ConversationHeaderParticipantListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertConversationHeaderParticipant(ConversationHeaderParticipantPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            ConversationHeaderParticipantUpdateService service = new ConversationHeaderParticipantUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateConversationHeaderParticipant(ConversationHeaderParticipantPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
          //  SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ConversationHeaderParticipantUpdateService service = new ConversationHeaderParticipantUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

     


        [Invoke]
        public bool EditConversationHeaderParticipantRead(int tenant, string ConversationHeaderId, string userid , DateTime Lastreadmessage)
        {


            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);

            List<ConversationHeaderParticipant> ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetAllParticipant(ConversationHeaderId, tenant);
            foreach (ConversationHeaderParticipant item  in ConversationHeaderParticipants)
            {

                if (item.ParticipantUserId == userid)
                {
               
                    item.IsRead = true;
                    item.LastReadDate = TenantServerConfigration.GetCurrentDateTime(tenant);//Lastreadmessage;

                }
                else
                {
                    if (item.IsLeft == false)
                    {
                        item.IsRead = false;
                    }
                }
        
             }
            conversationHeaderParticipantRepository.SubmitChanges();
                return true;

        }



        [Invoke]
        public bool MakeMeReadMessage(int tenant, string ConversationHeaderId, string userid, DateTime Lastreadmessage)
        {


            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);
          

            ConversationHeaderParticipant ConversationHeaderParticipant = conversationHeaderParticipantRepository.GetMyParticipant(ConversationHeaderId, userid, tenant);
            if (ConversationHeaderParticipant != null)
            {
                ConversationHeaderParticipant.LastReadDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                ConversationHeaderParticipant.IsRead = true;
          
                conversationHeaderParticipantRepository.SubmitChanges();
            }
            return true;

        }





        [Invoke]
        public bool MakeConversationHeaderParticipantReadAndUnRead(int tenant, string ConversationHeaderId, string userid , string typequery)
        {

            bool IsSuccess = false;
            ConversationHeaderMessageQueryService conversationHeaderMessageQueryService = new ConversationHeaderMessageQueryService(tenant);
            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);

            DateTime? LastMessageDate = conversationHeaderMessageQueryService.GetLastMessageDateForUser(userid, ConversationHeaderId, tenant);

            ConversationHeaderParticipant ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetConversationHeaderParticipant(ConversationHeaderId, tenant, userid);
            if (typequery == "Mark as Unread")
            {
           
                if (ConversationHeaderParticipants != null)
                {
           
                    if (LastMessageDate != null)
                    {
                        ConversationHeaderParticipants.IsRead = false;

                        ConversationHeaderParticipants.LastReadDate = LastMessageDate.Value.AddSeconds(-1);
                        IsSuccess = true;


                        conversationHeaderParticipantRepository.SubmitChanges();
                    }
            
                }
            }

            else
            {
                //MarkRead
                if (ConversationHeaderParticipants != null)
                {
           
                    if (LastMessageDate != null)
                    {
                        ConversationHeaderParticipants.LastReadDate = LastMessageDate.Value.AddSeconds(1);
                        IsSuccess = true;
                        ConversationHeaderParticipants.IsRead = true;

                        conversationHeaderParticipantRepository.SubmitChanges();
                    }
          
                }
            }



            return IsSuccess;

        }



        [Invoke]
        public bool EditConversationHeaderParticipantLeft(int tenant, string ConversationHeaderId, string userid)
        {
            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);

            ConversationHeaderParticipant ConversationHeaderParticipant = conversationHeaderParticipantRepository.GetMyParticipant(ConversationHeaderId, userid, tenant);
            if (ConversationHeaderParticipant != null)
            {
                ConversationHeaderParticipant.IsLeft = true;
                ConversationHeaderParticipant.LeaveDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                conversationHeaderParticipantRepository.SubmitChanges();
            }
            return true;

        }









      [Invoke]
        public bool EditConversationHeaderParticipantRepliedOrRead(int tenant, string ConversationHeaderId, string userid , string Type)
        {
            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);

            ConversationHeaderParticipant ConversationHeaderParticipant = conversationHeaderParticipantRepository.GetMyParticipant(ConversationHeaderId, userid, tenant);

            if (ConversationHeaderParticipant != null)
            {
                if (Type == "Replied")
                {
                    ConversationHeaderParticipant.Replied = true;
                }
                else
                    if (Type == "Read")
                    {
                        ConversationHeaderParticipant.IsRead = true;
                    }



                conversationHeaderParticipantRepository.SubmitChanges();
            }
                return true;

        }




      public List<ConversationHeaderParticipantPM> GetAllParticipantsHeader(int tenant, string conversationHeaderId)
      {

          List<ConversationHeaderParticipantPM> ConversationHeaderParticipantPMList = new List<ConversationHeaderParticipantPM>();
          SecurityUtility.AuthenticationOnTenant(tenant);
          //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

          if (socialContext == null)
          {
              socialContext = SocialContext.GetContext(tenant);
          }

          ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
          ConversationHeaderParticipantPMList = listService.GetAllParticipantListForHeader(conversationHeaderId, tenant);

          return ConversationHeaderParticipantPMList;
      }


      [Invoke]
      public string GetAllParticipantsConversationHeaderMessageId(int tenant, string conversationHeaderId)
      {
          SecurityUtility.AuthenticationOnTenant(tenant);
          //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
          if (socialContext == null)
          {
              socialContext = SocialContext.GetContext(tenant);
          }


          ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
          return listService.GetAllImageIdParticipantListForHeader(conversationHeaderId, tenant);

      }


      //public void DeleteConversationHeaderParticipantPMs(ConversationHeaderParticipantPM entitypm)
      //{
      //    ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(entitypm.Tenant);

      //    ConversationHeaderParticipantKeys conversationHeaderParticipantKeysKeys = new ConversationHeaderParticipantKeys() { Id = entitypm.Id };
      //    ConversationHeaderParticipant conversationHeaderParticipant = conversationHeaderParticipantRepository.GetSingle(conversationHeaderParticipantKeysKeys);

      //    conversationHeaderParticipantRepository.Remove(conversationHeaderParticipant);
   

      //}



      //public ConversationHeaderParticipantPM GetMyConversationHeaderParticipantPM(string conversationHeaderId , string userid, int tenant)
      //{

      //    SecurityUtility.AuthenticationOnTenant(tenant);
      //    SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
      //    socialContext = SocialContext.GetContext(tenant);
      //    ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
      //    return listService.GetMyParticipantPM(conversationHeaderId, userid, tenant);
         
      //}


      [Invoke]
      public void DeleteConversationHeaderParticipant(string ConversationHeaderId, string userid, int tenant )
      {

          if (socialContext == null)
          {
              socialContext = SocialContext.GetContext(tenant);
          }

          ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
          string conversationHeaderParticipantPMId = listService.GetMyParticipantPMId(ConversationHeaderId, userid, tenant);

          if  (!string.IsNullOrEmpty(conversationHeaderParticipantPMId))
          {
              ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);
              ConversationHeaderParticipantKeys conversationHeaderParticipantKeysKeys = new ConversationHeaderParticipantKeys() { Id = conversationHeaderParticipantPMId };
              ConversationHeaderParticipant conversationHeaderParticipant = conversationHeaderParticipantRepository.GetSingle(conversationHeaderParticipantKeysKeys);


              if (conversationHeaderParticipant != null)
              {
                 conversationHeaderParticipant.DeleteDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                  conversationHeaderParticipant.IsDelete = true;
                 // conversationHeaderParticipantRepository.Remove(conversationHeaderParticipant);
                  conversationHeaderParticipantRepository.SubmitChanges();
              }
          }
      }









      [Invoke]
      public bool MakeDeleteParticipantUnDelete(int tenant, string ConversationHeaderId)
      {


          ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(tenant);

          List<ConversationHeaderParticipant> ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetAllParticipant(ConversationHeaderId, tenant);

          foreach (ConversationHeaderParticipant item in ConversationHeaderParticipants)
          {

              if (item.IsDelete)
              {
                  item.IsDelete = false;
              }
       

          }
          conversationHeaderParticipantRepository.SubmitChanges();
          return true;

      }










    }
}