 
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
   public partial class ConversationHeaderQueryService: EntityQueryService<ConversationHeader,ConversationHeaderKeys,ConversationHeaderPM,object,ConversationHeaderKeys>
   {
   
        ConversationHeaderRepository repository;
		ISocialContext  context;
        public ConversationHeaderQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new ConversationHeaderRepository(context);
            Repository = repository;
            mapping = new ConversationHeaderDataMapping();
        }

        public ConversationHeaderQueryService(ConversationHeaderRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConversationHeaderDataMapping();
        }

        public ConversationHeaderQueryService(ISocialContext context)
        {
            this.repository = new ConversationHeaderRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConversationHeaderDataMapping();
        }
		 
		public  ConversationHeaderPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConversationHeaderKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConversationHeader entityPOCO)
        {
            ConversationHeaderKeys entityKeys = new ConversationHeaderKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 