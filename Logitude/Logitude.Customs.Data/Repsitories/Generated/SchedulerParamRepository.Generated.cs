 
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
   public partial class SchedulerParamRepository:IRepository<SchedulerParam>
   {
   
        private ICustomContext currentContext;
        public SchedulerParamRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SchedulerParamRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SchedulerParam GetSingle(string id, int tenant)
        {
            return (from a in context.SchedulerParams
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SchedulerParam> GetAll(int tenant)
        {
            return from a in context.SchedulerParams  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SchedulerParam GetSingle(EntityKeyFields entityKeys)
        {
            SchedulerParamKeys keys = entityKeys as SchedulerParamKeys;
            return (from a in context.SchedulerParams
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SchedulerParam entity)
        {
            onAdd();
            context.SchedulerParams.Add(entity);
        }

        public void Remove(SchedulerParam entity)
        {
            context.SchedulerParams.Attach(entity);
            context.SchedulerParams.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SchedulerParam entity)
        {
            onUpdate();
            context.SchedulerParams.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SchedulerParam> All()
        {
            return context.SchedulerParams.ToList();
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
	 