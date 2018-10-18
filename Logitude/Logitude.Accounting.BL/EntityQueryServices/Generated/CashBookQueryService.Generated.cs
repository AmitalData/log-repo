 
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
   public partial class CashBookQueryService: EntityQueryService<CashBook,CashBookKeys,CashBookPM,object,CashBookKeys>
   {
   
        CashBookRepository repository;
		IAccountingContext  context;
        public CashBookQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CashBookRepository(context);
            Repository = repository;
            mapping = new CashBookDataMapping();
        }

        public CashBookQueryService(CashBookRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CashBookDataMapping();
        }

        public CashBookQueryService(IAccountingContext context)
        {
            this.repository = new CashBookRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CashBookDataMapping();
        }
		 
		public  CashBookPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CashBookKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CashBook entityPOCO)
        {
            CashBookKeys entityKeys = new CashBookKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 