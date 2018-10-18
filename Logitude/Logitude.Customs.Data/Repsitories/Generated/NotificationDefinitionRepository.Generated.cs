 
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
   public partial class NotificationDefinitionRepository:IRepository<NotificationDefinition>
   {
   
        private ICustomContext currentContext;
        public NotificationDefinitionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NotificationDefinitionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NotificationDefinition GetSingle(string code)
        {
            return (from a in context.NotificationDefinitions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<NotificationDefinition> GetAll()
        {
            return from a in context.NotificationDefinitions  
                   select a;
        }
				 
        public NotificationDefinition GetSingle(EntityKeyFields entityKeys)
        {
            NotificationDefinitionKeys keys = entityKeys as NotificationDefinitionKeys;
            return (from a in context.NotificationDefinitions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NotificationDefinition entity)
        {
            onAdd();
            context.NotificationDefinitions.Add(entity);
        }

        public void Remove(NotificationDefinition entity)
        {
            context.NotificationDefinitions.Attach(entity);
            context.NotificationDefinitions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NotificationDefinition entity)
        {
            onUpdate();
            context.NotificationDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NotificationDefinition> All()
        {
            return context.NotificationDefinitions.ToList();
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
	 