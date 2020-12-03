 
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
   public partial class UserDefinedReportQueryService: EntityQueryService<UserDefinedReport,UserDefinedReportKeys,UserDefinedReportPM,object,UserDefinedReportKeys>
   {
   
        UserDefinedReportRepository repository;
		IAccountingContext  context;
        public UserDefinedReportQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new UserDefinedReportRepository(context);
            Repository = repository;
            mapping = new UserDefinedReportDataMapping();
        }

        public UserDefinedReportQueryService(UserDefinedReportRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UserDefinedReportDataMapping();
        }

        public UserDefinedReportQueryService(IAccountingContext context)
        {
            this.repository = new UserDefinedReportRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UserDefinedReportDataMapping();
        }
		 
		public  UserDefinedReportPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UserDefinedReportKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UserDefinedReport entityPOCO)
        {
            UserDefinedReportKeys entityKeys = new UserDefinedReportKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 