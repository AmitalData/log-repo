 
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

namespace Logitude.Social.Data.Repsitories
{
   public partial class ConversationHeaderMessageRepository:IRepository<ConversationHeaderMessage>
   {
        
		public List<ConversationHeaderMessage> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }



        public List<ConversationHeaderMessage> GetAllMessage(string conversationHeaderid, int tenant)
        {

            List<ConversationHeaderMessage> Messages = (from a in context.ConversationHeaderMessages
                                                        where a.ConversationHeaderId == conversationHeaderid && a.Tenant == tenant
                                                        select a).ToList();

            return Messages;
        }

   
   }

}
   