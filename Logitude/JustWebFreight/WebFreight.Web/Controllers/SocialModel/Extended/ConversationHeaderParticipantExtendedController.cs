using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.SocialModel.Extended
{
    public class ConversationHeaderParticipantExtendedController : ApiController
    {
       
      public HttpResponseMessage GetAllParticipantsConversationHeaderMessageId(string conversationHeaderId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
                ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
                string result =  listService.GetAllImageIdParticipantListForHeader(conversationHeaderId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetAllConversationHeaderParticipantPMByConversationHeaderId(string conversationHeaderId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
                ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
                List<ConversationHeaderParticipantPM>conversationHeaderParticipantPMList = listService.GetAllParticipantListForHeader(conversationHeaderId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, conversationHeaderParticipantPMList);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        




        public HttpResponseMessage PostSaveConversationHeaderParticipantPMLists(List<ConversationHeaderParticipantPM> conversationHeaderParticipantPMLists)
         {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

                ConversationHeaderParticipantUpdateService service = new ConversationHeaderParticipantUpdateService(socialContext, new Dictionary<string, IContext>(), authToken.Tenant);
                foreach (ConversationHeaderParticipantPM entityPm in conversationHeaderParticipantPMLists)
                {
                    SecurityUtility.AuthenticationOnEntityTenant("ConversationHeaderParticipant", entityPm.Tenant, authToken.Tenant);
                    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    service.Update(entityPm, true);

                }
                
                return Request.CreateResponse(HttpStatusCode.OK, conversationHeaderParticipantPMLists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetMakeMeReadMessage( string conversationHeaderId, string userid)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);

            ConversationHeaderParticipant ConversationHeaderParticipant = conversationHeaderParticipantRepository.GetMyParticipant(conversationHeaderId, userid, authToken.Tenant);
            if (ConversationHeaderParticipant != null)
            {
                ConversationHeaderParticipant.LastReadDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                ConversationHeaderParticipant.IsRead = true;

                conversationHeaderParticipantRepository.SubmitChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, true);

        }

        public HttpResponseMessage GetMakeConversationHeaderParticipantReadMessage( string conversationHeaderId, string userid)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);

            List<ConversationHeaderParticipant> ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetAllParticipant(conversationHeaderId, authToken.Tenant);
            foreach (ConversationHeaderParticipant item in ConversationHeaderParticipants)
            {

                if (item.ParticipantUserId == userid)
                {

                    item.IsRead = true;
                    item.LastReadDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);

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
            return Request.CreateResponse(HttpStatusCode.OK, true);

        }


        public HttpResponseMessage GetMakeConversationHeaderParticipantRepliedOrRead(string conversationHeaderId, string userid, string type)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);

            ConversationHeaderParticipant ConversationHeaderParticipant = conversationHeaderParticipantRepository.GetMyParticipant(conversationHeaderId, userid, authToken.Tenant);

            if (ConversationHeaderParticipant != null)
            {
                if (type == "Replied")
                {
                    ConversationHeaderParticipant.Replied = true;
                }
                else
                    if (type == "Read")
                {
                    ConversationHeaderParticipant.IsRead = true;
                }



                conversationHeaderParticipantRepository.SubmitChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, true);

        }

        public HttpResponseMessage GetMakeDeleteParticipantUnDelete(string conversationHeaderId)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);

            List<ConversationHeaderParticipant> ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetAllParticipant(conversationHeaderId, authToken.Tenant);

            foreach (ConversationHeaderParticipant item in ConversationHeaderParticipants)
            {

                if (item.IsDelete)
                {
                    item.IsDelete = false;
                }


            }
            conversationHeaderParticipantRepository.SubmitChanges();
            return Request.CreateResponse(HttpStatusCode.OK, true);


        }

        public HttpResponseMessage GetDeleteConversationHeaderParticipant(string conversationHeaderId, string userid)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);


            ConversationHeaderParticipantQueryService listService = new ConversationHeaderParticipantQueryService(socialContext);
            string conversationHeaderParticipantPMId = listService.GetMyParticipantPMId(conversationHeaderId, userid, authToken.Tenant);

            if (!string.IsNullOrEmpty(conversationHeaderParticipantPMId))
            {
                ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);
                ConversationHeaderParticipantKeys conversationHeaderParticipantKeysKeys = new ConversationHeaderParticipantKeys() { Id = conversationHeaderParticipantPMId };
                ConversationHeaderParticipant conversationHeaderParticipant = conversationHeaderParticipantRepository.GetSingle(conversationHeaderParticipantKeysKeys);


                if (conversationHeaderParticipant != null)
                {
                    conversationHeaderParticipant.DeleteDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                    conversationHeaderParticipant.IsDelete = true;
                    // conversationHeaderParticipantRepository.Remove(conversationHeaderParticipant);
                    conversationHeaderParticipantRepository.SubmitChanges();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        public HttpResponseMessage GetMakeConversationHeaderParticipantReadAndUnRead(string conversationHeaderId, string userid, string typequery)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
          


            bool IsSuccess = false;
            ConversationHeaderMessageQueryService conversationHeaderMessageQueryService = new ConversationHeaderMessageQueryService(authToken.Tenant);
            ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(authToken.Tenant);

            DateTime? LastMessageDate = conversationHeaderMessageQueryService.GetLastMessageDateForUser(userid, conversationHeaderId, authToken.Tenant);

            ConversationHeaderParticipant ConversationHeaderParticipants = conversationHeaderParticipantRepository.GetConversationHeaderParticipant(conversationHeaderId, authToken.Tenant, userid);
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


            return Request.CreateResponse(HttpStatusCode.OK, IsSuccess);
          

        }


    }
}