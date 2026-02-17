 
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
   public partial class TaxReportLineStatusQueryService: EntityQueryService<TaxReportLineStatus,TaxReportLineStatusKeys,TaxReportLineStatusPM,object,TaxReportLineStatusKeys>
   {
   
        TaxReportLineStatusRepository repository;
		IAccountingContext  context;
        public TaxReportLineStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxReportLineStatusRepository(context);
            Repository = repository;
            mapping = new TaxReportLineStatusDataMapping();
        }

        public TaxReportLineStatusQueryService(TaxReportLineStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxReportLineStatusDataMapping();
        }

        public TaxReportLineStatusQueryService(IAccountingContext context)
        {
            this.repository = new TaxReportLineStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxReportLineStatusDataMapping();
        }
		 
		public  TaxReportLineStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxReportLineStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxReportLineStatus entityPOCO)
        {
            TaxReportLineStatusKeys entityKeys = new TaxReportLineStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 