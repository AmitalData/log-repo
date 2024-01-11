 
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
   public partial class CancellationRequestStatusRepository:IRepository<CancellationRequestStatus>
   {
   
        private ICustomContext currentContext;
        public CancellationRequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CancellationRequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CancellationRequestStatus GetSingle(string code)
        {
            return (from a in context.CancellationRequestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CancellationRequestStatus> GetAll()
        {
            return from a in context.CancellationRequestStatuses  
                   select a;
        }
				 
        public CancellationRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            CancellationRequestStatusKeys keys = entityKeys as CancellationRequestStatusKeys;
            return (from a in context.CancellationRequestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CancellationRequestStatus entity)
        {
            onAdd();
            context.CancellationRequestStatuses.Add(entity);
        }

        public void Remove(CancellationRequestStatus entity)
        {
            context.CancellationRequestStatuses.Attach(entity);
            context.CancellationRequestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CancellationRequestStatus entity)
        {
            onUpdate();
            context.CancellationRequestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CancellationRequestStatus> All()
        {
            return context.CancellationRequestStatuses.ToList();
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
	 