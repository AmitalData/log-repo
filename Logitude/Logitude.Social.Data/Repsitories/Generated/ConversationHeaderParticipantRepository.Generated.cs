 
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
   public partial class ConversationHeaderParticipantRepository:IRepository<ConversationHeaderParticipant>
   {
   
        private ISocialContext currentContext;
        public ConversationHeaderParticipantRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public ConversationHeaderParticipantRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConversationHeaderParticipant GetSingle(string id, int tenant)
        {
            return (from a in context.ConversationHeaderParticipants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConversationHeaderParticipant> GetAll(int tenant)
        {
            return from a in context.ConversationHeaderParticipants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConversationHeaderParticipant GetSingle(EntityKeyFields entityKeys)
        {
            ConversationHeaderParticipantKeys keys = entityKeys as ConversationHeaderParticipantKeys;
            return (from a in context.ConversationHeaderParticipants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConversationHeaderParticipant entity)
        {
            onAdd();
            context.ConversationHeaderParticipants.Add(entity);
        }

        public void Remove(ConversationHeaderParticipant entity)
        {
            context.ConversationHeaderParticipants.Attach(entity);
            context.ConversationHeaderParticipants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConversationHeaderParticipant entity)
        {
            onUpdate();
            context.ConversationHeaderParticipants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConversationHeaderParticipant> All()
        {
            return context.ConversationHeaderParticipants.ToList();
        }

        private ISocialContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 