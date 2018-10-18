 
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
   public partial class TaxReportQueryService: EntityQueryService<TaxReport,TaxReportKeys,TaxReportPM,object,TaxReportKeys>
   {
   
        TaxReportRepository repository;
		IAccountingContext  context;
        public TaxReportQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxReportRepository(context);
            Repository = repository;
            mapping = new TaxReportDataMapping();
        }

        public TaxReportQueryService(TaxReportRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxReportDataMapping();
        }

        public TaxReportQueryService(IAccountingContext context)
        {
            this.repository = new TaxReportRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxReportDataMapping();
        }
		 
		public  TaxReportPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxReportKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxReport entityPOCO)
        {
            TaxReportKeys entityKeys = new TaxReportKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 