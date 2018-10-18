 
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
   public partial class ChartOfAccountQueryService: EntityQueryService<ChartOfAccount,ChartOfAccountKeys,ChartOfAccountPM,object,ChartOfAccountKeys>
   {
   
        ChartOfAccountRepository repository;
		IAccountingContext  context;
        public ChartOfAccountQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ChartOfAccountRepository(context);
            Repository = repository;
            mapping = new ChartOfAccountDataMapping();
        }

        public ChartOfAccountQueryService(ChartOfAccountRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ChartOfAccountDataMapping();
        }

        public ChartOfAccountQueryService(IAccountingContext context)
        {
            this.repository = new ChartOfAccountRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ChartOfAccountDataMapping();
        }
		 
		public  ChartOfAccountPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ChartOfAccountKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ChartOfAccount entityPOCO)
        {
            ChartOfAccountKeys entityKeys = new ChartOfAccountKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 