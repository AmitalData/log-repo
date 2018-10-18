using Logitude.Social.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityLists;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Logitude.Social.BL.EntityQueryServices
{
   public partial class ConversationHeaderMessageQueryService
    {

       public List<ConversationHeaderMessagePM> GetAllConversationMessageListForHeader(string conversationHeaderId, string userid ,int tenant)
       {

           ColorIndexRepository colorIndexRepository = new ColorIndexRepository(tenant);
           IQueryable<ConversationHeaderMessage> conversationHeaderMessageList;
           List<ConversationHeaderMessagePM> conversationHeaderMessagePMs = new List<ConversationHeaderMessagePM>();
          
           ConversationHeaderParticipant conversationHeaderParticipant = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader").Include("ParticipantUser").Include("ParticipantUser.Contact")
                                          where a.ConversationHeaderId == conversationHeaderId && a.Tenant == tenant && a.ParticipantUserId == userid
                                          select a).FirstOrDefault();


           if (conversationHeaderParticipant != null)
           {

               if (conversationHeaderParticipant.DeleteDate != null && !conversationHeaderParticipant.IsDelete)
               {
                   conversationHeaderMessageList = (from a in context.ConversationHeaderMessages.Include("ConversationHeader").Include("CreatedByUser").Include("CreatedByUser.Contact")
                                                    where a.ConversationHeaderId == conversationHeaderId && a.CreateDate > conversationHeaderParticipant.DeleteDate
                                                    orderby a.CreateDate
                                                    select a);

               }
               else
               {
                   conversationHeaderMessageList = (from a in context.ConversationHeaderMessages.Include("ConversationHeader").Include("CreatedByUser").Include("CreatedByUser.Contact")
                                                    where a.ConversationHeaderId == conversationHeaderId
                                                    orderby a.CreateDate
                                                    select a);

               }
           
               foreach (ConversationHeaderMessage conversationHeaderMessage in conversationHeaderMessageList)
               {
                   EntityKeys = new ConversationHeaderMessageKeys() { Id = conversationHeaderMessage.Id };
                   ConversationHeaderMessagePM conversationMessagePM = new ConversationHeaderMessagePM();
                   mapping.CustomPOCOToPM(conversationMessagePM, conversationHeaderMessage);
                   mapping.POCOToPM(conversationMessagePM, conversationHeaderMessage);
                   conversationMessagePM.UserName = conversationHeaderMessage.CreatedByUser.Contact.EnglishName;
                   conversationMessagePM.UserImageDetailId = conversationHeaderMessage.CreatedByUser.Contact.ImageDetailId;

                 
                   conversationMessagePM.DefaultColor = colorIndexRepository.GetSingleHasColor(conversationHeaderMessage.CreatedByUser.Contact.IndexColor);
        

                   conversationHeaderMessagePMs.Add(conversationMessagePM);
               }
             
           }

           return conversationHeaderMessagePMs;
       }

       public DateTime? GetLastMessageDateForUser(string userid, string ConversationHeaderId, int tenant)
       {
           DateTime? LastMessageDate = null;
         
           ConversationHeaderMessage item = (from a in context.ConversationHeaderMessages
                                             where a.ConversationHeaderId == ConversationHeaderId && a.Tenant == tenant && a.CreatedByUserId != userid
                                             select a).OrderByDescending(f => f.CreateDate).FirstOrDefault();
           if (item != null)
           {
               LastMessageDate = item.CreateDate ;
           }
           return LastMessageDate;
                                                                        
       }
    }


}
