 
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
   public partial class DefaultValueRepository:IRepository<DefaultValue>
   {
   
        private ICustomContext currentContext;
        public DefaultValueRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DefaultValueRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DefaultValue GetSingle(string id, int tenant)
        {
            return (from a in context.DefaultValues
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DefaultValue> GetAll(int tenant)
        {
            return from a in context.DefaultValues  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DefaultValue GetSingle(EntityKeyFields entityKeys)
        {
            DefaultValueKeys keys = entityKeys as DefaultValueKeys;
            return (from a in context.DefaultValues
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DefaultValue entity)
        {
            onAdd();
            context.DefaultValues.Add(entity);
        }

        public void Remove(DefaultValue entity)
        {
            context.DefaultValues.Attach(entity);
            context.DefaultValues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DefaultValue entity)
        {
            onUpdate();
            context.DefaultValues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultValue> All()
        {
            return context.DefaultValues.ToList();
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
	 