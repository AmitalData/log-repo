 
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
   public partial class TaxDeductionReportQueryService: EntityQueryService<TaxDeductionReport,TaxDeductionReportKeys,TaxDeductionReportPM,object,TaxDeductionReportKeys>
   {
   
        TaxDeductionReportRepository repository;
		IAccountingContext  context;
        public TaxDeductionReportQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxDeductionReportRepository(context);
            Repository = repository;
            mapping = new TaxDeductionReportDataMapping();
        }

        public TaxDeductionReportQueryService(TaxDeductionReportRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxDeductionReportDataMapping();
        }

        public TaxDeductionReportQueryService(IAccountingContext context)
        {
            this.repository = new TaxDeductionReportRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxDeductionReportDataMapping();
        }
		 
		public  TaxDeductionReportPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxDeductionReportKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxDeductionReport entityPOCO)
        {
            TaxDeductionReportKeys entityKeys = new TaxDeductionReportKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 