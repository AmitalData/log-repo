using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Social.BL.EntityQueryServices
{
    public partial class ConversationHeaderQueryService
    {

     public List<ConversationHeaderPM> GetMessagePMsByFilter(MessageFilters messageFilters, bool getcomposition, int tenant)
     {

         ConversationHeaderRepository conversationHeaderRepository = this.Repository as ConversationHeaderRepository;
         List<ConversationHeader> conversationHeaderList = new List<ConversationHeader>();
         List<ConversationHeaderPM> ConversationHeaderPMsList = new List<ConversationHeaderPM>();
         Dictionary<string, string> ParticipantsNames = new Dictionary<string, string>();
         Dictionary<string, int> ParticipantsCount = new Dictionary<string, int>();



         IQueryable<ConversationHeaderParticipant> Participants = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader")
                                                                   where a.ParticipantUserId == messageFilters.UserId && a.Tenant == tenant && !a.IsDelete
                                                                   select a);




         IQueryable<ConversationHeaderMessage> Messages = (from d in context.ConversationHeaderMessages
                                                           where d.Tenant == tenant
                                                           select d);


         if (messageFilters.QueryName == "Unread") Participants = Participants.Where(d => d.IsRead == false && d.Tenant == tenant);
    
         string ParticipantsName = "";
         int i = 0;
           
            List<Contact> contactLists = null;
           
            List<string> contactIds = new List<string>();
            foreach (ConversationHeaderParticipant item in Participants)
            {
                if (!string.IsNullOrEmpty(item.ParticipantUserId))
                {
                    if (!contactIds.Contains(item.ParticipantUserId))
                    {
                        contactIds.Add(item.ParticipantUserId);
                    }

                }

            }

            foreach (ConversationHeaderMessage item in Messages)
            {
                if (!string.IsNullOrEmpty(item.CreatedByUserId))
                {
                    if (!contactIds.Contains(item.CreatedByUserId))
                    {
                        contactIds.Add(item.CreatedByUserId);
                    }

                }

            }
            ContactRepository contactRepository = new ContactRepository(tenant);
            if (contactIds.Count > 0)
            {
                contactLists = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
            }

         IQueryable<ConversationHeaderParticipant> UserParticipants;
         foreach (ConversationHeaderParticipant item in Participants)
         {
             UserParticipants = (from a in context.ConversationHeaderParticipants
                               where a.ConversationHeaderId == item.ConversationHeaderId  &&  a.Tenant == tenant 
                               select a);

                contactIds = new List<string>();
                foreach (ConversationHeaderParticipant itemParticipant in UserParticipants)
                {
                    if (!contactIds.Contains(itemParticipant.ParticipantUserId) && contactLists.Where(d=>d.Id == itemParticipant.ParticipantUserId).FirstOrDefault() == null)
                    {
                        contactIds.Add(itemParticipant.ParticipantUserId);
                    }
                }


                if (contactIds.Count > 0)
                {
                    var contacts = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
                    foreach (Contact contact in contacts)
                    {
                        if (!contactLists.Contains(contact))
                        {
                            contactLists.Add(contact);
                        }
                    }
                }


                foreach (ConversationHeaderParticipant itemParticipant in UserParticipants)
                {
                    ++i;

                    if (i < 4)
                    {
                        var contact = contactLists.Where(d => d.Id == itemParticipant.ParticipantUserId).FirstOrDefault();
                        if (contact != null)
                        {
                            string englishName = contactLists.Where(d => d.Id == itemParticipant.ParticipantUserId).FirstOrDefault().EnglishName;

                            if (i == 3) ParticipantsName += englishName;

                            else ParticipantsName += englishName + ",";
                        }
                    }

                }
          
              i = 0;
           
              ParticipantsNames.Add(item.ConversationHeader.Id, ParticipantsName);
              ParticipantsName = "";

              ParticipantsCount.Add(item.ConversationHeader.Id, UserParticipants.Count());
              conversationHeaderList.Add(item.ConversationHeader);
         }


         if (messageFilters.QueryName == "Waiting for response")
         {
             conversationHeaderList = conversationHeaderList.Where(d => d.IsWaitingForResponse == true).ToList();
         }

         conversationHeaderList = conversationHeaderList.OrderByDescending(f => f.CreateDate).Skip(messageFilters.PageIndex).Take(messageFilters.PageSize).ToList();

         List<ConversationHeaderPM> conversationHeaderPMs = new List<ConversationHeaderPM>();
         foreach (ConversationHeader conversationHeader in conversationHeaderList)
         {
             EntityKeys = new ConversationHeaderKeys() { Id = conversationHeader.Id };
             ConversationHeaderPM conversationHeaderPM = new ConversationHeaderPM();
             mapping.CustomPOCOToPM(conversationHeaderPM, conversationHeader);
             mapping.POCOToPM(conversationHeaderPM, conversationHeader);

             conversationHeaderPM.IsLeft = Participants.Where(s => s.ParticipantUserId == messageFilters.UserId && s.Tenant == tenant && s.ConversationHeaderId == conversationHeaderPM.Id).FirstOrDefault().IsLeft;
             conversationHeaderPM.LeaveDate = Participants.Where(s => s.ParticipantUserId == messageFilters.UserId && s.Tenant == tenant && s.ConversationHeaderId == conversationHeaderPM.Id).FirstOrDefault().LeaveDate;
             ConversationHeaderMessage lastMessage;
             ConversationHeaderMessage firstMessage = Messages.Where(d => d.ConversationHeaderId == conversationHeaderPM.Id && d.Tenant == tenant).FirstOrDefault();

                if (conversationHeaderPM.IsLeft)
                {

                    lastMessage = Messages.Where(d => d.ConversationHeaderId == conversationHeaderPM.Id && d.CreateDate < conversationHeaderPM.LeaveDate && d.Tenant == tenant).OrderByDescending(d => d.CreateDate).FirstOrDefault();

                }
                else
                {
                    lastMessage = Messages.Where(d => d.ConversationHeaderId == conversationHeaderPM.Id && d.Tenant == tenant).OrderByDescending(d => d.CreateDate).FirstOrDefault();
                }


                

                if (lastMessage != null)
                {

                    conversationHeaderPM.LasMessageBody = lastMessage.MessageBody;
                    conversationHeaderPM.LasMessageUserId = lastMessage.CreatedByUserId;
                    conversationHeaderPM.LastMessageDate = lastMessage.CreateDate;

                    var contact = contactLists.Where(d => d.Id == lastMessage.CreatedByUserId).FirstOrDefault();
                    if (contact != null)
                    {
                        conversationHeaderPM.LasMessageUserName = contact.EnglishName;
                        conversationHeaderPM.UserImageDetailId = contact.ImageDetailId;
                    }

                }

                if (firstMessage != null)
                {
                    var firstMessagecontact = contactLists.Where(d => d.Id == firstMessage.CreatedByUserId).FirstOrDefault();
                    conversationHeaderPM.FirstImageDetailId = firstMessagecontact != null ? firstMessagecontact.ImageDetailId : "";

                    conversationHeaderPM.FirstMessageUserId = firstMessage.CreatedByUserId;
                    conversationHeaderPM.FirstMessageBody = firstMessage.MessageBody;
                }



             conversationHeaderPM.MessageParticipants = ParticipantsNames.Where(d => d.Key == conversationHeaderPM.Id ).Select(d => d.Value).FirstOrDefault();
             conversationHeaderPM.MessageParticipantsCount = ParticipantsCount.Where(d => d.Key == conversationHeaderPM.Id).Select(d => d.Value).FirstOrDefault();
             conversationHeaderPM.IsReplied = Participants.Where(s => s.ParticipantUserId == messageFilters.UserId  && s.Tenant == tenant && s.ConversationHeaderId == conversationHeaderPM.Id).FirstOrDefault().Replied;

           
   

             DateTime? LastReadDate =  Participants.Where(d => d.ConversationHeaderId == conversationHeaderPM.Id && d.ParticipantUserId == messageFilters.UserId  && d.Tenant == tenant).FirstOrDefault().LastReadDate;


            if (LastReadDate == null)
            {
                conversationHeaderPM.IsRead = true;
                conversationHeaderPM.NumberUnreadComment = 0;
            }
            else
            {
                conversationHeaderPM.IsRead = false;
                conversationHeaderPM.NumberUnreadComment = Messages.Where(d => d.ConversationHeaderId == conversationHeaderPM.Id && d.CreateDate > LastReadDate && d.CreatedByUserId != messageFilters.UserId).Count();
            }

                if (messageFilters.QueryName == "Unread")
                {
                    if (messageFilters.Technology == "Angular")
                    {
                        if (conversationHeaderPM.NumberUnreadComment > 0) conversationHeaderPMs.Add(conversationHeaderPM);
                    }
                    else conversationHeaderPMs.Add(conversationHeaderPM);
                }
                else conversationHeaderPMs.Add(conversationHeaderPM);
               
         

         }



         if (messageFilters.AreaMessage != "Inbox")
         {

             if (messageFilters.QueryName == "Waiting for response")
             {
                 if (!string.IsNullOrEmpty( messageFilters.EntityId) && !string.IsNullOrEmpty(messageFilters.ObjectTableId))
                 {
                     conversationHeaderPMs = conversationHeaderPMs.Where(d => d.IsWaitingForResponse == true && d.Tenant == tenant && d.CreatedByUserId == messageFilters.UserId && d.EntityId == messageFilters.EntityId && d.ObjectTableId == messageFilters.ObjectTableId).ToList();

                 }
                 else
                 {
                     conversationHeaderPMs = conversationHeaderPMs.Where(d => d.IsWaitingForResponse == true && d.CreatedByUserId == messageFilters.UserId).ToList();
                 }

             }

             else
             {
                    if (!string.IsNullOrEmpty(messageFilters.EntityId) && !string.IsNullOrEmpty(messageFilters.ObjectTableId))
                    {
                        conversationHeaderPMs = conversationHeaderPMs.Where(d => d.EntityId == messageFilters.EntityId && d.Tenant == tenant && d.ObjectTableId == messageFilters.ObjectTableId).ToList();
                    }
             }
         }
         else
         {
             if (messageFilters.QueryName == "Waiting for response")
             {
                  conversationHeaderPMs = conversationHeaderPMs.Where(d => d.IsWaitingForResponse == true && d.CreatedByUserId == messageFilters.UserId).ToList();

             }
            
    
         }

         return conversationHeaderPMs;

     }

     public int GetCountMessagePMsByFilter(MessageFilters messageFilters, bool getcomposition, int tenant)
     {
         int Countmessage = 0;

         if (messageFilters.AreaMessage != "Inbox")
         {

                if (!string.IsNullOrEmpty(messageFilters.EntityId) && !string.IsNullOrEmpty(messageFilters.ObjectTableId))
                {
                    switch (messageFilters.QueryName)
                    {

                        case "All":
                            Countmessage = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader")
                                            where a.ParticipantUserId == messageFilters.UserId && a.Tenant == tenant && a.ConversationHeader.EntityId == messageFilters.EntityId && a.ConversationHeader.ObjectTableId == messageFilters.ObjectTableId && !a.IsDelete
                                            select a).Count();

                            break;

                        case "Unread":
                            Countmessage = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader")
                                            where a.ParticipantUserId == messageFilters.UserId && a.Tenant == tenant && a.IsRead == false && a.ConversationHeader.EntityId == messageFilters.EntityId && a.ConversationHeader.ObjectTableId == messageFilters.ObjectTableId && !a.IsDelete
                                            select a).Count();

                            break;

                        case "Waiting for response":
                            Countmessage = (from a in context.ConversationHeaderParticipants
                                            where a.ParticipantUserId == messageFilters.UserId && a.Tenant == tenant && a.ConversationHeader.IsWaitingForResponse == true && a.ConversationHeader.EntityId == messageFilters.EntityId && a.ConversationHeader.ObjectTableId == messageFilters.ObjectTableId && !a.IsDelete
                                            select a).Count();
                            break;


                        default:

                            break;

                    }
                }

                else Countmessage = GetCount(messageFilters, tenant, Countmessage);
          
         }
         else Countmessage = GetCount(messageFilters, tenant, Countmessage);
        


         return Countmessage;


     }

     private int GetCount(MessageFilters messageFilters, int tenant, int Countmessage)
     {
         switch (messageFilters.QueryName)
         {

             case "All":
                 Countmessage = (from a in context.ConversationHeaderParticipants
                                 where a.ParticipantUserId == messageFilters.UserId && a.Tenant == tenant && !a.IsDelete
                                 select a).Count();

                 break;

             case "Unread":
                 Countmessage = (from a in context.ConversationHeaderParticipants
                                 where a.ParticipantUserId == messageFilters.UserId && a.IsRead == false && a.Tenant == tenant && !a.IsDelete
                                 select a).Count();

                 break;

             case "Waiting for response":
                 Countmessage = (from a in context.ConversationHeaderParticipants
                                 where a.ParticipantUserId == messageFilters.UserId && a.ConversationHeader.IsWaitingForResponse == true && a.Tenant == tenant && !a.IsDelete
                                 select a).Count();
                 break;


             default:

                 break;

         }
         return Countmessage;
     }

     public int GetCountUnReadMessagePMsByFilter(string userid, int tenant, string entityId, string objectTableId, string areaMessage)
     {
         int Countmessage = 0;

         IQueryable<ConversationHeaderParticipant> Participants = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader")
                                                                   where a.ParticipantUserId == userid && a.Tenant == tenant && !a.IsDelete
                                                                   select a);

         IQueryable<ConversationHeaderMessage> Messages = (from d in context.ConversationHeaderMessages
                                                           where d.Tenant == tenant
                                                           select d);


         if (areaMessage != "Inbox")
         {
             if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(objectTableId))
             {
                 foreach (ConversationHeaderParticipant item in Participants)
                 {
                     Countmessage += Messages.Where(d => d.ConversationHeaderId == item.ConversationHeaderId && d.CreateDate > item.LastReadDate && d.CreatedByUserId != userid && item.ConversationHeader.EntityId == entityId && item.ConversationHeader.ObjectTableId == objectTableId).Count();
                 }
             }
             else
             {
                foreach (ConversationHeaderParticipant item in Participants)
                 {

                     Countmessage += Messages.Where(d => d.ConversationHeaderId == item.ConversationHeaderId && d.CreateDate > item.LastReadDate && d.CreatedByUserId != userid).Count();
                 }

             }
         }

         else
         {
             foreach (ConversationHeaderParticipant item in Participants)
             {
                 Countmessage += Messages.Where(d => d.ConversationHeaderId == item.ConversationHeaderId && d.CreateDate > item.LastReadDate && d.CreatedByUserId != userid).Count();
             }

         }
 
         return Countmessage;
     }


    }
}
