 
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
   public partial class CustomsDocumentsDefinitionRepository:IRepository<CustomsDocumentsDefinition>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentsDefinitionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentsDefinitionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentsDefinition GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsDocumentsDefinitions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentsDefinition> GetAll(int tenant)
        {
            return from a in context.CustomsDocumentsDefinitions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsDocumentsDefinition GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentsDefinitionKeys keys = entityKeys as CustomsDocumentsDefinitionKeys;
            return (from a in context.CustomsDocumentsDefinitions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentsDefinition entity)
        {
            onAdd();
            context.CustomsDocumentsDefinitions.Add(entity);
        }

        public void Remove(CustomsDocumentsDefinition entity)
        {
            context.CustomsDocumentsDefinitions.Attach(entity);
            context.CustomsDocumentsDefinitions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentsDefinition entity)
        {
            onUpdate();
            context.CustomsDocumentsDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentsDefinition> All()
        {
            return context.CustomsDocumentsDefinitions.ToList();
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
	 