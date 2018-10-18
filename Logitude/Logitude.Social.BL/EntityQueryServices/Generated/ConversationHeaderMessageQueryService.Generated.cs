 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityDataMappings;
using Logitude.Social.Data.Repsitories;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Social.BL.EntityQueryServices
{ 
   public partial class ConversationHeaderMessageQueryService: EntityQueryService<ConversationHeaderMessage,ConversationHeaderMessageKeys,ConversationHeaderMessagePM,object,ConversationHeaderMessageKeys>
   {
   
        ConversationHeaderMessageRepository repository;
		ISocialContext  context;
        public ConversationHeaderMessageQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new ConversationHeaderMessageRepository(context);
            Repository = repository;
            mapping = new ConversationHeaderMessageDataMapping();
        }

        public ConversationHeaderMessageQueryService(ConversationHeaderMessageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConversationHeaderMessageDataMapping();
        }

        public ConversationHeaderMessageQueryService(ISocialContext context)
        {
            this.repository = new ConversationHeaderMessageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConversationHeaderMessageDataMapping();
        }
		 
		public  ConversationHeaderMessagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConversationHeaderMessageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConversationHeaderMessage entityPOCO)
        {
            ConversationHeaderMessageKeys entityKeys = new ConversationHeaderMessageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 