 
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
   public partial class PeriodTypeQueryService: EntityQueryService<PeriodType,PeriodTypeKeys,PeriodTypePM,object,PeriodTypeKeys>
   {
   
        PeriodTypeRepository repository;
		IAccountingContext  context;
        public PeriodTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new PeriodTypeRepository(context);
            Repository = repository;
            mapping = new PeriodTypeDataMapping();
        }

        public PeriodTypeQueryService(PeriodTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PeriodTypeDataMapping();
        }

        public PeriodTypeQueryService(IAccountingContext context)
        {
            this.repository = new PeriodTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PeriodTypeDataMapping();
        }
		 
		public  PeriodTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PeriodTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PeriodType entityPOCO)
        {
            PeriodTypeKeys entityKeys = new PeriodTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 