 
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
   public partial class OccasionStatusRepository:IRepository<OccasionStatus>
   {
   
        private ICRMContext currentContext;
        public OccasionStatusRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OccasionStatusRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OccasionStatus GetSingle(string code)
        {
            return (from a in context.OccasionStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OccasionStatus> GetAll()
        {
            return from a in context.OccasionStatuses  
                   select a;
        }
				 
        public OccasionStatus GetSingle(EntityKeyFields entityKeys)
        {
            OccasionStatusKeys keys = entityKeys as OccasionStatusKeys;
            return (from a in context.OccasionStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OccasionStatus entity)
        {
            onAdd();
            context.OccasionStatuses.Add(entity);
        }

        public void Remove(OccasionStatus entity)
        {
            context.OccasionStatuses.Attach(entity);
            context.OccasionStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OccasionStatus entity)
        {
            onUpdate();
            context.OccasionStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OccasionStatus> All()
        {
            return context.OccasionStatuses.ToList();
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
	 