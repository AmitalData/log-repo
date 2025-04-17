 
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
   public partial class SIIRequestStatusRepository:IRepository<SIIRequestStatus>
   {
   
        private ICustomContext currentContext;
        public SIIRequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SIIRequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SIIRequestStatus GetSingle(string code)
        {
            return (from a in context.SIIRequestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SIIRequestStatus> GetAll()
        {
            return from a in context.SIIRequestStatuses  
                   select a;
        }
				 
        public SIIRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            SIIRequestStatusKeys keys = entityKeys as SIIRequestStatusKeys;
            return (from a in context.SIIRequestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SIIRequestStatus entity)
        {
            onAdd();
            context.SIIRequestStatuses.Add(entity);
        }

        public void Remove(SIIRequestStatus entity)
        {
            context.SIIRequestStatuses.Attach(entity);
            context.SIIRequestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SIIRequestStatus entity)
        {
            onUpdate();
            context.SIIRequestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SIIRequestStatus> All()
        {
            return context.SIIRequestStatuses.ToList();
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
	 