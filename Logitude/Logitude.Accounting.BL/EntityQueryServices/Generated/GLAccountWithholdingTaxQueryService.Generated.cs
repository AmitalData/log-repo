 
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
   public partial class GLAccountWithholdingTaxQueryService: EntityQueryService<GLAccountWithholdingTax,GLAccountWithholdingTaxKeys,GLAccountWithholdingTaxPM,GLAccountPM,GLAccountKeys>
   {
   
        GLAccountWithholdingTaxRepository repository;
		IAccountingContext  context;
        public GLAccountWithholdingTaxQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountWithholdingTaxRepository(context);
            Repository = repository;
            mapping = new GLAccountWithholdingTaxDataMapping();
        }

        public GLAccountWithholdingTaxQueryService(GLAccountWithholdingTaxRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountWithholdingTaxDataMapping();
        }

        public GLAccountWithholdingTaxQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountWithholdingTaxRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountWithholdingTaxDataMapping();
        }
		 
		public  GLAccountWithholdingTaxPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountWithholdingTaxKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountWithholdingTax entityPOCO)
        {
            GLAccountWithholdingTaxKeys entityKeys = new GLAccountWithholdingTaxKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 