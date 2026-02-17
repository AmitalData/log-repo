 
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
   public partial class CashBookTypeQueryService: EntityQueryService<CashBookType,CashBookTypeKeys,CashBookTypePM,object,CashBookTypeKeys>
   {
   
        CashBookTypeRepository repository;
		IAccountingContext  context;
        public CashBookTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CashBookTypeRepository(context);
            Repository = repository;
            mapping = new CashBookTypeDataMapping();
        }

        public CashBookTypeQueryService(CashBookTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CashBookTypeDataMapping();
        }

        public CashBookTypeQueryService(IAccountingContext context)
        {
            this.repository = new CashBookTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CashBookTypeDataMapping();
        }
		 
		public  CashBookTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CashBookTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CashBookType entityPOCO)
        {
            CashBookTypeKeys entityKeys = new CashBookTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 