 
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
   public partial class ConfirmationNumberStatusQueryService: EntityQueryService<ConfirmationNumberStatus,ConfirmationNumberStatusKeys,ConfirmationNumberStatusPM,object,ConfirmationNumberStatusKeys>
   {
   
        ConfirmationNumberStatusRepository repository;
		IAccountingContext  context;
        public ConfirmationNumberStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ConfirmationNumberStatusRepository(context);
            Repository = repository;
            mapping = new ConfirmationNumberStatusDataMapping();
        }

        public ConfirmationNumberStatusQueryService(ConfirmationNumberStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConfirmationNumberStatusDataMapping();
        }

        public ConfirmationNumberStatusQueryService(IAccountingContext context)
        {
            this.repository = new ConfirmationNumberStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConfirmationNumberStatusDataMapping();
        }
		 
		public  ConfirmationNumberStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConfirmationNumberStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConfirmationNumberStatus entityPOCO)
        {
            ConfirmationNumberStatusKeys entityKeys = new ConfirmationNumberStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 