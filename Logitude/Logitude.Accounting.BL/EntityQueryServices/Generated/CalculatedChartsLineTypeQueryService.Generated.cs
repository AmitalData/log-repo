 
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
   public partial class CalculatedChartsLineTypeQueryService: EntityQueryService<CalculatedChartsLineType,CalculatedChartsLineTypeKeys,CalculatedChartsLineTypePM,object,CalculatedChartsLineTypeKeys>
   {
   
        CalculatedChartsLineTypeRepository repository;
		IAccountingContext  context;
        public CalculatedChartsLineTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CalculatedChartsLineTypeRepository(context);
            Repository = repository;
            mapping = new CalculatedChartsLineTypeDataMapping();
        }

        public CalculatedChartsLineTypeQueryService(CalculatedChartsLineTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CalculatedChartsLineTypeDataMapping();
        }

        public CalculatedChartsLineTypeQueryService(IAccountingContext context)
        {
            this.repository = new CalculatedChartsLineTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CalculatedChartsLineTypeDataMapping();
        }
		 
		public  CalculatedChartsLineTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CalculatedChartsLineTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CalculatedChartsLineType entityPOCO)
        {
            CalculatedChartsLineTypeKeys entityKeys = new CalculatedChartsLineTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 