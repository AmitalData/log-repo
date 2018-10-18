 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class WithholdingTaxDeductionTypeQueryService: EntityQueryService<WithholdingTaxDeductionType,WithholdingTaxDeductionTypeKeys,WithholdingTaxDeductionTypePM,object,WithholdingTaxDeductionTypeKeys>
   {
   
        WithholdingTaxDeductionTypeRepository repository;
		IAccountingContext  context;
        public WithholdingTaxDeductionTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new WithholdingTaxDeductionTypeRepository(context);
            Repository = repository;
            mapping = new WithholdingTaxDeductionTypeDataMapping();
        }

        public WithholdingTaxDeductionTypeQueryService(WithholdingTaxDeductionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WithholdingTaxDeductionTypeDataMapping();
        }

        public WithholdingTaxDeductionTypeQueryService(IAccountingContext context)
        {
            this.repository = new WithholdingTaxDeductionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WithholdingTaxDeductionTypeDataMapping();
        }
		 
		public  WithholdingTaxDeductionTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WithholdingTaxDeductionTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WithholdingTaxDeductionType entityPOCO)
        {
            WithholdingTaxDeductionTypeKeys entityKeys = new WithholdingTaxDeductionTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 