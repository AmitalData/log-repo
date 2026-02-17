 
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
   public partial class TaxReportLineTypeQueryService: EntityQueryService<TaxReportLineType,TaxReportLineTypeKeys,TaxReportLineTypePM,object,TaxReportLineTypeKeys>
   {
   
        TaxReportLineTypeRepository repository;
		IAccountingContext  context;
        public TaxReportLineTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxReportLineTypeRepository(context);
            Repository = repository;
            mapping = new TaxReportLineTypeDataMapping();
        }

        public TaxReportLineTypeQueryService(TaxReportLineTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxReportLineTypeDataMapping();
        }

        public TaxReportLineTypeQueryService(IAccountingContext context)
        {
            this.repository = new TaxReportLineTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxReportLineTypeDataMapping();
        }
		 
		public  TaxReportLineTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxReportLineTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxReportLineType entityPOCO)
        {
            TaxReportLineTypeKeys entityKeys = new TaxReportLineTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 