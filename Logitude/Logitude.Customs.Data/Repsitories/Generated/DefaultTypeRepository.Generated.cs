 
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
   public partial class DefaultTypeRepository:IRepository<DefaultType>
   {
   
        private ICustomContext currentContext;
        public DefaultTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DefaultTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DefaultType GetSingle(string id, int tenant)
        {
            return (from a in context.DefaultTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DefaultType> GetAll(int tenant)
        {
            return from a in context.DefaultTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DefaultType GetSingle(EntityKeyFields entityKeys)
        {
            DefaultTypeKeys keys = entityKeys as DefaultTypeKeys;
            return (from a in context.DefaultTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DefaultType entity)
        {
            onAdd();
            context.DefaultTypes.Add(entity);
        }

        public void Remove(DefaultType entity)
        {
            context.DefaultTypes.Attach(entity);
            context.DefaultTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DefaultType entity)
        {
            onUpdate();
            context.DefaultTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultType> All()
        {
            return context.DefaultTypes.ToList();
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
	 