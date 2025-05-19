 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class MagayaStatusRepository:IRepository<MagayaStatus>
   {
   
        private IAccountingContext currentContext;
        public MagayaStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public MagayaStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  MagayaStatus GetSingle(string statuscode)
        {
            return (from a in context.MagayaStatuses
                    where a.StatusCode == statuscode 
                    select a).FirstOrDefault();
        }

        public IQueryable<MagayaStatus> GetAll()
        {
            return from a in context.MagayaStatuses  
                   select a;
        }
				 
        public MagayaStatus GetSingle(EntityKeyFields entityKeys)
        {
            MagayaStatusKeys keys = entityKeys as MagayaStatusKeys;
            return (from a in context.MagayaStatuses
                    where a.StatusCode == keys.StatusCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MagayaStatus entity)
        {
            onAdd();
            context.MagayaStatuses.Add(entity);
        }

        public void Remove(MagayaStatus entity)
        {
            context.MagayaStatuses.Attach(entity);
            context.MagayaStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MagayaStatus entity)
        {
            onUpdate();
            context.MagayaStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MagayaStatus> All()
        {
            return context.MagayaStatuses.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 