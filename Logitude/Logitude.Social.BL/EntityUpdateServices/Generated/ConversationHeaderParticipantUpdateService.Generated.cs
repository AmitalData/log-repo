 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityDataMappings;
using Logitude.Social.Data.Repsitories;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data;

namespace Logitude.Social.BL.EntityUpdateServices
{ 
   public partial class ConversationHeaderParticipantUpdateService:EntityUpdateService<ConversationHeaderParticipant,ConversationHeaderParticipantPM,EntityPM>
   {
   
        ConversationHeaderParticipantRepository entityRepository;
        public ConversationHeaderParticipantUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ISocialContext  context = mainContext as SocialContext;
            context = context ??mainContext as ISocialContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ConversationHeaderParticipantDataMapping();
            Repository = new ConversationHeaderParticipantRepository(context);
        }

       
        private ISocialContext currentContext;
        public ConversationHeaderParticipantUpdateService(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public ConversationHeaderParticipantUpdateService(ISocialContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ConversationHeaderParticipantPM entityPM)
        {
            ConversationHeaderParticipantKeys entityKeys = new ConversationHeaderParticipantKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ConversationHeaderParticipantPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ConversationHeaderParticipantPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 