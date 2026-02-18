 
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
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class DepositQueryService: EntityQueryService<Deposit,DepositKeys,DepositPM,object,DepositKeys>
   {
   
        DepositRepository repository;
		IAccountingContext  context;
        public DepositQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new DepositRepository(context);
            Repository = repository;
            mapping = new DepositDataMapping();
        }

        public DepositQueryService(DepositRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DepositDataMapping();
        }

        public DepositQueryService(IAccountingContext context)
        {
            this.repository = new DepositRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DepositDataMapping();
        }
		 
		public  DepositPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DepositKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Deposit entityPOCO)
        {
            DepositKeys entityKeys = new DepositKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 