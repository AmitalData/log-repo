 
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
   public partial class TaxDeductionReportStatusQueryService: EntityQueryService<TaxDeductionReportStatus,TaxDeductionReportStatusKeys,TaxDeductionReportStatusPM,object,TaxDeductionReportStatusKeys>
   {
   
        TaxDeductionReportStatusRepository repository;
		IAccountingContext  context;
        public TaxDeductionReportStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxDeductionReportStatusRepository(context);
            Repository = repository;
            mapping = new TaxDeductionReportStatusDataMapping();
        }

        public TaxDeductionReportStatusQueryService(TaxDeductionReportStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxDeductionReportStatusDataMapping();
        }

        public TaxDeductionReportStatusQueryService(IAccountingContext context)
        {
            this.repository = new TaxDeductionReportStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxDeductionReportStatusDataMapping();
        }
		 
		public  TaxDeductionReportStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxDeductionReportStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxDeductionReportStatus entityPOCO)
        {
            TaxDeductionReportStatusKeys entityKeys = new TaxDeductionReportStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 