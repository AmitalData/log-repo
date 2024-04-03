 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class UIMessageRepository:IRepository<UIMessage>
   {
   
        private ICustomContext currentContext;
        public UIMessageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UIMessageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  UIMessage GetSingle(string code)
        {
            return (from a in context.UIMessages
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<UIMessage> GetAll()
        {
            return from a in context.UIMessages  
                   select a;
        }
				 
        public UIMessage GetSingle(EntityKeyFields entityKeys)
        {
            UIMessageKeys keys = entityKeys as UIMessageKeys;
            return (from a in context.UIMessages
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UIMessage entity)
        {
            onAdd();
            context.UIMessages.Add(entity);
        }

        public void Remove(UIMessage entity)
        {
            context.UIMessages.Attach(entity);
            context.UIMessages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UIMessage entity)
        {
            onUpdate();
            context.UIMessages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UIMessage> All()
        {
            return context.UIMessages.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 