 
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
   public partial class GLAccountInterestPeriodQueryService: EntityQueryService<GLAccountInterestPeriod,GLAccountInterestPeriodKeys,GLAccountInterestPeriodPM,GLAccountPM,GLAccountKeys>
   {
   
        GLAccountInterestPeriodRepository repository;
		IAccountingContext  context;
        public GLAccountInterestPeriodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountInterestPeriodRepository(context);
            Repository = repository;
            mapping = new GLAccountInterestPeriodDataMapping();
        }

        public GLAccountInterestPeriodQueryService(GLAccountInterestPeriodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountInterestPeriodDataMapping();
        }

        public GLAccountInterestPeriodQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountInterestPeriodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountInterestPeriodDataMapping();
        }
		 
		public  GLAccountInterestPeriodPM GetSingle(int linenumber, string glaccountid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountInterestPeriodKeys(){ LineNumber = linenumber, GLAccountId = glaccountid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountInterestPeriod entityPOCO)
        {
            GLAccountInterestPeriodKeys entityKeys = new GLAccountInterestPeriodKeys() { LineNumber = entityPOCO.LineNumber, GLAccountId = entityPOCO.GLAccountId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 