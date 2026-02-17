 
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
   public partial class AutomaticExternalRconcilMthodQueryService: EntityQueryService<AutomaticExternalRconcilMthod,AutomaticExternalRconcilMthodKeys,AutomaticExternalRconcilMthodPM,object,AutomaticExternalRconcilMthodKeys>
   {
   
        AutomaticExternalRconcilMthodRepository repository;
		IAccountingContext  context;
        public AutomaticExternalRconcilMthodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AutomaticExternalRconcilMthodRepository(context);
            Repository = repository;
            mapping = new AutomaticExternalRconcilMthodDataMapping();
        }

        public AutomaticExternalRconcilMthodQueryService(AutomaticExternalRconcilMthodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AutomaticExternalRconcilMthodDataMapping();
        }

        public AutomaticExternalRconcilMthodQueryService(IAccountingContext context)
        {
            this.repository = new AutomaticExternalRconcilMthodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AutomaticExternalRconcilMthodDataMapping();
        }
		 
		public  AutomaticExternalRconcilMthodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AutomaticExternalRconcilMthodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AutomaticExternalRconcilMthod entityPOCO)
        {
            AutomaticExternalRconcilMthodKeys entityKeys = new AutomaticExternalRconcilMthodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 