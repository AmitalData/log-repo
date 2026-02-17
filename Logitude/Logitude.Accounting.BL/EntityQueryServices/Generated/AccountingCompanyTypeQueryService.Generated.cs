 
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
   public partial class AccountingCompanyTypeQueryService: EntityQueryService<AccountingCompanyType,AccountingCompanyTypeKeys,AccountingCompanyTypePM,object,AccountingCompanyTypeKeys>
   {
   
        AccountingCompanyTypeRepository repository;
		IAccountingContext  context;
        public AccountingCompanyTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingCompanyTypeRepository(context);
            Repository = repository;
            mapping = new AccountingCompanyTypeDataMapping();
        }

        public AccountingCompanyTypeQueryService(AccountingCompanyTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingCompanyTypeDataMapping();
        }

        public AccountingCompanyTypeQueryService(IAccountingContext context)
        {
            this.repository = new AccountingCompanyTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingCompanyTypeDataMapping();
        }
		 
		public  AccountingCompanyTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingCompanyTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingCompanyType entityPOCO)
        {
            AccountingCompanyTypeKeys entityKeys = new AccountingCompanyTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 