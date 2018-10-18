 
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
   public partial class AmendmentRequestStatusRepository:IRepository<AmendmentRequestStatus>
   {
   
        private ICustomContext currentContext;
        public AmendmentRequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentRequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendmentRequestStatus GetSingle(string code)
        {
            return (from a in context.AmendmentRequestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendmentRequestStatus> GetAll()
        {
            return from a in context.AmendmentRequestStatuses  
                   select a;
        }
				 
        public AmendmentRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            AmendmentRequestStatusKeys keys = entityKeys as AmendmentRequestStatusKeys;
            return (from a in context.AmendmentRequestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendmentRequestStatus entity)
        {
            onAdd();
            context.AmendmentRequestStatuses.Add(entity);
        }

        public void Remove(AmendmentRequestStatus entity)
        {
            context.AmendmentRequestStatuses.Attach(entity);
            context.AmendmentRequestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendmentRequestStatus entity)
        {
            onUpdate();
            context.AmendmentRequestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendmentRequestStatus> All()
        {
            return context.AmendmentRequestStatuses.ToList();
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
	 