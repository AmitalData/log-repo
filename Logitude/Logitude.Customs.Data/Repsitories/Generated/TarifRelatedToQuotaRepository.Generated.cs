 
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
   public partial class TarifRelatedToQuotaRepository:IRepository<TarifRelatedToQuota>
   {
   
        private ICustomContext currentContext;
        public TarifRelatedToQuotaRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TarifRelatedToQuotaRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TarifRelatedToQuota GetSingle(string code)
        {
            return (from a in context.TarifRelatedToQuotas
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TarifRelatedToQuota> GetAll()
        {
            return from a in context.TarifRelatedToQuotas  
                   select a;
        }
				 
        public TarifRelatedToQuota GetSingle(EntityKeyFields entityKeys)
        {
            TarifRelatedToQuotaKeys keys = entityKeys as TarifRelatedToQuotaKeys;
            return (from a in context.TarifRelatedToQuotas
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TarifRelatedToQuota entity)
        {
            onAdd();
            context.TarifRelatedToQuotas.Add(entity);
        }

        public void Remove(TarifRelatedToQuota entity)
        {
            context.TarifRelatedToQuotas.Attach(entity);
            context.TarifRelatedToQuotas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TarifRelatedToQuota entity)
        {
            onUpdate();
            context.TarifRelatedToQuotas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarifRelatedToQuota> All()
        {
            return context.TarifRelatedToQuotas.ToList();
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
	 