 
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
   public partial class StageRepository:IRepository<Stage>
   {
   
        private ICRMContext currentContext;
        public StageRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public StageRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Stage GetSingle(string id, int tenant)
        {
            return (from a in context.Stages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Stage> GetAll(int tenant)
        {
            return from a in context.Stages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Stage GetSingle(EntityKeyFields entityKeys)
        {
            StageKeys keys = entityKeys as StageKeys;
            return (from a in context.Stages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Stage entity)
        {
            onAdd();
            context.Stages.Add(entity);
        }

        public void Remove(Stage entity)
        {
            context.Stages.Attach(entity);
            context.Stages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Stage entity)
        {
            onUpdate();
            context.Stages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Stage> All()
        {
            return context.Stages.ToList();
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
	 