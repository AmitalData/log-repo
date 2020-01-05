 
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
   public partial class InterestReportLineQueryService: EntityQueryService<InterestReportLine,InterestReportLineKeys,InterestReportLinePM,object,InterestReportLineKeys>
   {
   
        InterestReportLineRepository repository;
		IAccountingContext  context;
        public InterestReportLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestReportLineRepository(context);
            Repository = repository;
            mapping = new InterestReportLineDataMapping();
        }

        public InterestReportLineQueryService(InterestReportLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestReportLineDataMapping();
        }

        public InterestReportLineQueryService(IAccountingContext context)
        {
            this.repository = new InterestReportLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestReportLineDataMapping();
        }
		 
		public  InterestReportLinePM GetSingle(string interestreportid, string interesttransactionid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestReportLineKeys(){ InterestReportId = interestreportid, InterestTransactionId = interesttransactionid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestReportLine entityPOCO)
        {
            InterestReportLineKeys entityKeys = new InterestReportLineKeys() { InterestReportId = entityPOCO.InterestReportId, InterestTransactionId = entityPOCO.InterestTransactionId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 