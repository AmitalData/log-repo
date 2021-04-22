 
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
   public partial class GLAccountFollowUpDataQueryService: EntityQueryService<GLAccountFollowUpData,GLAccountFollowUpDataKeys,GLAccountFollowUpDataPM,object,GLAccountFollowUpDataKeys>
   {
   
        GLAccountFollowUpDataRepository repository;
		IAccountingContext  context;
        public GLAccountFollowUpDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountFollowUpDataRepository(context);
            Repository = repository;
            mapping = new GLAccountFollowUpDataDataMapping();
        }

        public GLAccountFollowUpDataQueryService(GLAccountFollowUpDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountFollowUpDataDataMapping();
        }

        public GLAccountFollowUpDataQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountFollowUpDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountFollowUpDataDataMapping();
        }
		 
		public  GLAccountFollowUpDataPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountFollowUpDataKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountFollowUpData entityPOCO)
        {
            GLAccountFollowUpDataKeys entityKeys = new GLAccountFollowUpDataKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 