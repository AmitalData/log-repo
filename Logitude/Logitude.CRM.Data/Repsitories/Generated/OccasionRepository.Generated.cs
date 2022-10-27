 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class OccasionRepository:IRepository<Occasion>
   {
   
        private ICRMContext currentContext;
        public OccasionRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OccasionRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Occasion GetSingle(string id, int tenant)
        {
            return (from a in context.Occasions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Occasion> GetAll(int tenant)
        {
            return from a in context.Occasions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Occasion GetSingle(EntityKeyFields entityKeys)
        {
            OccasionKeys keys = entityKeys as OccasionKeys;
            return (from a in context.Occasions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Occasion entity)
        {
            onAdd();
            context.Occasions.Add(entity);
        }

        public void Remove(Occasion entity)
        {
            context.Occasions.Attach(entity);
            context.Occasions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Occasion entity)
        {
            onUpdate();
            context.Occasions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Occasion> All()
        {
            return context.Occasions.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 