 
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
   public partial class GLAccountTotalByMonthQueryService: EntityQueryService<GLAccountTotalByMonth,GLAccountTotalByMonthKeys,GLAccountTotalByMonthPM,object,GLAccountTotalByMonthKeys>
   {
   
        GLAccountTotalByMonthRepository repository;
		IAccountingContext  context;
        public GLAccountTotalByMonthQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountTotalByMonthRepository(context);
            Repository = repository;
            mapping = new GLAccountTotalByMonthDataMapping();
        }

        public GLAccountTotalByMonthQueryService(GLAccountTotalByMonthRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountTotalByMonthDataMapping();
        }

        public GLAccountTotalByMonthQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountTotalByMonthRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountTotalByMonthDataMapping();
        }
		 
		public  GLAccountTotalByMonthPM GetSingle(string accountid, string datetypecode, int year, int month, string currencyid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountTotalByMonthKeys(){ AccountId = accountid, DateTypeCode = datetypecode, Year = year, Month = month, CurrencyId = currencyid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountTotalByMonth entityPOCO)
        {
            GLAccountTotalByMonthKeys entityKeys = new GLAccountTotalByMonthKeys() { AccountId = entityPOCO.AccountId, DateTypeCode = entityPOCO.DateTypeCode, Year = entityPOCO.Year, Month = entityPOCO.Month, CurrencyId = entityPOCO.CurrencyId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 