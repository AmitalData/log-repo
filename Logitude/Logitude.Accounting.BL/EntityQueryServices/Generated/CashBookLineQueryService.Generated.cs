 
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
   public partial class CashBookLineQueryService: EntityQueryService<CashBookLine,CashBookLineKeys,CashBookLinePM,CashBookPM,CashBookKeys>
   {
   
        CashBookLineRepository repository;
		IAccountingContext  context;
        public CashBookLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CashBookLineRepository(context);
            Repository = repository;
            mapping = new CashBookLineDataMapping();
        }

        public CashBookLineQueryService(CashBookLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CashBookLineDataMapping();
        }

        public CashBookLineQueryService(IAccountingContext context)
        {
            this.repository = new CashBookLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CashBookLineDataMapping();
        }
		 
		public  CashBookLinePM GetSingle(string cashbookid, string arpchequeid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CashBookLineKeys(){ CashBookId = cashbookid, ARPChequeId = arpchequeid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CashBookLine entityPOCO)
        {
            CashBookLineKeys entityKeys = new CashBookLineKeys() { CashBookId = entityPOCO.CashBookId, ARPChequeId = entityPOCO.ARPChequeId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 