 
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
   public partial class InterestReportStatuseQueryService: EntityQueryService<InterestReportStatuse,InterestReportStatuseKeys,InterestReportStatusePM,object,InterestReportStatuseKeys>
   {
   
        InterestReportStatuseRepository repository;
		IAccountingContext  context;
        public InterestReportStatuseQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestReportStatuseRepository(context);
            Repository = repository;
            mapping = new InterestReportStatuseDataMapping();
        }

        public InterestReportStatuseQueryService(InterestReportStatuseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestReportStatuseDataMapping();
        }

        public InterestReportStatuseQueryService(IAccountingContext context)
        {
            this.repository = new InterestReportStatuseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestReportStatuseDataMapping();
        }
		 
		public  InterestReportStatusePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestReportStatuseKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestReportStatuse entityPOCO)
        {
            InterestReportStatuseKeys entityKeys = new InterestReportStatuseKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 