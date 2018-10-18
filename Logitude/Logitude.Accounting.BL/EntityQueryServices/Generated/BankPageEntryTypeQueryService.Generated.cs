 
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
   public partial class BankPageEntryTypeQueryService: EntityQueryService<BankPageEntryType,BankPageEntryTypeKeys,BankPageEntryTypePM,object,BankPageEntryTypeKeys>
   {
   
        BankPageEntryTypeRepository repository;
		IAccountingContext  context;
        public BankPageEntryTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankPageEntryTypeRepository(context);
            Repository = repository;
            mapping = new BankPageEntryTypeDataMapping();
        }

        public BankPageEntryTypeQueryService(BankPageEntryTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankPageEntryTypeDataMapping();
        }

        public BankPageEntryTypeQueryService(IAccountingContext context)
        {
            this.repository = new BankPageEntryTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankPageEntryTypeDataMapping();
        }
		 
		public  BankPageEntryTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BankPageEntryTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BankPageEntryType entityPOCO)
        {
            BankPageEntryTypeKeys entityKeys = new BankPageEntryTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 