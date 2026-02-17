 
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
   public partial class BankDepositLineQueryService: EntityQueryService<BankDepositLine,BankDepositLineKeys,BankDepositLinePM,BankDepositPM,BankDepositKeys>
   {
   
        BankDepositLineRepository repository;
		IAccountingContext  context;
        public BankDepositLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankDepositLineRepository(context);
            Repository = repository;
            mapping = new BankDepositLineDataMapping();
        }

        public BankDepositLineQueryService(BankDepositLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankDepositLineDataMapping();
        }

        public BankDepositLineQueryService(IAccountingContext context)
        {
            this.repository = new BankDepositLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankDepositLineDataMapping();
        }
		 
		public  BankDepositLinePM GetSingle(string depositid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BankDepositLineKeys(){ DepositId = depositid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BankDepositLine entityPOCO)
        {
            BankDepositLineKeys entityKeys = new BankDepositLineKeys() { DepositId = entityPOCO.DepositId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 