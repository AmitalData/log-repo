 
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
   public partial class ConversationHeaderRepository:IRepository<ConversationHeader>
   {
   
        private ISocialContext currentContext;
        public ConversationHeaderRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public ConversationHeaderRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConversationHeader GetSingle(string id, int tenant)
        {
            return (from a in context.ConversationHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConversationHeader> GetAll(int tenant)
        {
            return from a in context.ConversationHeaders  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConversationHeader GetSingle(EntityKeyFields entityKeys)
        {
            ConversationHeaderKeys keys = entityKeys as ConversationHeaderKeys;
            return (from a in context.ConversationHeaders
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConversationHeader entity)
        {
            onAdd();
            context.ConversationHeaders.Add(entity);
        }

        public void Remove(ConversationHeader entity)
        {
            context.ConversationHeaders.Attach(entity);
            context.ConversationHeaders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConversationHeader entity)
        {
            onUpdate();
            context.ConversationHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConversationHeader> All()
        {
            return context.ConversationHeaders.ToList();
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
	 