 
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
   public partial class ChartOfAccountsTypeQueryService: EntityQueryService<ChartOfAccountsType,ChartOfAccountsTypeKeys,ChartOfAccountsTypePM,object,ChartOfAccountsTypeKeys>
   {
   
        ChartOfAccountsTypeRepository repository;
		IAccountingContext  context;
        public ChartOfAccountsTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ChartOfAccountsTypeRepository(context);
            Repository = repository;
            mapping = new ChartOfAccountsTypeDataMapping();
        }

        public ChartOfAccountsTypeQueryService(ChartOfAccountsTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ChartOfAccountsTypeDataMapping();
        }

        public ChartOfAccountsTypeQueryService(IAccountingContext context)
        {
            this.repository = new ChartOfAccountsTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ChartOfAccountsTypeDataMapping();
        }
		 
		public  ChartOfAccountsTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ChartOfAccountsTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ChartOfAccountsType entityPOCO)
        {
            ChartOfAccountsTypeKeys entityKeys = new ChartOfAccountsTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 