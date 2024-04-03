 
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
   public partial class PhysicalCheckRepository:IRepository<PhysicalCheck>
   {
   
        private ICustomContext currentContext;
        public PhysicalCheckRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PhysicalCheckRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PhysicalCheck GetSingle(string id, int tenant)
        {
            return (from a in context.PhysicalChecks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PhysicalCheck> GetAll(int tenant)
        {
            return from a in context.PhysicalChecks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PhysicalCheck GetSingle(EntityKeyFields entityKeys)
        {
            PhysicalCheckKeys keys = entityKeys as PhysicalCheckKeys;
            return (from a in context.PhysicalChecks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PhysicalCheck entity)
        {
            onAdd();
            context.PhysicalChecks.Add(entity);
        }

        public void Remove(PhysicalCheck entity)
        {
            context.PhysicalChecks.Attach(entity);
            context.PhysicalChecks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PhysicalCheck entity)
        {
            onUpdate();
            context.PhysicalChecks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PhysicalCheck> All()
        {
            return context.PhysicalChecks.ToList();
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
	 