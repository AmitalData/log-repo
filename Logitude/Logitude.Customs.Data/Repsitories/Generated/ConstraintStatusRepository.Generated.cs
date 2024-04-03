 
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
   public partial class ConstraintStatusRepository:IRepository<ConstraintStatus>
   {
   
        private ICustomContext currentContext;
        public ConstraintStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConstraintStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConstraintStatus GetSingle(string code)
        {
            return (from a in context.ConstraintStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConstraintStatus> GetAll()
        {
            return from a in context.ConstraintStatuses  
                   select a;
        }
				 
        public ConstraintStatus GetSingle(EntityKeyFields entityKeys)
        {
            ConstraintStatusKeys keys = entityKeys as ConstraintStatusKeys;
            return (from a in context.ConstraintStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConstraintStatus entity)
        {
            onAdd();
            context.ConstraintStatuses.Add(entity);
        }

        public void Remove(ConstraintStatus entity)
        {
            context.ConstraintStatuses.Attach(entity);
            context.ConstraintStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConstraintStatus entity)
        {
            onUpdate();
            context.ConstraintStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConstraintStatus> All()
        {
            return context.ConstraintStatuses.ToList();
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
	 