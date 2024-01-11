 
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
   public partial class EntityTypeLookupRepository:IRepository<EntityTypeLookup>
   {
   
        private ICustomContext currentContext;
        public EntityTypeLookupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public EntityTypeLookupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  EntityTypeLookup GetSingle(string code)
        {
            return (from a in context.EntityTypeLookups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<EntityTypeLookup> GetAll()
        {
            return from a in context.EntityTypeLookups  
                   select a;
        }
				 
        public EntityTypeLookup GetSingle(EntityKeyFields entityKeys)
        {
            EntityTypeLookupKeys keys = entityKeys as EntityTypeLookupKeys;
            return (from a in context.EntityTypeLookups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EntityTypeLookup entity)
        {
            onAdd();
            context.EntityTypeLookups.Add(entity);
        }

        public void Remove(EntityTypeLookup entity)
        {
            context.EntityTypeLookups.Attach(entity);
            context.EntityTypeLookups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EntityTypeLookup entity)
        {
            onUpdate();
            context.EntityTypeLookups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntityTypeLookup> All()
        {
            return context.EntityTypeLookups.ToList();
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
	 