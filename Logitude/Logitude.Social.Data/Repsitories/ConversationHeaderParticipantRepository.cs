 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Social.Data.EntityLists;

namespace Logitude.Social.Data.Repsitories
{
   public partial class ConversationHeaderParticipantRepository:IRepository<ConversationHeaderParticipant>
   {
        
		public List<ConversationHeaderParticipant> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<ConversationHeaderParticipant> GetAllParticipant(string conversationHeaderid, int tenant)
        {

            List<ConversationHeaderParticipant> Participants = (from a in context.ConversationHeaderParticipants
                                                                where a.ConversationHeaderId == conversationHeaderid && a.Tenant == tenant
                                                                select a).ToList();

            return Participants;
        }





        public ConversationHeaderParticipant GetConversationHeaderParticipant(string conversationHeaderid, int tenant, string userid)
        {

            ConversationHeaderParticipant Participant = (from a in context.ConversationHeaderParticipants
                                                                where a.ConversationHeaderId == conversationHeaderid && a.Tenant == tenant && a.ParticipantUserId == userid
                                                                select a).FirstOrDefault();

            return Participant;
        }

        public ConversationHeaderParticipant GetMyParticipant(string conversationHeaderid, string userid, int tenant)
        {

            return (from a in context.ConversationHeaderParticipants
                    where a.ConversationHeaderId == conversationHeaderid && a.ParticipantUserId == userid &&  a.Tenant == tenant
                    select a).FirstOrDefault();

       
        }


        //public IQueryable<ConversationHeaderParticipantList> GetAllParticipantForHeader(string conversationHeaderId, int tenant)
        //{
        //    IQueryable<ConversationHeaderParticipantList> myResult = from a in context.ConversationHeaderParticipants.Include("ParticipantUser").Include("ParticipantUser.Contact")
        //                                                             where a.ConversationHeaderId == conversationHeaderId
        //                                                             select new ConversationHeaderParticipantList()
        //                                                             {
        //                                                                 Id = a.Id,
        //                                                                 DeleteDate = a.DeleteDate,
        //                                                                 CreateDate = a.CreateDate,
        //                                                                 ParticipantUserId = a.ParticipantUserId,
        //                                                                 ConversationHeaderId = a.ConversationHeaderId,
        //                                                                 IsRead = a.IsRead,
        //                                                                 IsDelete = a.IsDelete,
        //                                                                 IsLeft = a.IsLeft,

        //                                                             };

        //    return myResult;
        //}



        public IQueryable<ConversationHeaderParticipantList> GetAllParticipantForHeader(string conversationHeaderId, int tenant)
        {

            IQueryable<ConversationHeaderParticipantList> myResult = from a in context.ConversationHeaderParticipants
                                                                     where a.ConversationHeaderId == conversationHeaderId && a.Tenant == tenant
                                                                     select new ConversationHeaderParticipantList()
                                                                     {
                                                                         Id = a.Id,
                                                                         DeleteDate = a.DeleteDate,
                                                                         CreateDate = a.CreateDate,
                                                                         ParticipantUserId = a.ParticipantUserId,
                                                                         ConversationHeaderId = a.ConversationHeaderId,
                                                                         IsRead = a.IsRead,
                                                                         IsDelete = a.IsDelete,
                                                                         IsLeft = a.IsLeft,

                                                                     };

            return myResult;


        }









   }

}
   