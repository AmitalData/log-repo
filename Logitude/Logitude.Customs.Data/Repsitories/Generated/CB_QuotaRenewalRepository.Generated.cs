 
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
   public partial class CB_QuotaRenewalRepository:IRepository<CB_QuotaRenewal>
   {
   
        private ICustomContext currentContext;
        public CB_QuotaRenewalRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_QuotaRenewalRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_QuotaRenewal GetSingle(string id)
        {
            return (from a in context.CB_QuotaRenewals
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_QuotaRenewal> GetAll()
        {
            return from a in context.CB_QuotaRenewals  
                   select a;
        }
				 
        public CB_QuotaRenewal GetSingle(EntityKeyFields entityKeys)
        {
            CB_QuotaRenewalKeys keys = entityKeys as CB_QuotaRenewalKeys;
            return (from a in context.CB_QuotaRenewals
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_QuotaRenewal entity)
        {
            onAdd();
            context.CB_QuotaRenewals.Add(entity);
        }

        public void Remove(CB_QuotaRenewal entity)
        {
            context.CB_QuotaRenewals.Attach(entity);
            context.CB_QuotaRenewals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_QuotaRenewal entity)
        {
            onUpdate();
            context.CB_QuotaRenewals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_QuotaRenewal> All()
        {
            return context.CB_QuotaRenewals.ToList();
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
	 