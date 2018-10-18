 
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
   public partial class PriorityRepository:IRepository<Priority>
   {
   
        private ICRMContext currentContext;
        public PriorityRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public PriorityRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Priority GetSingle(string code)
        {
            return (from a in context.ActivityPriorities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Priority> GetAll()
        {
            return from a in context.ActivityPriorities  
                   select a;
        }
				 
        public Priority GetSingle(EntityKeyFields entityKeys)
        {
            PriorityKeys keys = entityKeys as PriorityKeys;
            return (from a in context.Priorities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 
        public void Add(Priority entity)
        {
            context.Priorities.Add(entity);
        }

        public void Remove(Priority entity)
        {
            context.Priorities.Attach(entity);
            context.Priorities.Remove(entity);
        }

        public void Update(Priority entity)
        {
            context.Priorities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Priority> All()
        {
            return context.Priorities.ToList();
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
	 