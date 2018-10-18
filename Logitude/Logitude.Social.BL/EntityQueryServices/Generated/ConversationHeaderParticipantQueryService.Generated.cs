 
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
   public partial class ConversationHeaderParticipantQueryService: EntityQueryService<ConversationHeaderParticipant,ConversationHeaderParticipantKeys,ConversationHeaderParticipantPM,object,ConversationHeaderParticipantKeys>
   {
   
        ConversationHeaderParticipantRepository repository;
		ISocialContext  context;
        public ConversationHeaderParticipantQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new ConversationHeaderParticipantRepository(context);
            Repository = repository;
            mapping = new ConversationHeaderParticipantDataMapping();
        }

        public ConversationHeaderParticipantQueryService(ConversationHeaderParticipantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConversationHeaderParticipantDataMapping();
        }

        public ConversationHeaderParticipantQueryService(ISocialContext context)
        {
            this.repository = new ConversationHeaderParticipantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConversationHeaderParticipantDataMapping();
        }
		 
		public  ConversationHeaderParticipantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConversationHeaderParticipantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConversationHeaderParticipant entityPOCO)
        {
            ConversationHeaderParticipantKeys entityKeys = new ConversationHeaderParticipantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 