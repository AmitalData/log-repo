 
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
   public partial class BankDepositQueryService: EntityQueryService<BankDeposit,BankDepositKeys,BankDepositPM,object,BankDepositKeys>
   {
   
        BankDepositRepository repository;
		IAccountingContext  context;
        public BankDepositQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankDepositRepository(context);
            Repository = repository;
            mapping = new BankDepositDataMapping();
        }

        public BankDepositQueryService(BankDepositRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankDepositDataMapping();
        }

        public BankDepositQueryService(IAccountingContext context)
        {
            this.repository = new BankDepositRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankDepositDataMapping();
        }
		 
		public  BankDepositPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BankDepositKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BankDeposit entityPOCO)
        {
            BankDepositKeys entityKeys = new BankDepositKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 