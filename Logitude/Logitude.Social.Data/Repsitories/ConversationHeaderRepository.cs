 
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Social.Data.Repsitories
{
   public partial class ConversationHeaderRepository:IRepository<ConversationHeader>
   {
        
		public List<ConversationHeader> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }




        public ConversationHeader GetConversationHeaderbyId(string Id )
        {
            ConversationHeader conversationHeaders = (from a in context.ConversationHeaders
                                                      where a.Id == Id
                                                      select a).FirstOrDefault();

            return conversationHeaders;
        }




        public List<ConversationHeader> GetCreateByMeMessage(MessageFilters messageFilters)
        {
            List<ConversationHeader> conversationHeaders = new List<ConversationHeader>();

            conversationHeaders = (from a in context.ConversationHeaders
                                   where a.CreatedByUserId == messageFilters.UserId
                                   select a).ToList();

            return conversationHeaders;
        }


        public int  GetCreateByMeMessageCount(MessageFilters messageFilters)
        {
            int Count;

            Count = (from a in context.ConversationHeaders
                                   where a.CreatedByUserId == messageFilters.UserId
                                   select a).Count();

            return Count;
        }


        public List<ConversationHeader> GetUnreadMessage(MessageFilters messageFilters)
        {
            throw new NotImplementedException();
        }

        public List<ConversationHeader> GetWaitingForResponseMessage(MessageFilters messageFilters)
        {
            throw new NotImplementedException();
        }








   }

}
   