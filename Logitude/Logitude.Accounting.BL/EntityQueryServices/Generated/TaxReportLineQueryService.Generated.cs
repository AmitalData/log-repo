 
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
   public partial class TaxReportLineQueryService: EntityQueryService<TaxReportLine,TaxReportLineKeys,TaxReportLinePM,object,TaxReportLineKeys>
   {
   
        TaxReportLineRepository repository;
		IAccountingContext  context;
        public TaxReportLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxReportLineRepository(context);
            Repository = repository;
            mapping = new TaxReportLineDataMapping();
        }

        public TaxReportLineQueryService(TaxReportLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxReportLineDataMapping();
        }

        public TaxReportLineQueryService(IAccountingContext context)
        {
            this.repository = new TaxReportLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxReportLineDataMapping();
        }
		 
		public  TaxReportLinePM GetSingle(string taxreportid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxReportLineKeys(){ TaxReportId = taxreportid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxReportLine entityPOCO)
        {
            TaxReportLineKeys entityKeys = new TaxReportLineKeys() { TaxReportId = entityPOCO.TaxReportId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 