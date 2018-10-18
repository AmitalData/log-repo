 
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
   public partial class BankCodeQueryService: EntityQueryService<BankCode,BankCodeKeys,BankCodePM,object,BankCodeKeys>
   {
   
        BankCodeRepository repository;
		IAccountingContext  context;
        public BankCodeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankCodeRepository(context);
            Repository = repository;
            mapping = new BankCodeDataMapping();
        }

        public BankCodeQueryService(BankCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankCodeDataMapping();
        }

        public BankCodeQueryService(IAccountingContext context)
        {
            this.repository = new BankCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankCodeDataMapping();
        }
		 
		public  BankCodePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BankCodeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BankCode entityPOCO)
        {
            BankCodeKeys entityKeys = new BankCodeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 