 
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
   public partial class ConfirmationNumberDefaultQueryService: EntityQueryService<ConfirmationNumberDefault,ConfirmationNumberDefaultKeys,ConfirmationNumberDefaultPM,object,ConfirmationNumberDefaultKeys>
   {
   
        ConfirmationNumberDefaultRepository repository;
		IAccountingContext  context;
        public ConfirmationNumberDefaultQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ConfirmationNumberDefaultRepository(context);
            Repository = repository;
            mapping = new ConfirmationNumberDefaultDataMapping();
        }

        public ConfirmationNumberDefaultQueryService(ConfirmationNumberDefaultRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConfirmationNumberDefaultDataMapping();
        }

        public ConfirmationNumberDefaultQueryService(IAccountingContext context)
        {
            this.repository = new ConfirmationNumberDefaultRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConfirmationNumberDefaultDataMapping();
        }
		 
		public  ConfirmationNumberDefaultPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConfirmationNumberDefaultKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConfirmationNumberDefault entityPOCO)
        {
            ConfirmationNumberDefaultKeys entityKeys = new ConfirmationNumberDefaultKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 