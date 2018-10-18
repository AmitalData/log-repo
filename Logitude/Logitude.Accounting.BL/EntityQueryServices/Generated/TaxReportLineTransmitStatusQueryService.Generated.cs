 
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
   public partial class TaxReportLineTransmitStatusQueryService: EntityQueryService<TaxReportLineTransmitStatus,TaxReportLineTransmitStatusKeys,TaxReportLineTransmitStatusPM,object,TaxReportLineTransmitStatusKeys>
   {
   
        TaxReportLineTransmitStatusRepository repository;
		IAccountingContext  context;
        public TaxReportLineTransmitStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxReportLineTransmitStatusRepository(context);
            Repository = repository;
            mapping = new TaxReportLineTransmitStatusDataMapping();
        }

        public TaxReportLineTransmitStatusQueryService(TaxReportLineTransmitStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxReportLineTransmitStatusDataMapping();
        }

        public TaxReportLineTransmitStatusQueryService(IAccountingContext context)
        {
            this.repository = new TaxReportLineTransmitStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxReportLineTransmitStatusDataMapping();
        }
		 
		public  TaxReportLineTransmitStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxReportLineTransmitStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxReportLineTransmitStatus entityPOCO)
        {
            TaxReportLineTransmitStatusKeys entityKeys = new TaxReportLineTransmitStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 