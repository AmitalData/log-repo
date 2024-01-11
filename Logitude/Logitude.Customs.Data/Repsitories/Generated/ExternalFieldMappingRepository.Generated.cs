 
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
   public partial class ExternalFieldMappingRepository:IRepository<ExternalFieldMapping>
   {
   
        private ICustomContext currentContext;
        public ExternalFieldMappingRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExternalFieldMappingRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExternalFieldMapping GetSingle(string id, int tenant)
        {
            return (from a in context.ExternalFieldMappings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExternalFieldMapping> GetAll(int tenant)
        {
            return from a in context.ExternalFieldMappings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExternalFieldMapping GetSingle(EntityKeyFields entityKeys)
        {
            ExternalFieldMappingKeys keys = entityKeys as ExternalFieldMappingKeys;
            return (from a in context.ExternalFieldMappings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExternalFieldMapping entity)
        {
            onAdd();
            context.ExternalFieldMappings.Add(entity);
        }

        public void Remove(ExternalFieldMapping entity)
        {
            context.ExternalFieldMappings.Attach(entity);
            context.ExternalFieldMappings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExternalFieldMapping entity)
        {
            onUpdate();
            context.ExternalFieldMappings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalFieldMapping> All()
        {
            return context.ExternalFieldMappings.ToList();
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
	 