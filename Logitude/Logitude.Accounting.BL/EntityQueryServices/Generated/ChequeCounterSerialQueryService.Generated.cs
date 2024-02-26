 
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
   public partial class ChequeCounterSerialQueryService: EntityQueryService<ChequeCounterSerial,ChequeCounterSerialKeys,ChequeCounterSerialPM,BankAccountPM,BankAccountKeys>
   {
   
        ChequeCounterSerialRepository repository;
		IAccountingContext  context;
        public ChequeCounterSerialQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ChequeCounterSerialRepository(context);
            Repository = repository;
            mapping = new ChequeCounterSerialDataMapping();
        }

        public ChequeCounterSerialQueryService(ChequeCounterSerialRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ChequeCounterSerialDataMapping();
        }

        public ChequeCounterSerialQueryService(IAccountingContext context)
        {
            this.repository = new ChequeCounterSerialRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ChequeCounterSerialDataMapping();
        }
		 
		public  ChequeCounterSerialPM GetSingle(int seriesid, string bankaccountid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ChequeCounterSerialKeys(){ SeriesId = seriesid, BankAccountId = bankaccountid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ChequeCounterSerial entityPOCO)
        {
            ChequeCounterSerialKeys entityKeys = new ChequeCounterSerialKeys() { SeriesId = entityPOCO.SeriesId, BankAccountId = entityPOCO.BankAccountId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 