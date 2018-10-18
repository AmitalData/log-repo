 
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
   
        private ISocialContext currentContext;
        public ConversationHeaderMessageRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public ConversationHeaderMessageRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConversationHeaderMessage GetSingle(string id, int tenant)
        {
            return (from a in context.ConversationHeaderMessages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConversationHeaderMessage> GetAll(int tenant)
        {
            return from a in context.ConversationHeaderMessages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConversationHeaderMessage GetSingle(EntityKeyFields entityKeys)
        {
            ConversationHeaderMessageKeys keys = entityKeys as ConversationHeaderMessageKeys;
            return (from a in context.ConversationHeaderMessages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConversationHeaderMessage entity)
        {
            onAdd();
            context.ConversationHeaderMessages.Add(entity);
        }

        public void Remove(ConversationHeaderMessage entity)
        {
            context.ConversationHeaderMessages.Attach(entity);
            context.ConversationHeaderMessages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConversationHeaderMessage entity)
        {
            onUpdate();
            context.ConversationHeaderMessages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConversationHeaderMessage> All()
        {
            return context.ConversationHeaderMessages.ToList();
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
	 