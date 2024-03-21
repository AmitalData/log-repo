 
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
   public partial class CB_QuotaRepository:IRepository<CB_Quota>
   {
   
        private ICustomContext currentContext;
        public CB_QuotaRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_QuotaRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_Quota GetSingle(string cb_id)
        {
            return (from a in context.CB_Quotas
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_Quota> GetAll()
        {
            return from a in context.CB_Quotas  
                   select a;
        }
				 
        public CB_Quota GetSingle(EntityKeyFields entityKeys)
        {
            CB_QuotaKeys keys = entityKeys as CB_QuotaKeys;
            return (from a in context.CB_Quotas
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_Quota entity)
        {
            onAdd();
            context.CB_Quotas.Add(entity);
        }

        public void Remove(CB_Quota entity)
        {
            context.CB_Quotas.Attach(entity);
            context.CB_Quotas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_Quota entity)
        {
            onUpdate();
            context.CB_Quotas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_Quota> All()
        {
            return context.CB_Quotas.ToList();
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
	 