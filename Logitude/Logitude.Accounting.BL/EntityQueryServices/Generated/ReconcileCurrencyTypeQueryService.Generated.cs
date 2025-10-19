 
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
   public partial class ReconcileCurrencyTypeQueryService: EntityQueryService<ReconcileCurrencyType,ReconcileCurrencyTypeKeys,ReconcileCurrencyTypePM,object,ReconcileCurrencyTypeKeys>
   {
   
        ReconcileCurrencyTypeRepository repository;
		IAccountingContext  context;
        public ReconcileCurrencyTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconcileCurrencyTypeRepository(context);
            Repository = repository;
            mapping = new ReconcileCurrencyTypeDataMapping();
        }

        public ReconcileCurrencyTypeQueryService(ReconcileCurrencyTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconcileCurrencyTypeDataMapping();
        }

        public ReconcileCurrencyTypeQueryService(IAccountingContext context)
        {
            this.repository = new ReconcileCurrencyTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconcileCurrencyTypeDataMapping();
        }
		 
		public  ReconcileCurrencyTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconcileCurrencyTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReconcileCurrencyType entityPOCO)
        {
            ReconcileCurrencyTypeKeys entityKeys = new ReconcileCurrencyTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 