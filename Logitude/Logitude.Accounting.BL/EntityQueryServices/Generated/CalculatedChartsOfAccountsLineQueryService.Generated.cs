 
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
   public partial class CalculatedChartsOfAccountsLineQueryService: EntityQueryService<CalculatedChartsOfAccountsLine,CalculatedChartsOfAccountsLineKeys,CalculatedChartsOfAccountsLinePM,CalculatedChartsOfAccountPM,CalculatedChartsOfAccountKeys>
   {
   
        CalculatedChartsOfAccountsLineRepository repository;
		IAccountingContext  context;
        public CalculatedChartsOfAccountsLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CalculatedChartsOfAccountsLineRepository(context);
            Repository = repository;
            mapping = new CalculatedChartsOfAccountsLineDataMapping();
        }

        public CalculatedChartsOfAccountsLineQueryService(CalculatedChartsOfAccountsLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CalculatedChartsOfAccountsLineDataMapping();
        }

        public CalculatedChartsOfAccountsLineQueryService(IAccountingContext context)
        {
            this.repository = new CalculatedChartsOfAccountsLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CalculatedChartsOfAccountsLineDataMapping();
        }
		 
		public  CalculatedChartsOfAccountsLinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CalculatedChartsOfAccountsLineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CalculatedChartsOfAccountsLine entityPOCO)
        {
            CalculatedChartsOfAccountsLineKeys entityKeys = new CalculatedChartsOfAccountsLineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 