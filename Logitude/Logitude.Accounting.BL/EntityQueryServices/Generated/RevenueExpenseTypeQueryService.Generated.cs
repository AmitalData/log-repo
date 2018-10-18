 
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
   public partial class RevenueExpenseTypeQueryService: EntityQueryService<RevenueExpenseType,RevenueExpenseTypeKeys,RevenueExpenseTypePM,object,RevenueExpenseTypeKeys>
   {
   
        RevenueExpenseTypeRepository repository;
		IAccountingContext  context;
        public RevenueExpenseTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new RevenueExpenseTypeRepository(context);
            Repository = repository;
            mapping = new RevenueExpenseTypeDataMapping();
        }

        public RevenueExpenseTypeQueryService(RevenueExpenseTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RevenueExpenseTypeDataMapping();
        }

        public RevenueExpenseTypeQueryService(IAccountingContext context)
        {
            this.repository = new RevenueExpenseTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RevenueExpenseTypeDataMapping();
        }
		 
		public  RevenueExpenseTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RevenueExpenseTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RevenueExpenseType entityPOCO)
        {
            RevenueExpenseTypeKeys entityKeys = new RevenueExpenseTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 