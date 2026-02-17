 
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
   public partial class WithholdingTaxDeductionTypeRepository:IRepository<WithholdingTaxDeductionType>
   {
   
        private IAccountingContext currentContext;
        public WithholdingTaxDeductionTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public WithholdingTaxDeductionTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  WithholdingTaxDeductionType GetSingle(string id, int tenant)
        {
            return (from a in context.WithholdingTaxDeductionTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WithholdingTaxDeductionType> GetAll(int tenant)
        {
            return from a in context.WithholdingTaxDeductionTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WithholdingTaxDeductionType GetSingle(EntityKeyFields entityKeys)
        {
            WithholdingTaxDeductionTypeKeys keys = entityKeys as WithholdingTaxDeductionTypeKeys;
            return (from a in context.WithholdingTaxDeductionTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WithholdingTaxDeductionType entity)
        {
            onAdd();
            context.WithholdingTaxDeductionTypes.Add(entity);
        }

        public void Remove(WithholdingTaxDeductionType entity)
        {
            context.WithholdingTaxDeductionTypes.Attach(entity);
            context.WithholdingTaxDeductionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WithholdingTaxDeductionType entity)
        {
            onUpdate();
            context.WithholdingTaxDeductionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WithholdingTaxDeductionType> All()
        {
            return context.WithholdingTaxDeductionTypes.ToList();
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
	 